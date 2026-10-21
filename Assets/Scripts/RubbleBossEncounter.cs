using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class RubbleBossEncounter : MonoBehaviour
    {
        [SerializeField] private GangDogBoss boss;
        [SerializeField] private RubbleSwitchPuzzle puzzle;
        [SerializeField] private Vector2 introDestination;
        [SerializeField] private Vector2 battlePlayerPosition;
        [SerializeField] private Vector2 bossEntrancePosition;

        private PlayerInteraction player;
        private Rigidbody2D playerBody;
        private bool encounterStarted;
        private bool battleActive;
        private int captures;
        private Text progressText;

        public bool IsBattleActive => battleActive;

        public void Configure(GangDogBoss leader, RubbleSwitchPuzzle switchPuzzle,
            Vector2 walkDestination, Vector2 playerStart, Vector2 bossStart)
        {
            boss = leader;
            puzzle = switchPuzzle;
            introDestination = walkDestination;
            battlePlayerPosition = playerStart;
            bossEntrancePosition = bossStart;
        }

        private void Update()
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerInteraction>();
                if (player != null) playerBody = player.GetComponent<Rigidbody2D>();
            }

            if (encounterStarted || playerBody == null) return;
            Vector2 position = playerBody.position;
            if (position.x < 294f || position.x > 306f || position.y > -4.2f) return;
            BeginIntroWalk();
        }

        private void FixedUpdate()
        {
            if (!encounterStarted || battleActive || playerBody == null) return;
            Vector2 next = Vector2.MoveTowards(playerBody.position, introDestination,
                2.8f * Time.fixedDeltaTime);
            playerBody.MovePosition(next);
            if (Vector2.Distance(next, introDestination) < .03f) BeginDialogue();
        }

        private void BeginIntroWalk()
        {
            encounterStarted = true;
            SetPlayerControls(false);
        }

        private void BeginDialogue()
        {
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;
            if (boss != null) boss.gameObject.SetActive(true);
            string bossName = GameLocalization.Get("rubble.speaker.strayDogsBoss");
            dialogue.StartDialogue(new[]
            {
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.1")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.2")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.3")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.4")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.5")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.6")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.7")),
                new DialoguePage(bossName, GameLocalization.Get("rubble.encounter.8")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.9")),
                new DialoguePage(bossName, GameLocalization.Get("rubble.encounter.10")),
                new DialoguePage("主角", GameLocalization.Get("rubble.encounter.11")),
                new DialoguePage(bossName, GameLocalization.Get("rubble.encounter.12")),
                new DialoguePage(bossName, GameLocalization.Get("rubble.encounter.13")),
                new DialoguePage(bossName, GameLocalization.Get("rubble.encounter.14"))
            }, BeginBattle);
        }

        private void BeginBattle()
        {
            battleActive = true;
            captures = 0;
            puzzle?.ResetForNewRound();
            SetPlayerControls(true);
            if (boss != null)
            {
                boss.gameObject.SetActive(true);
                boss.BeginBattle(this, bossEntrancePosition);
            }

            EnsureProgressText();
            RefreshProgress();
        }

        public void OnBossCapture()
        {
            if (!battleActive) return;
            captures++;
            if (captures >= 3)
            {
                battleActive = false;
                boss?.EndBattle();
                SetPlayerControls(false);
                ShowFailureEnding();
                return;
            }

            puzzle?.ResetForNewRound();
            if (playerBody != null)
            {
                playerBody.position = battlePlayerPosition;
                playerBody.velocity = Vector2.zero;
            }

            boss?.ResetForRound(bossEntrancePosition);
            RefreshProgress();
        }

        public void RefreshProgress()
        {
            if (progressText != null && battleActive)
            {
                progressText.text = GameLocalization.Format("rubble.ui.progress",
                    puzzle != null ? puzzle.ActivatedCount : 0);
            }
        }

        public void CompleteBattle()
        {
            if (!battleActive) return;
            if (progressText != null) progressText.text = GameLocalization.Get("rubble.ui.escape");
        }

        public bool PlayLoreDialogue(DialoguePage[] pages)
        {
            if (!battleActive || player == null) return false;
            DialogueController dialogue = player.GetComponent<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return false;
            boss?.SetTemporarilyPaused(true);
            bool started = dialogue.StartDialogue(pages,
                () => boss?.SetTemporarilyPaused(false));
            if (!started) boss?.SetTemporarilyPaused(false);
            return started;
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

        private void EnsureProgressText()
        {
            if (progressText != null) return;
            GameObject canvasObject = new GameObject("Rubble Boss Progress Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 35;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            GameObject textObject = new GameObject("Progress");
            textObject.transform.SetParent(canvasObject.transform, false);
            progressText = textObject.AddComponent<Text>();
            progressText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            progressText.fontSize = 25;
            progressText.color = Color.white;
            progressText.alignment = TextAnchor.UpperCenter;
            RectTransform rect = progressText.rectTransform;
            rect.anchorMin = new Vector2(.5f, 1f);
            rect.anchorMax = new Vector2(.5f, 1f);
            rect.pivot = new Vector2(.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -28f);
            rect.sizeDelta = new Vector2(720f, 52f);
        }

        private void ShowFailureEnding()
        {
            GameObject canvasObject = new GameObject("Broken Dreams Ending Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            Image panel = canvasObject.AddComponent<Image>();
            panel.color = new Color(0f, 0f, 0f, .9f);
            RectTransform panelRect = panel.rectTransform;
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            GameObject textObject = new GameObject("Ending Title");
            textObject.transform.SetParent(canvasObject.transform, false);
            Text title = textObject.AddComponent<Text>();
            title.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            title.fontSize = 46;
            title.color = Color.white;
            title.alignment = TextAnchor.MiddleCenter;
            title.text = GameLocalization.Get("rubble.ending.brokenDreams.title");
            RectTransform rect = title.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.anchoredPosition = new Vector2(0f, 235f);
            rect.sizeDelta = new Vector2(680f, 120f);

            GameObject descriptionObject = new GameObject("Ending Description");
            descriptionObject.transform.SetParent(canvasObject.transform, false);
            Text description = descriptionObject.AddComponent<Text>();
            description.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            description.fontSize = 25;
            description.color = Color.white;
            description.alignment = TextAnchor.MiddleCenter;
            description.text = string.Join("\n\n", new[]
            {
                GameLocalization.Get("rubble.ending.brokenDreams.1"),
                GameLocalization.Get("rubble.ending.brokenDreams.2"),
                GameLocalization.Get("rubble.ending.brokenDreams.3"),
                GameLocalization.Get("rubble.ending.brokenDreams.4"),
                GameLocalization.Get("rubble.ending.brokenDreams.5")
            });
            RectTransform descriptionRect = description.rectTransform;
            descriptionRect.anchorMin = new Vector2(.5f, .5f);
            descriptionRect.anchorMax = new Vector2(.5f, .5f);
            descriptionRect.anchoredPosition = new Vector2(0f, -25f);
            descriptionRect.sizeDelta = new Vector2(1100f, 470f);
        }
    }
}
