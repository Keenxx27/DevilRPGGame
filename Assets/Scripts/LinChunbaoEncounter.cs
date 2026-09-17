using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG
{
    public sealed class LinChunbaoEncounter : MonoBehaviour
    {
        private enum ChoiceStage
        {
            None,
            Appearance,
            DemonBelief,
            Reconsideration
        }

        private static readonly DialoguePage[] KitchenPages =
        {
            new DialoguePage("主角", "高手？"),
            new DialoguePage("林春宝", "那是。搏击社的训练包括手腕稳定性和空间感，\n懂不懂？这算应用练习。")
        };

        private static readonly DialoguePage[][] ResponsePages =
        {
            new[] { new DialoguePage("林春宝", "挺好。") },
            new[] { new DialoguePage("林春宝", "那行，就在洗手间药柜里。") },
            new[] { new DialoguePage("林春宝", "你什么时候变成这样了？") }
        };

        private static readonly string[] Answers =
        {
            "1. 天热，闷。",
            "2. 确实忘记了，等会能帮我拿一个吗？",
            "3. 这不关你的事。"
        };

        private static readonly DialoguePage[] DiningTablePages =
        {
            new DialoguePage("林春宝", "反正家里就我，看着看着也习惯了。\n说起来——"),
            new DialoguePage("主角", "嗯？"),
            new DialoguePage("林春宝", "下午路过郊区，听说又有个小归墟开了，\n指甲盖那么大。猎魔机构的人去了，\n封了三条街，吵死了。"),
            new DialoguePage("主角", "然后呢？"),
            new DialoguePage("林春宝", "还能然后？老一套呗。\n清场，开枪，广播叫大家别慌。"),
            new DialoguePage("主角", "……"),
            new DialoguePage("林春宝", "我跟你说，社长我今天教了新招，\n侧身闪接低扫腿，配合呼吸节奏，\n发力点特别——"),
            new DialoguePage("主角", "……")
        };

        private static readonly string[] AppearanceAnswers =
        {
            "1. 是的",
            "2. 没有"
        };

        private static readonly DialoguePage[][] AppearanceResponsePages =
        {
            new[] { new DialoguePage("林春宝", "我就说嘛，有什么心事说给我听听！") },
            new[] { new DialoguePage("林春宝", "有。每次你露出这种‘全世界都错了就我对’的表情，\n就是你脸上这玩意最红的时候。\n现在它就在发亮。") }
        };

        private static readonly DialoguePage[] DemonBeliefPages =
        {
            new DialoguePage("主角", "……我只是觉得，他们从来不去想，\n有没有别的可能性。"),
            new DialoguePage("林春宝", "恶魔哎，大哥。"),
            new DialoguePage("林春宝", "它们从归墟地洞里爬出来，见人就扑，\n资料片里都播了无数遍了。能有什么可能性？\n难道请它们喝茶？"),
            new DialoguePage("主角", "资料片也是人拍的。也许……\n它们只是不知道该怎么和我们打招呼。"),
            new DialoguePage("林春宝", "（重重靠回椅背大笑起来）"),
            new DialoguePage("林春宝", "完了，我哥读书读傻了。"),
            new DialoguePage("林春宝", "……"),
            new DialoguePage("林春宝", "你要不要重新考虑考虑？")
        };

        private static readonly string[] ReconsiderationAnswers =
        {
            "1. 我是认真的",
            "2. 你说的对"
        };

        private static readonly DialoguePage[][] ReconsiderationResponsePages =
        {
            new[]
            {
                new DialoguePage("林春宝", "听着，不管你怎么想，外面的人可不会陪你发呆。\n真有哪天哪个不长眼的恶魔找你麻烦——"),
                new DialoguePage("林春宝", "有岔子，交给我。"),
                new DialoguePage("主角", "这样看来，我也不该耽搁了，\n我现在就去郊区。"),
                new DialoguePage("林春宝", "行啊。不过我建议你从食品柜里拿点东西带上，\n万一有必要吸引它们呢。")
            },
            new[]
            {
                new DialoguePage("林春宝", "好吧，看来我哥脑子本质上还是正常的。"),
                new DialoguePage("林春宝", "也许不过是被那帮坏同学欺负后的气话罢了，\n对吗？"),
                new DialoguePage("主角", "……"),
                new DialoguePage("林春宝", "得了，好好吃晚餐吧。")
            }
        };

        private bool hasStarted;
        private bool isChoosing;
        private int selectedAnswer;
        private GameObject choicePanel;
        private GameObject choiceCanvas;
        private Text[] answerTexts;
        private ChoiceStage choiceStage;
        private string[] activeAnswers;
        private bool isLeavingKitchen;
        [SerializeField] private Transform linChunbao;
        [SerializeField] private Transform diningTable;

        private void Update()
        {
            if (!hasStarted)
            {
                TryStartEncounter();
                return;
            }

            if (isChoosing)
            {
                HandleChoiceInput();
            }
        }

        private void TryStartEncounter()
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || dialogue.IsPlaying) return;

            Vector2 position = dialogue.transform.position;
            if (position.x < 154.5f || position.x > 165.5f || position.y < -4f || position.y > 4f)
            {
                return;
            }

            hasStarted = dialogue.StartDialogue(KitchenPages, BeginChoice);
        }

        private void BeginChoice()
        {
            BeginLockedChoice(ChoiceStage.Appearance, "今天没贴那个丑兮兮的胶布？", Answers);
        }

        private void HandleChoiceInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SetSelectedAnswer(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SetSelectedAnswer(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SetSelectedAnswer(2);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ConfirmAnswer();
            }
        }

        private void SetSelectedAnswer(int index)
        {
            selectedAnswer = index;
            RefreshAnswerColors();
        }

        private void ConfirmAnswer()
        {
            isChoosing = false;
            if (choiceCanvas != null) choiceCanvas.SetActive(false);

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null) return;
            dialogue.EndLockedChoiceDialogue();

            if (choiceStage == ChoiceStage.Appearance)
            {
                dialogue.StartDialogue(ResponsePages[selectedAnswer], MoveToDiningTable);
            }
            else if (choiceStage == ChoiceStage.DemonBelief)
            {
                dialogue.StartDialogue(AppearanceResponsePages[selectedAnswer], StartDemonBeliefDialogue);
            }
            else if (choiceStage == ChoiceStage.Reconsideration)
            {
                DialoguePage[] response = ReconsiderationResponsePages[selectedAnswer];
                dialogue.StartDialogue(response, selectedAnswer == 0 ? (System.Action)ResumeAfterKitchenDialogue : ShowIndifferentEnding);
            }

            choiceStage = ChoiceStage.None;
        }

        private void MoveToDiningTable()
        {
            if (linChunbao == null || diningTable == null)
            {
                FindObjectOfType<DialogueController>()?.StartDialogue(DiningTablePages);
                return;
            }

            StartCoroutine(MoveThenTalk());
        }

        private IEnumerator MoveThenTalk()
        {
            Vector2 destination = (Vector2)diningTable.position + Vector2.left * 1.2f;
            while (Vector2.Distance(linChunbao.position, destination) > .02f)
            {
                linChunbao.position = Vector2.MoveTowards(linChunbao.position, destination,
                    2.2f * Time.deltaTime);
                yield return null;
            }

            DialogueController dialogue = FindObjectOfType<DialogueController>();
            dialogue?.StartDialogue(DiningTablePages, BeginDemonBeliefChoice);
        }

        private void BeginDemonBeliefChoice()
        {
            BeginLockedChoice(ChoiceStage.DemonBelief, "哥，你又在想奇怪的事。", AppearanceAnswers);
        }

        private void StartDemonBeliefDialogue()
        {
            FindObjectOfType<DialogueController>()?.StartDialogue(DemonBeliefPages, BeginReconsiderationChoice);
        }

        private void BeginReconsiderationChoice()
        {
            BeginLockedChoice(ChoiceStage.Reconsideration, "你要不要重新考虑考虑？", ReconsiderationAnswers);
        }

        private void ResumeAfterKitchenDialogue()
        {
            // 此分支保留厨房自由行动，后续剧情和解谜可继续触发。
        }

        public void LeaveKitchenForLivingRoom()
        {
            if (isLeavingKitchen || linChunbao == null) return;
            isLeavingKitchen = true;
            StartCoroutine(MoveLinToLivingRoom());
        }

        private IEnumerator MoveLinToLivingRoom()
        {
            DoorController kitchenExit = GameObject.Find("Kitchen Exit Door")
                ?.GetComponent<DoorController>();
            if (kitchenExit != null)
            {
                Vector2 exitPosition = kitchenExit.transform.position;
                while (Vector2.Distance(linChunbao.position, exitPosition) > .04f)
                {
                    linChunbao.position = Vector2.MoveTowards(linChunbao.position, exitPosition,
                        2.2f * Time.deltaTime);
                    yield return null;
                }

                if (!kitchenExit.IsFullyOpen) kitchenExit.ToggleDoor();
                while (!kitchenExit.IsFullyOpen)
                {
                    yield return null;
                }
            }

            if (!SceneManager.GetSceneByName("ApartmentLivingRoom").isLoaded)
            {
                AsyncOperation load = SceneManager.LoadSceneAsync("ApartmentLivingRoom", LoadSceneMode.Additive);
                while (load != null && !load.isDone)
                {
                    yield return null;
                }
            }

            GameObject livingRoom = GameObject.Find("Apartment Living Room");
            if (livingRoom != null)
            {
                linChunbao.SetParent(livingRoom.transform, true);
                linChunbao.position = new Vector3(141.6f, .35f, 0f);
                linChunbao.GetComponent<LinChunbaoLivingRoomDialogue>()?.ActivateAtSofa();
            }

            if (kitchenExit != null)
            {
                yield return new WaitForSeconds(.25f);
                if (kitchenExit.IsFullyOpen) kitchenExit.ToggleDoor();
            }
        }

        private void ShowIndifferentEnding()
        {
            GameObject endingCanvas = new GameObject("Indifferent Ending Canvas");
            Canvas canvas = endingCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 40;
            UiScaleSettings.Configure(endingCanvas.AddComponent<CanvasScaler>());

            GameObject panel = new GameObject("Ending Panel");
            panel.transform.SetParent(endingCanvas.transform, false);
            Image image = panel.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, .88f);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            CreateText("Ending Title", panel.transform, "结局：关我何事", 46,
                Vector2.zero, TextAnchor.MiddleCenter, Color.white);
        }

        private void BeginLockedChoice(ChoiceStage stage, string question, string[] answers)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || !dialogue.BeginLockedChoiceDialogue("林春宝", question)) return;

            choiceStage = stage;
            activeAnswers = answers;
            isChoosing = true;
            selectedAnswer = 0;
            ShowChoicePanel();
        }

        private void ShowChoicePanel()
        {
            if (choicePanel == null)
            {
                CreateChoicePanel();
            }

            choicePanel.SetActive(true);
            choiceCanvas.SetActive(true);
            RefreshChoiceTexts();
            RefreshAnswerColors();
        }

        private void CreateChoicePanel()
        {
            choiceCanvas = new GameObject("Lin Chunbao Choice Canvas");
            choiceCanvas.transform.SetParent(transform, false);
            Canvas canvas = choiceCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 30;
            UiScaleSettings.Configure(choiceCanvas.AddComponent<CanvasScaler>());
            choiceCanvas.AddComponent<GraphicRaycaster>();

            CreateText("Choice Tutorial", choiceCanvas.transform,
                "通过按下数字键选择答复，按下E键确认答复", 22,
                new Vector2(0f, 210f), TextAnchor.MiddleCenter, Color.white);

            choicePanel = new GameObject("Choice Panel");
            choicePanel.transform.SetParent(choiceCanvas.transform, false);
            RectTransform panelRect = choicePanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(760f, 180f);
            panelRect.anchoredPosition = new Vector2(0f, 5f);

            answerTexts = new Text[Answers.Length];
            for (int index = 0; index < answerTexts.Length; index++)
            {
                answerTexts[index] = CreateText("Answer " + (index + 1), choicePanel.transform,
                    string.Empty, 28, new Vector2(0f, 62f - index * 58f),
                    TextAnchor.MiddleCenter, Color.white);
            }
        }

        private void RefreshChoiceTexts()
        {
            if (answerTexts == null || activeAnswers == null) return;
            for (int index = 0; index < answerTexts.Length; index++)
            {
                bool isVisible = index < activeAnswers.Length;
                answerTexts[index].gameObject.SetActive(isVisible);
                if (isVisible)
                {
                    answerTexts[index].text = activeAnswers[index];
                }
            }
        }

        private static Text CreateText(string name, Transform parent, string content, int fontSize,
            Vector2 position, TextAnchor alignment, Color color)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.text = content;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(720f, 44f);
            rect.anchoredPosition = position;
            return text;
        }

        private void RefreshAnswerColors()
        {
            if (answerTexts == null) return;
            for (int index = 0; index < answerTexts.Length; index++)
            {
                if (!answerTexts[index].gameObject.activeSelf) continue;
                answerTexts[index].color = index == selectedAnswer
                    ? new Color(1f, .85f, .1f, 1f)
                    : Color.white;
            }
        }
    }
}
