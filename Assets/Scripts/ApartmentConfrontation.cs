using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class ApartmentConfrontation : MonoBehaviour
    {
        private static readonly DialoguePage[] InitialPages =
        {
            new DialoguePage("林春宝", "……凭什么？就凭一张没盖章的破纸？\n我哥脸上那点东西怎么了？碍着谁了？！"),
            new DialoguePage("行政人员", "林春宝同学，请冷静。这是学校的决定，也是为了全体师生的安全考虑。\n您兄长面部的异常情况，根据《非人知性体接触者管理暂行条例》第七款，需要接受指定机构的隔离检查。\n在检查结果出来前，暂停其一切校内活动。"),
            new DialoguePage("林春宝", "不是房东就是哪个混蛋搞的鬼！")
        };

        private static readonly DialoguePage[] NoticePages =
        {
            new DialoguePage("行政人员", "屋主回来了。通知您已经看到。请于四十八小时内，携带这份通知，\n前往市立第三检疫中心报到。逾期未到，将由安全部门强制执行。"),
            new DialoguePage("主角", "……"),
            new DialoguePage("主角", "我知道了。")
        };

        private static readonly DialoguePage[] ThreatPages =
        {
            new DialoguePage("行政人员", "你知道吗……"),
            new DialoguePage("行政人员", "如果我们俩是清理部门的人，会考虑当场把你和你妹妹的人头\n抛到大街上。"),
            new DialoguePage("行政人员", "走。")
        };

        private static readonly DialoguePage[] FinalPages =
        {
            new DialoguePage("林春宝", "哥！你不能去！那种地方进去谁知道会怎样！\n我们走，现在就收拾东西……"),
            new DialoguePage("主角", "这停学通知措辞严谨，引用条例，看似合法合规。\n但它本质上是一纸驱逐令。"),
            new DialoguePage("主角", "看来，这里真的没有我的位置了。"),
            new DialoguePage("主角", "所以，我们得去别处看看。")
        };

        [SerializeField] private GameObject linChunbao;
        [SerializeField] private GameObject officialOne;
        [SerializeField] private GameObject officialTwo;
        [SerializeField] private GameObject thirteen;
        private PlayerInteraction player;
        private Rigidbody2D playerBody;
        private bool started;

        public void Configure(GameObject lin, GameObject firstOfficial, GameObject secondOfficial,
            GameObject companion)
        {
            linChunbao = lin;
            officialOne = firstOfficial;
            officialTwo = secondOfficial;
            thirteen = companion;
        }

        private void Update()
        {
            if (started || !ThirteenCompanionState.IsTravelingWithPlayer) return;
            if (player == null)
            {
                player = FindObjectOfType<PlayerInteraction>();
                if (player != null) playerBody = player.GetComponent<Rigidbody2D>();
            }

            if (playerBody == null || playerBody.position.x < 132f || playerBody.position.x > 148f) return;
            started = true;
            SetSceneActorsActive(true);
            SetPlayerControls(false);
            player.GetComponent<DialogueController>()?.StartDialogue(InitialPages, MovePlayerForward);
        }

        private void MovePlayerForward()
        {
            StartCoroutine(MovePlayerThenDialogue(new Vector2(140f, -1.25f), NoticePages, MoveToSofa));
        }

        private void MoveToSofa()
        {
            StartCoroutine(MovePlayerThenDialogue(new Vector2(141.65f, .6f), ThreatPages, LeaveOfficials));
        }

        private IEnumerator MovePlayerThenDialogue(Vector2 destination, DialoguePage[] pages,
            System.Action completion)
        {
            while (playerBody != null && Vector2.Distance(playerBody.position, destination) > .03f)
            {
                playerBody.position = Vector2.MoveTowards(playerBody.position, destination,
                    3.2f * Time.deltaTime);
                yield return null;
            }

            player?.GetComponent<DialogueController>()?.StartDialogue(pages, completion);
        }

        private void LeaveOfficials()
        {
            StartCoroutine(MoveOfficialsOut());
        }

        private IEnumerator MoveOfficialsOut()
        {
            DoorController door = GameObject.Find("Apartment Exit Door")?.GetComponent<DoorController>();
            if (door != null && !door.IsFullyOpen) door.ToggleDoor();
            Vector2 exit = new Vector2(140f, -4.65f);
            while (MoveActor(officialOne, exit) | MoveActor(officialTwo, exit + Vector2.right * .45f))
            {
                yield return null;
            }

            if (officialOne != null) officialOne.SetActive(false);
            if (officialTwo != null) officialTwo.SetActive(false);
            StartCoroutine(MoveLinThenFinish());
        }

        private IEnumerator MoveLinThenFinish()
        {
            if (linChunbao != null && playerBody != null)
            {
                Vector2 destination = playerBody.position + Vector2.up * .9f;
                while (Vector2.Distance(linChunbao.transform.position, destination) > .03f)
                {
                    linChunbao.transform.position = Vector2.MoveTowards(linChunbao.transform.position,
                        destination, 2.8f * Time.deltaTime);
                    yield return null;
                }
            }

            player?.GetComponent<DialogueController>()?.StartDialogue(FinalPages, ShowTrueEnding);
        }

        private void ShowTrueEnding()
        {
            GameObject canvasObject = new GameObject("No Belonging Ending Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            GameObject panelObject = new GameObject("Ending Panel");
            panelObject.transform.SetParent(canvasObject.transform, false);
            Image panel = panelObject.AddComponent<Image>();
            panel.color = new Color(0f, 0f, 0f, .9f);
            RectTransform panelRect = panel.rectTransform;
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            CreateEndingText("Ending Title", panelObject.transform, "结局：无归属者", 46,
                new Vector2(0f, 35f));
            CreateEndingText("Thanks", panelObject.transform, "感谢游玩", 26,
                new Vector2(0f, -42f));
        }

        private static void CreateEndingText(string name, Transform parent, string content,
            int fontSize, Vector2 position)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = content;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(700f, 100f);
        }

        private static bool MoveActor(GameObject actor, Vector2 destination)
        {
            if (actor == null || Vector2.Distance(actor.transform.position, destination) <= .03f) return false;
            actor.transform.position = Vector2.MoveTowards(actor.transform.position, destination,
                2.8f * Time.deltaTime);
            return true;
        }

        private void SetSceneActorsActive(bool active)
        {
            if (linChunbao != null) linChunbao.SetActive(active);
            if (officialOne != null) officialOne.SetActive(active);
            if (officialTwo != null) officialTwo.SetActive(active);
            if (thirteen != null) thirteen.SetActive(active);
        }

        private void SetPlayerControls(bool enabled)
        {
            if (player == null) return;
            MainCharacterMovement movement = player.GetComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (movement != null) movement.enabled = enabled;
            player.enabled = enabled;
            if (inventory != null) inventory.enabled = enabled;
        }
    }
}
