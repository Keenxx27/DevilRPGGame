using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class DialogueTriggerTests
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
        public void BroadcastTrigger_RequestsSequenceOnlyOnce()
        {
            DialogueController controller = CreateController();
            BroadcastDialogueTrigger trigger = Track(new GameObject("Broadcast"))
                .AddComponent<BroadcastDialogueTrigger>();
            SetField(trigger, "controller", controller);
            SetField(trigger, "pages", OnePage("教授"));

            Assert.That(trigger.Trigger(), Is.True);
            controller.SkipDialogue();
            Assert.That(trigger.Trigger(), Is.False);
            Assert.That(trigger.HasTriggered, Is.True);
        }

        [Test]
        public void PrivateTrigger_RegistersPlayerOnEnterAndUnregistersOnExit()
        {
            DialogueController controller = CreateController();
            PrivateDialogueTrigger source = Track(new GameObject("NPC"))
                .AddComponent<PrivateDialogueTrigger>();
            SetField(source, "pages", OnePage("NPC"));
            Collider2D playerCollider = controller.GetComponent<Collider2D>();

            EditModeTestLifecycle.Invoke(source, "OnTriggerEnter2D", playerCollider);
            Assert.That(controller.TryStartNearestPrivateDialogue(), Is.True);
            controller.SkipDialogue();

            EditModeTestLifecycle.Invoke(source, "OnTriggerExit2D", playerCollider);
            Assert.That(controller.TryStartNearestPrivateDialogue(), Is.False);
        }

        [Test]
        public void PrivateTrigger_IgnoresColliderWithoutDialogueController()
        {
            DialogueController controller = CreateController();
            PrivateDialogueTrigger source = Track(new GameObject("NPC"))
                .AddComponent<PrivateDialogueTrigger>();
            SetField(source, "pages", OnePage("NPC"));
            Collider2D unrelated = Track(new GameObject("Unrelated")).AddComponent<BoxCollider2D>();

            EditModeTestLifecycle.Invoke(source, "OnTriggerEnter2D", unrelated);

            Assert.That(controller.TryStartNearestPrivateDialogue(), Is.False);
        }

        [Test]
        public void BroadcastTrigger_RaisesCompletedAfterSkippedDialogue()
        {
            DialogueController controller = CreateController();
            BroadcastDialogueTrigger trigger = Track(new GameObject("Broadcast"))
                .AddComponent<BroadcastDialogueTrigger>();
            SetField(trigger, "controller", controller);
            SetField(trigger, "pages", OnePage("教授"));
            int completed = 0;
            trigger.Completed += () => completed++;

            Assert.That(trigger.Trigger(), Is.True);
            Assert.That(completed, Is.Zero);
            controller.SkipDialogue();

            Assert.That(completed, Is.EqualTo(1));
        }

        [Test]
        public void BroadcastTrigger_CanRetryWhenDialogueWasBusy()
        {
            DialogueController controller = CreateController();
            Assert.That(controller.StartDialogue(OnePage("占用者")), Is.True);
            BroadcastDialogueTrigger trigger = Track(new GameObject("Broadcast"))
                .AddComponent<BroadcastDialogueTrigger>();
            SetField(trigger, "controller", controller);
            SetField(trigger, "pages", OnePage("教授"));
            int completed = 0;
            trigger.Completed += () => completed++;

            Assert.That(trigger.Trigger(), Is.False);
            Assert.That(trigger.HasTriggered, Is.False);

            controller.SkipDialogue();
            Assert.That(trigger.Trigger(), Is.True);
            Assert.That(trigger.HasTriggered, Is.True);
            controller.SkipDialogue();

            Assert.That(completed, Is.EqualTo(1));
        }

        private DialogueController CreateController()
        {
            GameObject player = Track(new GameObject("Player"));
            player.AddComponent<Rigidbody2D>();
            player.AddComponent<CircleCollider2D>();
            MainCharacterMovement movement = player.AddComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.AddComponent<PlayerInventory>();
            PlayerInteraction interaction = player.AddComponent<PlayerInteraction>();
            DialogueController controller = player.AddComponent<DialogueController>();

            DialogueHUD hud = Track(new GameObject("HUD")).AddComponent<DialogueHUD>();
            GameObject panel = Track(new GameObject("Panel"));
            SetField(hud, "panel", panel);
            SetField(hud, "speakerText", Track(new GameObject("Speaker")).AddComponent<Text>());
            SetField(hud, "bodyText", Track(new GameObject("Body")).AddComponent<Text>());
            SetField(hud, "hintText", Track(new GameObject("Hint")).AddComponent<Text>());
            SetField(controller, "hud", hud);
            SetField(controller, "movement", movement);
            SetField(controller, "playerInteraction", interaction);
            SetField(controller, "playerInventory", inventory);
            return controller;
        }

        private static DialoguePage[] OnePage(string speaker)
        {
            return new[] { new DialoguePage(speaker, "测试") };
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
    }
}
