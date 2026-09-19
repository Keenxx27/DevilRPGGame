using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class LinChunbaoPhone : MonoBehaviour, IInvestigationHandler
    {
        private static readonly DialoguePage[] StartPages =
        {
            new DialoguePage("主角", "那么，就由我来通关这个游戏吧……")
        };

        private static readonly DialoguePage[] FirstPasswordPages =
        {
            new DialoguePage("主角", "这屏幕……怎么回事？"),
            new DialoguePage("主角", "上面竟显示出一个数字1，难道是坏了？"),
            new DialoguePage("主角", "还是……和密码锁有关？")
        };

        private static readonly DialoguePage[] RepeatPasswordPages =
        {
            new DialoguePage("主角", "上面有一个数字1，可能同密码锁有关。")
        };

        private readonly List<Rect> obstacleRects = new List<Rect>();
        private Canvas canvas;
        private RectTransform keyRect;
        private RectTransform doorRect;
        private MainCharacterMovement movement;
        private PlayerInteraction interaction;
        private PlayerInventory inventory;
        private DialogueController dialogue;
        private bool movementWasEnabled;
        private bool interactionWasEnabled;
        private bool inventoryWasEnabled;
        private bool isPlaying;
        private bool isCompleted;
        private bool passwordRead;

        private const float LeftEdge = -520f;
        private const float RightEdge = 520f;
        private const float BottomEdge = -240f;
        private const float TopEdge = 240f;
        private const float KeySpeed = 190f;
        private const float VerticalSpeed = 310f;

        public static bool GameCompleted { get; private set; }

        public bool CanInvestigate(InvestigationKind kind)
        {
            DialogueController dialogue = FindObjectOfType<DialogueController>();
            return kind == InvestigationKind.LinBedroomPhone && dialogue != null
                && !dialogue.IsPlaying && !isPlaying;
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;

            DialogueController currentDialogue = FindObjectOfType<DialogueController>();
            if (!isCompleted)
            {
                return currentDialogue.StartDialogue(StartPages, StartMinigame);
            }

            DialoguePage[] pages = passwordRead ? RepeatPasswordPages : FirstPasswordPages;
            return currentDialogue.StartDialogue(pages, () => passwordRead = true);
        }

        private void Update()
        {
            if (!isPlaying || Time.timeScale <= 0f) return;

            Vector2 position = keyRect.anchoredPosition;
            position.x += KeySpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.W)) position.y += VerticalSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) position.y -= VerticalSpeed * Time.deltaTime;
            position.y = Mathf.Clamp(position.y, BottomEdge, TopEdge);
            keyRect.anchoredPosition = position;

            Rect keyBounds = GetRect(keyRect);
            foreach (Rect obstacle in obstacleRects)
            {
                if (keyBounds.Overlaps(obstacle))
                {
                    ResetKey();
                    return;
                }
            }

            if (keyBounds.Overlaps(GetRect(doorRect)))
            {
                FinishMinigame();
            }
        }

        private void StartMinigame()
        {
            PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
            if (player == null) return;

            movement = player.GetComponent<MainCharacterMovement>();
            interaction = player;
            inventory = player.GetComponent<PlayerInventory>();
            dialogue = player.GetComponent<DialogueController>();
            if (movement == null || inventory == null || dialogue == null) return;

            movementWasEnabled = movement.enabled;
            interactionWasEnabled = interaction.enabled;
            inventoryWasEnabled = inventory.enabled;
            movement.enabled = false;
            interaction.enabled = false;
            inventory.enabled = false;
            EnsureCanvas();
            canvas.gameObject.SetActive(true);
            ResetKey();
            isPlaying = true;
        }

        private void FinishMinigame()
        {
            isPlaying = false;
            isCompleted = true;
            GameCompleted = true;
            canvas.gameObject.SetActive(false);
            movement.enabled = movementWasEnabled;
            interaction.enabled = interactionWasEnabled;
            inventory.enabled = inventoryWasEnabled;
            ShowPasswordNumber();
        }

        private void ResetKey()
        {
            keyRect.anchoredPosition = new Vector2(LeftEdge, 0f);
        }

        private void EnsureCanvas()
        {
            if (canvas != null) return;

            GameObject canvasObject = new GameObject("Lin Chunbao Phone Minigame Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 45;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            RectTransform panel = CreateImage("Phone Game Panel", canvas.transform, Color.black,
                new Vector2(1280f, 760f));
            CreateText("Phone Game Title", panel, "林春宝的手机游戏", 30,
                new Vector2(0f, 322f));
            CreateText("Phone Game Hint", panel, "按 W / S 控制钥匙上下移动，避开障碍并抵达门前", 22,
                new Vector2(0f, -322f));

            RectTransform border = CreateImage("Game Area Border", panel,
                new Color(1f, 1f, 1f, .8f), new Vector2(1120f, 520f));
            border.GetComponent<Image>().type = Image.Type.Sliced;
            RectTransform playfield = CreateImage("Game Area", border, Color.black,
                new Vector2(1112f, 512f));

            keyRect = CreateImage("Moving Key", playfield, Color.white,
                new Vector2(40f, 22f));
            CreateKeyTeeth(keyRect);
            doorRect = CreateDoor(playfield);
            CreateObstacles(playfield);
            canvas.gameObject.SetActive(false);
        }

        private void CreateObstacles(Transform parent)
        {
            CreateObstacle(parent, -270f, -88f, 30f, 300f);
            CreateObstacle(parent, 0f, 88f, 30f, 300f);
            CreateObstacle(parent, 270f, -88f, 30f, 300f);
        }

        private void CreateObstacle(Transform parent, float x, float y, float width, float height)
        {
            RectTransform obstacle = CreateImage("White Obstacle", parent, Color.white,
                new Vector2(width, height));
            obstacle.anchoredPosition = new Vector2(x, y);
            obstacleRects.Add(GetRect(obstacle));
        }

        private RectTransform CreateDoor(Transform parent)
        {
            RectTransform door = CreateImage("Goal Door", parent, Color.white,
                new Vector2(42f, 130f));
            door.anchoredPosition = new Vector2(RightEdge, 0f);
            RectTransform opening = CreateImage("Door Opening", door, Color.black,
                new Vector2(28f, 108f));
            opening.anchoredPosition = Vector2.zero;
            return door;
        }

        private static void CreateKeyTeeth(RectTransform key)
        {
            RectTransform toothOne = CreateImage("Key Tooth One", key, Color.white,
                new Vector2(11f, 13f));
            toothOne.anchoredPosition = new Vector2(12f, -14f);
            RectTransform toothTwo = CreateImage("Key Tooth Two", key, Color.white,
                new Vector2(9f, 9f));
            toothTwo.anchoredPosition = new Vector2(1f, -12f);
        }

        private void ShowPasswordNumber()
        {
            Transform existing = transform.Find("Phone Password Number 1");
            if (existing != null)
            {
                existing.gameObject.SetActive(true);
                return;
            }

            GameObject number = new GameObject("Phone Password Number 1");
            number.transform.SetParent(transform, false);
            TextMesh text = number.AddComponent<TextMesh>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = "1";
            text.fontSize = 72;
            text.characterSize = .12f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.color = Color.white;
        }

        private static RectTransform CreateImage(string name, Transform parent, Color color,
            Vector2 size)
        {
            GameObject imageObject = new GameObject(name);
            imageObject.transform.SetParent(parent, false);
            Image image = imageObject.AddComponent<Image>();
            image.color = color;
            RectTransform rect = image.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = size;
            return rect;
        }

        private static Text CreateText(string name, Transform parent, string value, int fontSize,
            Vector2 position)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(1100f, 70f);
            rect.anchoredPosition = position;
            return text;
        }

        private static Rect GetRect(RectTransform rect)
        {
            return new Rect(rect.anchoredPosition - rect.sizeDelta * .5f, rect.sizeDelta);
        }
    }
}
