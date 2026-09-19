using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class RubbleBossEncounter : MonoBehaviour
    {
        private static readonly DialoguePage[] EncounterPages =
        {
            new DialoguePage("主角", "那里有什么东西在动。"),
            new DialoguePage("主角", "是一只……恶魔。大约小型犬大小，看上去受伤了。"),
            new DialoguePage("主角", "它看起来不像新闻里那些狰狞的怪物。\n更像一只走错地方的、古怪的蜥蜴。"),
            new DialoguePage("主角", "等等，那是……丧家犬的人？带着电棍？"),
            new DialoguePage("主角", "……"),
            new DialoguePage("主角", "它死了。"),
            new DialoguePage("主角", "（左脸的痕迹传来一阵细微的、冰凉的悸动，\n仿佛在共鸣某种遥远的痛苦。）"),
            new DialoguePage("丧家犬老大", "在这儿干什么呢你？没看到我正在进货吗？"),
            new DialoguePage("主角", "没……没什么。"),
            new DialoguePage("丧家犬老大", "别以为你脸上那片膏药能瞒过我，我儿子跟你大学一个专业的，\n早跟我说过你。你这半人半魔的怪物！"),
            new DialoguePage("主角", "你已经杀了那只恶魔，还不够吗？"),
            new DialoguePage("丧家犬老大", "还不够，凑儿子的学费还不够！"),
            new DialoguePage("丧家犬老大", "还不够让他读完下一年，好不再做我这恶心的营生！"),
            new DialoguePage("丧家犬老大", "但依我看，抓到了你这半魔……抵得上再抓五十只……\n不，一百只刚才那样的！")
        };

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
            dialogue.StartDialogue(EncounterPages, BeginBattle);
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
                progressText.text = "逃离丧家犬老大：已开启 "
                    + (puzzle != null ? puzzle.ActivatedCount : 0) + " / 4 个开关";
            }
        }

        public void CompleteBattle()
        {
            if (!battleActive) return;
            if (progressText != null) progressText.text = "尽快进入铁门离开";
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
            title.text = "结局：梦想折翼";
            RectTransform rect = title.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(680f, 120f);
        }
    }
}
