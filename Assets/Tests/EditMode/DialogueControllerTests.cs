using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class DialogueControllerTests
    {
        private readonly List<GameObject> created = new List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject item in created)
            {
                Object.DestroyImmediate(item);
            }
            created.Clear();
        }

        [Test]
        public void StartAndSkip_LocksThenRestoresGameplayComponents()
        {
            DialogueController controller = CreateController(
                out MainCharacterMovement movement,
                out PlayerInteraction interaction,
                out PlayerInventory inventory);

            Assert.That(controller.StartDialogue(OnePage("教授", "测试")), Is.True);
            Assert.That(movement.enabled, Is.False);
            Assert.That(interaction.enabled, Is.False);
            Assert.That(inventory.enabled, Is.False);

            controller.SkipDialogue();

            Assert.That(controller.IsPlaying, Is.False);
            Assert.That(movement.enabled, Is.True);
            Assert.That(interaction.enabled, Is.True);
            Assert.That(inventory.enabled, Is.True);
        }

        [Test]
        public void Skip_RestoresAComponentThatWasAlreadyDisabledToDisabled()
        {
            DialogueController controller = CreateController(out _, out _, out PlayerInventory inventory);
            inventory.enabled = false;

            controller.StartDialogue(OnePage("教授", "测试"));
            controller.SkipDialogue();

            Assert.That(inventory.enabled, Is.False);
        }

        [Test]
        public void StartDialogue_DoesNotOverwriteActiveSequence()
        {
            DialogueController controller = CreateController(out _, out _, out _);

            Assert.That(controller.StartDialogue(OnePage("教授", "第一段")), Is.True);
            Assert.That(controller.StartDialogue(OnePage("主角", "第二段")), Is.False);
            Assert.That(controller.CurrentSpeaker, Is.EqualTo("教授"));
        }

        [Test]
        public void TryAdvanceDialogue_IsBlockedUntilVisibleTextCompletes()
        {
            DialogueController controller = CreateController(out _, out _, out _);
            controller.StartDialogue(OnePage("教授", "一二三"));

            Assert.That(controller.TryAdvanceDialogue(), Is.EqualTo(DialogueAdvanceResult.Blocked));
            Assert.That(controller.IsPlaying, Is.True);
        }

        [Test]
        public void NearestPrivateDialogue_IsChosen()
        {
            DialogueController controller = CreateController(out _, out _, out _);
            PrivateDialogueTrigger far = CreateSource(new Vector2(3f, 0f), "远处");
            PrivateDialogueTrigger near = CreateSource(new Vector2(1f, 0f), "近处");
            controller.RegisterPrivateDialogue(far);
            controller.RegisterPrivateDialogue(near);

            Assert.That(controller.TryStartNearestPrivateDialogue(), Is.True);
            Assert.That(controller.CurrentSpeaker, Is.EqualTo("近处"));
        }

        [Test]
        public void InvalidPage_IsRejectedWithoutLockingGameplay()
        {
            DialogueController controller = CreateController(
                out MainCharacterMovement movement, out _, out _);

            Assert.That(controller.StartDialogue(OnePage("教授", "")), Is.False);
            Assert.That(movement.enabled, Is.True);
            Assert.That(controller.IsPlaying, Is.False);
        }

        [Test]
        public void CompletionCallback_RunsOnceAfterSkipAndCanChainDialogue()
        {
            DialogueController controller = CreateController(out _, out _, out _);
            int completed = 0;

            Assert.That(controller.StartDialogue(
                OnePage("教授", "第一段"),
                () =>
                {
                    completed++;
                    Assert.That(controller.IsPlaying, Is.False);
                    Assert.That(controller.StartDialogue(
                        OnePage("主角", "第二段")), Is.True);
                }), Is.True);

            controller.SkipDialogue();

            Assert.That(completed, Is.EqualTo(1));
            Assert.That(controller.IsPlaying, Is.True);
            Assert.That(controller.CurrentSpeaker, Is.EqualTo("主角"));
        }

        [Test]
        public void CompletionCallback_RunsOnceAfterLastPageCompletes()
        {
            DialogueController controller = CreateController(out _, out _, out _);
            int completed = 0;
            Assert.That(controller.StartDialogue(
                OnePage("主角", "测试"), () => completed++), Is.True);

            controller.Tick(1f);
            Assert.That(controller.TryAdvanceDialogue(),
                Is.EqualTo(DialogueAdvanceResult.Completed));

            Assert.That(completed, Is.EqualTo(1));
            Assert.That(controller.IsPlaying, Is.False);
        }

        [Test]
        public void CompletionCallback_DoesNotRunWhenStartIsRejected()
        {
            DialogueController controller = CreateController(out _, out _, out _);
            int completed = 0;
            controller.StartDialogue(OnePage("教授", "进行中"));

            Assert.That(controller.StartDialogue(
                OnePage("主角", "被拒绝"), () => completed++), Is.False);

            Assert.That(completed, Is.Zero);
        }

        [Test]
        public void CompletionCallback_DoesNotRunWhenHudCannotShowDialogue()
        {
            DialogueController controller = CreateController(
                out MainCharacterMovement movement,
                out PlayerInteraction interaction,
                out PlayerInventory inventory);
            DialogueHUD hud = GetField<DialogueHUD>(controller, "hud");
            SetField(hud, "speakerText", null);
            int completed = 0;
            LogAssert.Expect(LogType.Error,
                "DialogueHUD requires a panel and three Text references.");

            Assert.That(controller.StartDialogue(
                OnePage("主角", "无法显示"), () => completed++), Is.False);

            Assert.That(completed, Is.Zero);
            Assert.That(controller.IsPlaying, Is.False);
            Assert.That(movement.enabled, Is.True);
            Assert.That(interaction.enabled, Is.True);
            Assert.That(inventory.enabled, Is.True);
        }

        private DialogueController CreateController(
            out MainCharacterMovement movement,
            out PlayerInteraction interaction,
            out PlayerInventory inventory)
        {
            GameObject player = Track(new GameObject("Player"));
            player.AddComponent<Rigidbody2D>();
            movement = player.AddComponent<MainCharacterMovement>();
            inventory = player.AddComponent<PlayerInventory>();
            interaction = player.AddComponent<PlayerInteraction>();
            DialogueController controller = player.AddComponent<DialogueController>();

            GameObject hudObject = Track(new GameObject("HUD"));
            DialogueHUD hud = hudObject.AddComponent<DialogueHUD>();
            GameObject panel = Track(new GameObject("Panel"));
            Text speaker = Track(new GameObject("Speaker")).AddComponent<Text>();
            Text body = Track(new GameObject("Body")).AddComponent<Text>();
            Text hint = Track(new GameObject("Hint")).AddComponent<Text>();
            SetField(hud, "panel", panel);
            SetField(hud, "speakerText", speaker);
            SetField(hud, "bodyText", body);
            SetField(hud, "hintText", hint);

            SetField(controller, "hud", hud);
            SetField(controller, "movement", movement);
            SetField(controller, "playerInteraction", interaction);
            SetField(controller, "playerInventory", inventory);
            return controller;
        }

        private PrivateDialogueTrigger CreateSource(Vector2 position, string speaker)
        {
            GameObject sourceObject = Track(new GameObject(speaker));
            sourceObject.transform.position = position;
            PrivateDialogueTrigger source = sourceObject.AddComponent<PrivateDialogueTrigger>();
            SetField(source, "pages", OnePage(speaker, "测试"));
            return source;
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
            target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(target, value);
        }

        private static T GetField<T>(object target, string name)
        {
            return (T)target.GetType()
                .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(target);
        }
    }
}
