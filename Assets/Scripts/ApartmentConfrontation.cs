using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class ApartmentConfrontation : MonoBehaviour
    {
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
            player.GetComponent<DialogueController>()?.StartDialogue(CreateInitialPages(), MovePlayerForward);
        }

        private void MovePlayerForward()
        {
            StartCoroutine(MovePlayerThenDialogue(new Vector2(140f, -1.25f), CreateNoticePages(), MoveToSofa));
        }

        private void MoveToSofa()
        {
            StartCoroutine(MovePlayerThenDialogue(new Vector2(141.65f, .6f), CreateThreatPages(), LeaveOfficials));
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

            player?.GetComponent<DialogueController>()?.StartDialogue(CreateFinalPages(), ShowTrueEnding);
        }

        private void ShowTrueEnding()
        {
            GameObject canvasObject = new GameObject("No Belonging Ending Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            GameObject endingPage = CreateEndingPage("Ending Page", canvasObject.transform);
            CreateEndingText("Ending Title", endingPage.transform,
                GameLocalization.Get("apartment.ending.noBelonging.title"), 46,
                new Vector2(0f, 235f));
            Text description = CreateEndingText("Ending Description", endingPage.transform,
                string.Join("\n\n", EndingTexts()), 25, new Vector2(0f, -25f));
            description.rectTransform.sizeDelta = new Vector2(1100f, 470f);

            GameObject thanksPage = CreateEndingPage("Thanks Page", canvasObject.transform);
            CreateEndingText("Thanks", thanksPage.transform,
                GameLocalization.Get("apartment.ending.thanks"), 32, Vector2.zero);
            thanksPage.SetActive(false);
            canvasObject.AddComponent<NoBelongingEndingPages>().Initialize(endingPage, thanksPage);
        }

        private static GameObject CreateEndingPage(string name, Transform parent)
        {
            GameObject page = new GameObject(name);
            page.transform.SetParent(parent, false);
            Image panel = page.AddComponent<Image>();
            panel.color = new Color(0f, 0f, 0f, .9f);
            RectTransform panelRect = panel.rectTransform;
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            return page;
        }

        private static Text CreateEndingText(string name, Transform parent, string content,
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
            return text;
        }

        private static string[] EndingTexts() => new[]
        {
            GameLocalization.Get("apartment.ending.noBelonging.1"),
            GameLocalization.Get("apartment.ending.noBelonging.2"),
            GameLocalization.Get("apartment.ending.noBelonging.3"),
            GameLocalization.Get("apartment.ending.noBelonging.4"),
            GameLocalization.Get("apartment.ending.noBelonging.5")
        };

        private static DialoguePage[] CreateInitialPages()
        {
            string lin = GameLocalization.Get("apartment.speaker.linChunbao");
            string official = GameLocalization.Get("apartment.speaker.municipalOfficer");
            return new[]
            {
                new DialoguePage(lin, GameLocalization.Get("apartment.confrontation.initial.linChallenge")),
                new DialoguePage(official, GameLocalization.Get("apartment.confrontation.initial.officialNotice")),
                new DialoguePage(lin, GameLocalization.Get("apartment.confrontation.initial.linAccusation"))
            };
        }

        private static DialoguePage[] CreateNoticePages()
        {
            string official = GameLocalization.Get("apartment.speaker.municipalOfficer");
            return new[]
            {
                new DialoguePage(official, GameLocalization.Get("apartment.confrontation.notice.report")),
                new DialoguePage("主角", GameLocalization.Get("apartment.confrontation.notice.silence")),
                new DialoguePage("主角", GameLocalization.Get("apartment.confrontation.notice.acknowledge"))
            };
        }

        private static DialoguePage[] CreateThreatPages()
        {
            string official = GameLocalization.Get("apartment.speaker.municipalOfficer");
            return new[]
            {
                new DialoguePage(official, GameLocalization.Get("apartment.confrontation.threat.lead")),
                new DialoguePage(official, GameLocalization.Get("apartment.confrontation.threat.cleanup")),
                new DialoguePage(official, GameLocalization.Get("apartment.confrontation.threat.leave"))
            };
        }

        private static DialoguePage[] CreateFinalPages()
        {
            string lin = GameLocalization.Get("apartment.speaker.linChunbao");
            return new[]
            {
                new DialoguePage(lin, GameLocalization.Get("apartment.confrontation.final.linPlea")),
                new DialoguePage("主角", GameLocalization.Get("apartment.confrontation.final.evictionOrder")),
                new DialoguePage("主角", GameLocalization.Get("apartment.confrontation.final.noPlace")),
                new DialoguePage("主角", GameLocalization.Get("apartment.confrontation.final.leaveTogether"))
            };
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
            ThirteenFollower persistentCompanion = ThirteenFollower.PersistentCompanion;
            if (persistentCompanion != null) thirteen = persistentCompanion.gameObject;
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

    public sealed class NoBelongingEndingPages : MonoBehaviour
    {
        private GameObject endingPage;
        private GameObject thanksPage;
        private float advanceAllowedAt;

        public void Initialize(GameObject ending, GameObject thanks)
        {
            endingPage = ending;
            thanksPage = thanks;
            advanceAllowedAt = Time.unscaledTime + .2f;
        }

        private void Update()
        {
            if (Time.unscaledTime < advanceAllowedAt || thanksPage == null || thanksPage.activeSelf) return;
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)) ShowThanksPage();
        }

        public void ShowThanksPage()
        {
            if (endingPage != null) endingPage.SetActive(false);
            if (thanksPage != null) thanksPage.SetActive(true);
        }
    }
}
