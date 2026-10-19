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

        private static DialoguePage PlayerPage(string key) =>
            new DialoguePage("主角", GameLocalization.Get(key));

        private static DialoguePage LinPage(string key) =>
            new DialoguePage(GameLocalization.Get("apartment.speaker.linChunbao"), GameLocalization.Get(key));

        private static string[] Texts(params string[] keys)
        {
            string[] values = new string[keys.Length];
            for (int index = 0; index < keys.Length; index++) values[index] = GameLocalization.Get(keys[index]);
            return values;
        }

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

            hasStarted = dialogue.StartDialogue(new[]
            {
                PlayerPage("apartment.kitchen.encounter.1"),
                LinPage("apartment.kitchen.encounter.2")
            }, BeginChoice);
        }

        private void BeginChoice()
        {
            BeginLockedChoice(ChoiceStage.Appearance,
                GameLocalization.Get("apartment.kitchen.appearance.question"),
                Texts("apartment.kitchen.appearance.answer.1", "apartment.kitchen.appearance.answer.2",
                    "apartment.kitchen.appearance.answer.3"));
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
                dialogue.StartDialogue(new[] { LinPage("apartment.kitchen.appearance.response." + (selectedAnswer + 1)) }, MoveToDiningTable);
            }
            else if (choiceStage == ChoiceStage.DemonBelief)
            {
                dialogue.StartDialogue(new[] { LinPage("apartment.kitchen.concern.response." + (selectedAnswer + 1)) }, StartDemonBeliefDialogue);
            }
            else if (choiceStage == ChoiceStage.Reconsideration)
            {
                DialoguePage[] response = selectedAnswer == 0
                    ? new[]
                    {
                        LinPage("apartment.kitchen.resolve.1"), LinPage("apartment.kitchen.resolve.2"),
                        PlayerPage("apartment.kitchen.resolve.3"), LinPage("apartment.kitchen.resolve.4")
                    }
                    : new[]
                    {
                        LinPage("apartment.kitchen.dismissive.1"), LinPage("apartment.kitchen.dismissive.2"),
                        PlayerPage("apartment.kitchen.dismissive.3"), LinPage("apartment.kitchen.dismissive.4")
                    };
                dialogue.StartDialogue(response, selectedAnswer == 0 ? (System.Action)ResumeAfterKitchenDialogue : ShowIndifferentEnding);
            }

            choiceStage = ChoiceStage.None;
        }

        private void MoveToDiningTable()
        {
            if (linChunbao == null || diningTable == null)
            {
                FindObjectOfType<DialogueController>()?.StartDialogue(CreateDiningTablePages());
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
            dialogue?.StartDialogue(CreateDiningTablePages(), BeginDemonBeliefChoice);
        }

        private void BeginDemonBeliefChoice()
        {
            BeginLockedChoice(ChoiceStage.DemonBelief,
                GameLocalization.Get("apartment.kitchen.concern.question"),
                Texts("apartment.kitchen.concern.answer.1", "apartment.kitchen.concern.answer.2"));
        }

        private void StartDemonBeliefDialogue()
        {
            FindObjectOfType<DialogueController>()?.StartDialogue(new[]
            {
                PlayerPage("apartment.kitchen.belief.1"), LinPage("apartment.kitchen.belief.2"),
                LinPage("apartment.kitchen.belief.3"), PlayerPage("apartment.kitchen.belief.4"),
                LinPage("apartment.kitchen.belief.5"), LinPage("apartment.kitchen.belief.6"),
                LinPage("apartment.kitchen.belief.7"), LinPage("apartment.kitchen.belief.8")
            }, BeginReconsiderationChoice);
        }

        private void BeginReconsiderationChoice()
        {
            BeginLockedChoice(ChoiceStage.Reconsideration,
                GameLocalization.Get("apartment.kitchen.reconsider.question"),
                Texts("apartment.kitchen.reconsider.answer.1", "apartment.kitchen.reconsider.answer.2"));
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

            CreateText("Ending Title", panel.transform,
                GameLocalization.Get("apartment.ending.indifferent.title"), 46,
                new Vector2(0f, 150f), TextAnchor.MiddleCenter, Color.white);
            Text description = CreateText("Ending Description", panel.transform,
                string.Join("\n\n", Texts("apartment.ending.indifferent.1", "apartment.ending.indifferent.2",
                    "apartment.ending.indifferent.3", "apartment.ending.indifferent.4")), 25,
                new Vector2(0f, -35f), TextAnchor.MiddleCenter, Color.white);
            description.rectTransform.sizeDelta = new Vector2(1050f, 330f);
        }

        private void BeginLockedChoice(ChoiceStage stage, string question, string[] answers)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            if (dialogue == null || !dialogue.BeginLockedChoiceDialogue(
                    GameLocalization.Get("apartment.speaker.linChunbao"), question)) return;

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
                GameLocalization.Get("apartment.ui.choiceTutorial"), 22,
                new Vector2(0f, 210f), TextAnchor.MiddleCenter, Color.white);

            choicePanel = new GameObject("Choice Panel");
            choicePanel.transform.SetParent(choiceCanvas.transform, false);
            RectTransform panelRect = choicePanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(760f, 180f);
            panelRect.anchoredPosition = new Vector2(0f, 5f);

            answerTexts = new Text[3];
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

        private static DialoguePage[] CreateDiningTablePages()
        {
            return new[]
            {
                LinPage("apartment.kitchen.table.1"), PlayerPage("apartment.kitchen.table.2"),
                LinPage("apartment.kitchen.table.3"), PlayerPage("apartment.kitchen.table.4"),
                LinPage("apartment.kitchen.table.5"), PlayerPage("apartment.kitchen.table.6"),
                LinPage("apartment.kitchen.table.7"), PlayerPage("apartment.kitchen.table.8")
            };
        }
    }
}
