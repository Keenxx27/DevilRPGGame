using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class ThirteenEncounter : MonoBehaviour
    {
        private const string DriedMeatName = "肉干";

        private static readonly DialoguePage[] DiscoveryPages =
        {
            new DialoguePage("主角", "那里有什么东西在动。"),
            new DialoguePage("主角", "第一眼，真的像一只小黑猫。\n它蜷缩在最大的荧光蘑菇下，正好奇地打量着我。"),
            new DialoguePage("主角", "但它不是猫。耳朵的位置是两片薄膜般的结构，\n尾巴尖还燃着一小簇冰蓝色的火焰。")
        };

        private static readonly DialoguePage[] GiftPages =
        {
            new DialoguePage("主角", "它没有看肉干，反而慢慢靠近了我。"),
            new DialoguePage("主角", "它抬起前爪，小心翼翼地碰了碰我的手指。\n冰凉，还有一点粗糙。"),
            new DialoguePage("主角", "它又跑回蘑菇丛，叼来一块内部仿佛流淌着星光的晶石。\n这是礼物。你的呢？"),
            new DialoguePage("主角", "我……我没有恶意。"),
            new DialoguePage("？？？", "（它发出一个由三个高低不同音调组成的清脆音节。）"),
            new DialoguePage("主角", "十三……\n这个名字，怎么会忽然出现在我脑子里？"),
            new DialoguePage("十三", "（它歪了歪头，耳膜轻轻转动。）"),
            new DialoguePage("主角", "我伸出手，掌心向上。\n十三极轻地舔了一下我的指尖。"),
            new DialoguePage("主角", "那一刻，我十八年来所有的孤独和格格不入，\n都被这轻轻的一舔，短暂地治愈了。")
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
            discoveryStarted = dialogue.StartDialogue(DiscoveryPages, BeginOffering);
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

            if (!inventory.TryRemoveSelected(DriedMeatName, out WorldItem meat)) return false;
            meat.Drop(offeringPosition);
            meat.SetPickupEnabled(false);
            awaitingOffer = false;
            if (hintText != null) hintText.gameObject.SetActive(false);
            if (thirteenVisual != null) thirteenVisual.position = thirteenMeetingPosition;
            if (giftCrystal != null) giftCrystal.SetActive(true);
            DialogueController dialogue = inventory.GetComponent<DialogueController>();
            if (dialogue != null)
            {
                dialogue.StartDialogue(GiftPages, BeginReturnHome);
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
            hintText.text = "选中肉干后，在十三面前按 Q 放下肉干";
            RectTransform rect = hintText.rectTransform;
            rect.anchorMin = new Vector2(.5f, 1f);
            rect.anchorMax = new Vector2(.5f, 1f);
            rect.pivot = new Vector2(.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -28f);
            rect.sizeDelta = new Vector2(760f, 48f);
        }
    }
}
