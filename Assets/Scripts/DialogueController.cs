using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private DialogueHUD hud;
        [SerializeField] private MainCharacterMovement movement;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField, Min(0f)] private float charactersPerSecond = 12f;

        private readonly List<PrivateDialogueTrigger> nearbyPrivateDialogues =
            new List<PrivateDialogueTrigger>();
        private DialoguePlayback playback;
        private bool movementWasEnabled;
        private bool interactionWasEnabled;
        private bool inventoryWasEnabled;
        private System.Action onFinished;

        public bool IsPlaying => playback != null;
        public string CurrentSpeaker => playback != null ? playback.Speaker : string.Empty;

        private void Awake()
        {
            if (movement == null) movement = GetComponent<MainCharacterMovement>();
            if (playerInteraction == null) playerInteraction = GetComponent<PlayerInteraction>();
            if (playerInventory == null) playerInventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            if (IsPlaying)
            {
                Tick(Time.deltaTime);
                if (!IsPlaying)
                {
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SkipDialogue();
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    TryAdvanceDialogue();
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                TryStartNearestPrivateDialogue();
            }
        }

        public bool StartDialogue(DialoguePage[] pages)
        {
            return StartDialogue(pages, null);
        }

        public bool StartDialogue(DialoguePage[] pages, System.Action completion)
        {
            if (IsPlaying || !HasValidPages(pages))
            {
                return false;
            }

            if (hud == null || movement == null || playerInteraction == null
                || playerInventory == null)
            {
                Debug.LogError(
                    "DialogueController requires HUD and gameplay component references.", this);
                return false;
            }

            playback = new DialoguePlayback(pages);
            movementWasEnabled = movement.enabled;
            interactionWasEnabled = playerInteraction.enabled;
            inventoryWasEnabled = playerInventory.enabled;
            movement.enabled = false;
            playerInteraction.enabled = false;
            playerInventory.enabled = false;

            if (!hud.TryShow(playback.Speaker, playback.VisibleText, playback.IsPageComplete))
            {
                FinishDialogue();
                return false;
            }

            onFinished = completion;
            return true;
        }

        public void Tick(float deltaTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            playback.Step(charactersPerSecond, deltaTime);
            if (!hud.SetPage(playback.Speaker, playback.VisibleText, playback.IsPageComplete))
            {
                FinishDialogue();
            }
        }

        public DialogueAdvanceResult TryAdvanceDialogue()
        {
            if (!IsPlaying)
            {
                return DialogueAdvanceResult.Blocked;
            }

            DialogueAdvanceResult result = playback.TryAdvance();
            if (result == DialogueAdvanceResult.Completed)
            {
                FinishDialogue();
            }
            else if (result == DialogueAdvanceResult.Advanced
                && !hud.SetPage(playback.Speaker, playback.VisibleText, false))
            {
                FinishDialogue();
                return DialogueAdvanceResult.Completed;
            }

            return result;
        }

        public void SkipDialogue()
        {
            if (!IsPlaying)
            {
                return;
            }

            playback.Skip();
            FinishDialogue();
        }

        public void RegisterPrivateDialogue(PrivateDialogueTrigger source)
        {
            if (source != null && !nearbyPrivateDialogues.Contains(source))
            {
                nearbyPrivateDialogues.Add(source);
            }
        }

        public void UnregisterPrivateDialogue(PrivateDialogueTrigger source)
        {
            nearbyPrivateDialogues.Remove(source);
        }

        public void ClearNearbyPrivateDialogues()
        {
            nearbyPrivateDialogues.Clear();
        }

        public bool TryStartNearestPrivateDialogue()
        {
            if (IsPlaying)
            {
                return false;
            }

            PrivateDialogueTrigger nearest = null;
            float nearestDistance = float.PositiveInfinity;
            for (int index = nearbyPrivateDialogues.Count - 1; index >= 0; index--)
            {
                PrivateDialogueTrigger source = nearbyPrivateDialogues[index];
                if (source == null)
                {
                    nearbyPrivateDialogues.RemoveAt(index);
                    continue;
                }

                float distance = (source.transform.position - transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = source;
                    nearestDistance = distance;
                }
            }

            return nearest != null && StartDialogue(nearest.Pages);
        }

        private static bool HasValidPages(DialoguePage[] pages)
        {
            if (pages == null || pages.Length == 0)
            {
                return false;
            }

            foreach (DialoguePage page in pages)
            {
                if (page == null || string.IsNullOrWhiteSpace(page.Speaker)
                    || string.IsNullOrWhiteSpace(page.Text))
                {
                    return false;
                }
            }

            return true;
        }

        private void FinishDialogue()
        {
            System.Action completion = onFinished;
            onFinished = null;
            hud?.Hide();
            movement.enabled = movementWasEnabled;
            playerInteraction.enabled = interactionWasEnabled;
            playerInventory.enabled = inventoryWasEnabled;
            playback = null;
            completion?.Invoke();
        }
    }
}
