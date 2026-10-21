using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class ThirteenEncounter : MonoBehaviour
    {
        private const string DriedMeatId = "apartment.driedMeat";

        private static readonly string[] DiscoveryKeys =
        {
            "thirteen.encounter.discovery.1", "thirteen.encounter.discovery.2",
            "thirteen.encounter.discovery.3"
        };

        private static readonly string[] GiftKeys =
        {
            "thirteen.encounter.gift.1", "thirteen.encounter.gift.2",
            "thirteen.encounter.gift.3", "thirteen.encounter.gift.4",
            "thirteen.encounter.gift.5", "thirteen.encounter.gift.6",
            "thirteen.encounter.gift.7", "thirteen.encounter.gift.8"
        };

        [SerializeField] private Transform thirteenVisual;
        [SerializeField] private GameObject giftCrystal;
        [SerializeField] private Vector2 offeringPosition;
        [SerializeField] private Vector2 thirteenMeetingPosition;

        private bool discoveryStarted;
        private bool awaitingOffer;
        private bool completed;
        private Text hintText;

        public void Configure(Transform visual, GameObject crystal, Vector2 offerPosition,
            Vector2 meetingPosition)
        {
            thirteenVisual = visual;
            giftCrystal = crystal;
            offeringPosition = offerPosition;
            thirteenMeetingPosition = meetingPosition;
            if (giftCrystal != null) giftCrystal.SetActive(false);
        }

        private void Update()
        {
            if (discoveryStarted || completed) return;
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            DialogueController dialogue = player != null ? player.GetComponent<DialogueController>() : null;
            if (player == null || dialogue == null || dialogue.IsPlaying || thirteenVisual == null) return;
            if (Vector2.Distance(player.transform.position, thirteenVisual.position) > 2.3f) return;
            discoveryStarted = dialogue.StartDialogue(CreateDiscoveryPages(), BeginOffering);
        }

        public static bool TryOfferSelectedDriedMeat(PlayerInventory inventory)
        {
            ThirteenEncounter encounter = FindObjectOfType<ThirteenEncounter>();
            return encounter != null && encounter.TryOffer(inventory);
        }

        private bool TryOffer(PlayerInventory inventory)
        {
            if (!awaitingOffer || inventory == null || thirteenVisual == null
                || Vector2.Distance(inventory.transform.position, thirteenVisual.position) > 3f)
            {
                return false;
            }

            if (!inventory.TryRemoveSelectedById(DriedMeatId, out WorldItem meat)) return false;
            meat.Drop(offeringPosition);
            meat.SetPickupEnabled(false);
            awaitingOffer = false;
            if (hintText != null) hintText.gameObject.SetActive(false);
            if (thirteenVisual != null) thirteenVisual.position = thirteenMeetingPosition;
            if (giftCrystal != null) giftCrystal.SetActive(true);
            DialogueController dialogue = inventory.GetComponent<DialogueController>();
            if (dialogue != null)
            {
                dialogue.StartDialogue(CreateGiftPages(), BeginReturnHome);
            }

            return true;
        }

        private void BeginOffering()
        {
            awaitingOffer = true;
            EnsureHint();
            hintText.gameObject.SetActive(true);
        }

        private void BeginReturnHome()
        {
            completed = true;
            ThirteenCompanionState.IsTravelingWithPlayer = true;
            thirteenVisual?.GetComponent<ThirteenFollower>()?.BeginPersistentFollowing();
            StartCoroutine(ReturnHomeAfterBlackout());
        }

        private IEnumerator ReturnHomeAfterBlackout()
        {
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            if (player == null) yield break;
            MainCharacterMovement movement = player.GetComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (movement != null) movement.enabled = false;
            player.enabled = false;
            if (inventory != null) inventory.enabled = false;

            GameObject canvasObject = new GameObject("Return Home Blackout");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            Image panel = canvasObject.AddComponent<Image>();
            panel.color = Color.black;
            RectTransform rect = panel.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            yield return new WaitForSecondsRealtime(.45f);
            player.GetComponent<MapTransitionService>()?.RequestTransition("RoadHome", "RoadHomeFromMushroomCave");
            yield return new WaitForSecondsRealtime(.55f);
            Destroy(canvasObject);
            if (movement != null) movement.enabled = true;
            player.enabled = true;
            if (inventory != null) inventory.enabled = true;
        }

        private void EnsureHint()
        {
            if (hintText != null) return;
            GameObject canvasObject = new GameObject("Thirteen Offering Hint Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 35;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            GameObject textObject = new GameObject("Offering Hint");
            textObject.transform.SetParent(canvasObject.transform, false);
            hintText = textObject.AddComponent<Text>();
            hintText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            hintText.fontSize = 24;
            hintText.color = Color.white;
            hintText.alignment = TextAnchor.UpperCenter;
            hintText.text = GameLocalization.Get("thirteen.ui.offerDriedMeat");
            RectTransform rect = hintText.rectTransform;
            rect.anchorMin = new Vector2(.5f, 1f);
            rect.anchorMax = new Vector2(.5f, 1f);
            rect.pivot = new Vector2(.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -28f);
            rect.sizeDelta = new Vector2(760f, 48f);
        }

        private static DialoguePage[] CreateDiscoveryPages()
        {
            DialoguePage[] pages = new DialoguePage[DiscoveryKeys.Length];
            for (int index = 0; index < DiscoveryKeys.Length; index++)
            {
                pages[index] = new DialoguePage("主角", GameLocalization.Get(DiscoveryKeys[index]));
            }

            return pages;
        }

        private static DialoguePage[] CreateGiftPages()
        {
            DialoguePage[] pages = new DialoguePage[GiftKeys.Length];
            string unknown = GameLocalization.Get("thirteen.speaker.unknown");
            string thirteen = GameLocalization.Get("thirteen.speaker.thirteen");
            for (int index = 0; index < GiftKeys.Length; index++)
            {
                string speaker = index == 4 ? unknown : index == 6 ? thirteen : "主角";
                pages[index] = new DialoguePage(speaker, GameLocalization.Get(GiftKeys[index]));
            }

            return pages;
        }
    }
}
