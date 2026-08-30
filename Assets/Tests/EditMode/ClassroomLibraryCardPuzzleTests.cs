using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class ClassroomLibraryCardPuzzleTests
    {
        private readonly List<GameObject> created = new List<GameObject>();
        private readonly List<ClassroomLibraryCardPuzzle> puzzles =
            new List<ClassroomLibraryCardPuzzle>();

        [TearDown]
        public void TearDown()
        {
            foreach (ClassroomLibraryCardPuzzle puzzle in puzzles)
            {
                if (puzzle != null)
                {
                    EditModeTestLifecycle.Invoke(puzzle, "OnDisable");
                }
            }

            foreach (GameObject item in created)
            {
                if (item != null)
                {
                    Object.DestroyImmediate(item);
                }
            }

            puzzles.Clear();
            created.Clear();
        }

        [Test]
        public void BroadcastSkip_ChainsSearchIntroThenOpensInvestigations()
        {
            PuzzleFixture fixture = CreateFixture();

            Assert.That(fixture.Broadcast.Trigger(), Is.True);
            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.WaitingForOpening));
            fixture.Dialogue.SkipDialogue();

            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.PlayingSearchIntro));
            Assert.That(fixture.Dialogue.CurrentSpeaker, Is.EqualTo("主角"));
            Assert.That(fixture.Puzzle.CanInvestigate(
                InvestigationKind.Blackboard), Is.False);

            fixture.Dialogue.SkipDialogue();

            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.Searching));
            Assert.That(fixture.Puzzle.CanInvestigate(
                InvestigationKind.Blackboard), Is.True);
        }

        [Test]
        public void BroadcastNormalCompletion_AlsoChainsSearchIntro()
        {
            PuzzleFixture fixture = CreateFixture();
            Assert.That(fixture.Broadcast.Trigger(), Is.True);

            fixture.Dialogue.Tick(1f);
            Assert.That(fixture.Dialogue.TryAdvanceDialogue(),
                Is.EqualTo(DialogueAdvanceResult.Completed));

            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.PlayingSearchIntro));
            fixture.Dialogue.Tick(10f);
            Assert.That(fixture.Body.text,
                Is.EqualTo("肯定不是放在别的同学的课桌。"));
        }

        [TestCase(InvestigationKind.StorageCabinet,
            "储物柜里没有。看来也没人把它随手塞进来。")]
        [TestCase(InvestigationKind.Podium,
            "教授的桌子上也没有。最好别在这里乱翻。")]
        [TestCase(InvestigationKind.TeacherDesk,
            "讲台下面空空的。")]
        [TestCase(InvestigationKind.Blackboard,
            "黑板附近也没有。总不可能夹在粉笔槽里吧。")]
        public void OrdinaryInvestigation_UsesApprovedTextAndCanRepeat(
            InvestigationKind kind, string expectedText)
        {
            PuzzleFixture fixture = CreateFixture();
            OpenSearching(fixture);

            Assert.That(fixture.Puzzle.TryInvestigate(kind), Is.True);
            fixture.Dialogue.Tick(10f);
            Assert.That(fixture.Body.text, Is.EqualTo(expectedText));
            fixture.Dialogue.SkipDialogue();
            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.Searching));

            Assert.That(fixture.Puzzle.TryInvestigate(kind), Is.True);
        }

        [Test]
        public void FirstFlowerpotInvestigation_UsesApprovedPagesThenUnlocksAllChairs()
        {
            PuzzleFixture fixture = CreateFixture();
            OpenSearching(fixture);
            Assert.That(fixture.Chairs[0].GetComponent<WorldItem>().CanPickUp, Is.False);
            Assert.That(fixture.Chairs[1].GetComponent<WorldItem>().CanPickUp, Is.False);

            Assert.That(fixture.Puzzle.TryInvestigate(
                InvestigationKind.Flowerpot), Is.True);
            DialoguePage[] pages = GetField<DialoguePage[]>(
                fixture.Puzzle, "firstFlowerpotPages");
            Assert.That(pages, Has.Length.EqualTo(7));
            Assert.That(pages[0].Text, Is.EqualTo("花盆下面……好像压着什么。"));
            Assert.That(pages[1].Text, Is.EqualTo("是我的借书卡。"));
            Assert.That(pages[2].Text, Is.EqualTo("花盆纹丝不动。真够沉的。"));
            Assert.That(pages[3].Text, Is.EqualTo("把这种东西藏在这里，很好玩吗？"));
            Assert.That(pages[4].Text,
                Is.EqualTo("就因为我脸上的痕迹，他们就觉得可以随便欺负我。"));
            Assert.That(pages[5].Text,
                Is.EqualTo("生气又能怎么样……我总不能在教室里和所有人打一架。"));
            Assert.That(pages[6].Text,
                Is.EqualTo("得找个够结实的东西，把花盆砸开。"));
            Assert.That(fixture.Chairs[0].GetComponent<WorldItem>().CanPickUp, Is.False);

            fixture.Dialogue.SkipDialogue();

            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.ChairsUnlocked));
            Assert.That(fixture.Chairs[0].GetComponent<WorldItem>().CanPickUp, Is.True);
            Assert.That(fixture.Chairs[1].GetComponent<WorldItem>().CanPickUp, Is.True);
        }

        [Test]
        public void ReinvestigatingIntactFlowerpot_UsesApprovedRepeatLine()
        {
            PuzzleFixture fixture = CreateFixture();
            OpenSearching(fixture);
            fixture.Puzzle.TryInvestigate(InvestigationKind.Flowerpot);
            fixture.Dialogue.SkipDialogue();

            Assert.That(fixture.Puzzle.TryInvestigate(
                InvestigationKind.Flowerpot), Is.True);
            fixture.Dialogue.Tick(10f);

            Assert.That(fixture.Body.text,
                Is.EqualTo("还是推不动。得找个东西把它砸开。"));
        }

        [Test]
        public void BreakingFlowerpot_RevealsCardAndPlaysApprovedLine()
        {
            PuzzleFixture fixture = CreateFixture();
            OpenSearching(fixture);
            fixture.Puzzle.TryInvestigate(InvestigationKind.Flowerpot);
            fixture.Dialogue.SkipDialogue();
            ThrowableChair chair = fixture.Chairs[0];
            chair.BeginThrow(Vector2.right);

            Assert.That(fixture.Flowerpot.TryBreak(chair), Is.True);
            fixture.Dialogue.Tick(10f);

            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.PotBroken));
            Assert.That(fixture.Card.gameObject.activeSelf, Is.True);
            Assert.That(fixture.Body.text, Is.EqualTo("碎了。借书卡就在下面。"));
        }

        [Test]
        public void PickingUpRevealedCard_CompletesPuzzleOnlyOnceAndKeepsDropSupport()
        {
            PuzzleFixture fixture = CreateFixture();
            OpenSearching(fixture);
            fixture.Puzzle.TryInvestigate(InvestigationKind.Flowerpot);
            fixture.Dialogue.SkipDialogue();
            fixture.Chairs[0].BeginThrow(Vector2.right);
            fixture.Flowerpot.TryBreak(fixture.Chairs[0]);
            fixture.Dialogue.SkipDialogue();

            fixture.Card.PickUp();
            fixture.Dialogue.Tick(10f);

            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.Completed));
            Assert.That(fixture.Body.text,
                Is.EqualTo("终于拿回来了。该去图书馆了。"));
            fixture.Dialogue.SkipDialogue();

            fixture.Card.Drop(Vector2.zero);
            Assert.That(fixture.Card.gameObject.activeSelf, Is.True);
            fixture.Card.PickUp();
            Assert.That(fixture.Dialogue.IsPlaying, Is.False);
        }

        [Test]
        public void RevealedCard_EInteraction_EntersInventoryAndCanBeDroppedAndRepicked()
        {
            PuzzleFixture fixture = CreateFixture();
            OpenSearching(fixture);
            fixture.Puzzle.TryInvestigate(InvestigationKind.Flowerpot);
            fixture.Dialogue.SkipDialogue();
            fixture.Chairs[0].BeginThrow(Vector2.right);
            fixture.Flowerpot.TryBreak(fixture.Chairs[0]);
            fixture.Dialogue.SkipDialogue();
            EditModeTestLifecycle.Invoke(
                fixture.Card, "OnTriggerEnter2D", fixture.PlayerCollider);

            Assert.That(fixture.Interaction.TryInteract(), Is.True);
            Assert.That(fixture.Inventory.GetSlotItem(0), Is.SameAs(fixture.Card));
            Assert.That(fixture.Puzzle.State,
                Is.EqualTo(ClassroomPuzzleState.Completed));
            fixture.Dialogue.Tick(10f);
            Assert.That(fixture.Body.text,
                Is.EqualTo("终于拿回来了。该去图书馆了。"));
            fixture.Dialogue.SkipDialogue();

            EditModeTestLifecycle.Invoke(fixture.Inventory, "DropSelected");
            Assert.That(fixture.Card.gameObject.activeSelf, Is.True);
            EditModeTestLifecycle.Invoke(
                fixture.Card, "OnTriggerEnter2D", fixture.PlayerCollider);
            Assert.That(fixture.Interaction.TryInteract(), Is.True);
            Assert.That(fixture.Inventory.GetSlotItem(0), Is.SameAs(fixture.Card));
            Assert.That(fixture.Dialogue.IsPlaying, Is.False);
        }

        private void OpenSearching(PuzzleFixture fixture)
        {
            fixture.Broadcast.Trigger();
            fixture.Dialogue.SkipDialogue();
            fixture.Dialogue.SkipDialogue();
        }

        private PuzzleFixture CreateFixture()
        {
            GameObject player = Track(new GameObject("Player"));
            player.AddComponent<Rigidbody2D>();
            CircleCollider2D playerCollider = player.AddComponent<CircleCollider2D>();
            MainCharacterMovement movement = player.AddComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.AddComponent<PlayerInventory>();
            PlayerInteraction interaction = player.AddComponent<PlayerInteraction>();
            DialogueController dialogue = player.AddComponent<DialogueController>();
            EditModeTestLifecycle.Invoke(inventory, "Awake");
            EditModeTestLifecycle.Invoke(interaction, "Awake");

            DialogueHUD hud = Track(new GameObject("HUD")).AddComponent<DialogueHUD>();
            GameObject panel = Track(new GameObject("Panel"));
            Text speaker = Track(new GameObject("Speaker")).AddComponent<Text>();
            Text body = Track(new GameObject("Body")).AddComponent<Text>();
            Text hint = Track(new GameObject("Hint")).AddComponent<Text>();
            SetField(hud, "panel", panel);
            SetField(hud, "speakerText", speaker);
            SetField(hud, "bodyText", body);
            SetField(hud, "hintText", hint);
            SetField(dialogue, "hud", hud);
            SetField(dialogue, "movement", movement);
            SetField(dialogue, "playerInteraction", interaction);
            SetField(dialogue, "playerInventory", inventory);

            BroadcastDialogueTrigger broadcast = Track(new GameObject("Broadcast"))
                .AddComponent<BroadcastDialogueTrigger>();
            SetField(broadcast, "controller", dialogue);
            SetField(broadcast, "pages", OnePage("教授", "原广播"));

            ClassroomLibraryCardPuzzle puzzle = Track(new GameObject("Puzzle"))
                .AddComponent<ClassroomLibraryCardPuzzle>();
            SetField(puzzle, "dialogueController", dialogue);
            SetField(puzzle, "openingBroadcast", broadcast);
            SetField(puzzle, "searchIntroPages",
                OnePage("主角", "肯定不是放在别的同学的课桌。"));
            SetField(puzzle, "storageCabinetPages",
                OnePage("主角", "储物柜里没有。看来也没人把它随手塞进来。"));
            SetField(puzzle, "podiumPages",
                OnePage("主角", "教授的桌子上也没有。最好别在这里乱翻。"));
            SetField(puzzle, "teacherDeskPages",
                OnePage("主角", "讲台下面空空的。"));
            SetField(puzzle, "blackboardPages",
                OnePage("主角", "黑板附近也没有。总不可能夹在粉笔槽里吧。"));
            SetField(puzzle, "firstFlowerpotPages", new[]
            {
                new DialoguePage("主角", "花盆下面……好像压着什么。"),
                new DialoguePage("主角", "是我的借书卡。"),
                new DialoguePage("主角", "花盆纹丝不动。真够沉的。"),
                new DialoguePage("主角", "把这种东西藏在这里，很好玩吗？"),
                new DialoguePage("主角", "就因为我脸上的痕迹，他们就觉得可以随便欺负我。"),
                new DialoguePage("主角", "生气又能怎么样……我总不能在教室里和所有人打一架。"),
                new DialoguePage("主角", "得找个够结实的东西，把花盆砸开。")
            });
            SetField(puzzle, "repeatFlowerpotPages",
                OnePage("主角", "还是推不动。得找个东西把它砸开。"));
            SetField(puzzle, "flowerpotBrokenPages",
                OnePage("主角", "碎了。借书卡就在下面。"));
            SetField(puzzle, "cardPickedUpPages",
                OnePage("主角", "终于拿回来了。该去图书馆了。"));

            ThrowableChair[] chairs = new ThrowableChair[2];
            for (int index = 0; index < chairs.Length; index++)
            {
                GameObject chairObject = Track(new GameObject("Chair " + index));
                WorldItem chairItem = chairObject.AddComponent<WorldItem>();
                EditModeTestLifecycle.Invoke(chairItem, "Awake");
                chairs[index] = chairObject.AddComponent<ThrowableChair>();
                EditModeTestLifecycle.Invoke(chairs[index], "Awake");
            }
            GameObject cardObject = Track(new GameObject("Library Card"));
            WorldItem card = cardObject.AddComponent<WorldItem>();
            EditModeTestLifecycle.Invoke(card, "Awake");
            cardObject.SetActive(false);
            FlowerpotBreakable flowerpot = Track(new GameObject("Flowerpot"))
                .AddComponent<FlowerpotBreakable>();
            SetField(flowerpot, "libraryCard", cardObject);
            SetField(puzzle, "chairs", chairs);
            SetField(puzzle, "flowerpot", flowerpot);
            SetField(puzzle, "libraryCard", card);
            puzzles.Add(puzzle);
            EditModeTestLifecycle.Invoke(puzzle, "OnEnable");

            return new PuzzleFixture(
                dialogue, broadcast, puzzle, body, chairs, flowerpot, card,
                interaction, inventory, playerCollider);
        }

        private static DialoguePage[] OnePage(string speaker, string text)
        {
            return new[] { new DialoguePage(speaker, text) };
        }

        private GameObject Track(GameObject item)
        {
            created.Add(item);
            return item;
        }

        private static void SetField(object target, string name, object value)
        {
            target.GetType().GetField(name,
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(target, value);
        }

        private static T GetField<T>(object target, string name)
        {
            return (T)target.GetType().GetField(name,
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(target);
        }

        private sealed class PuzzleFixture
        {
            public PuzzleFixture(
                DialogueController dialogue,
                BroadcastDialogueTrigger broadcast,
                ClassroomLibraryCardPuzzle puzzle,
                Text body,
                ThrowableChair[] chairs,
                FlowerpotBreakable flowerpot,
                WorldItem card,
                PlayerInteraction interaction,
                PlayerInventory inventory,
                Collider2D playerCollider)
            {
                Dialogue = dialogue;
                Broadcast = broadcast;
                Puzzle = puzzle;
                Body = body;
                Chairs = chairs;
                Flowerpot = flowerpot;
                Card = card;
                Interaction = interaction;
                Inventory = inventory;
                PlayerCollider = playerCollider;
            }

            public DialogueController Dialogue { get; }
            public BroadcastDialogueTrigger Broadcast { get; }
            public ClassroomLibraryCardPuzzle Puzzle { get; }
            public Text Body { get; }
            public ThrowableChair[] Chairs { get; }
            public FlowerpotBreakable Flowerpot { get; }
            public WorldItem Card { get; }
            public PlayerInteraction Interaction { get; }
            public PlayerInventory Inventory { get; }
            public Collider2D PlayerCollider { get; }
        }
    }
}
