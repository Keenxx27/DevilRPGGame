using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class StorageRoomDarkness : MonoBehaviour
    {
        private static readonly DialoguePage[] OpeningPages =
        {
            new DialoguePage("主角", "里面真黑，完全看不见。"),
            new DialoguePage("主角", "也不知道电灯开关在哪里，\n看来只能摸索着找东西了。")
        };

        private PlayerInteraction player;
        private Canvas canvas;
        private readonly RectTransform[] panels = new RectTransform[4];
        private bool openingPlayed;

        private void Update()
        {
            if (player == null) player = FindObjectOfType<PlayerInteraction>();
            if (player == null) return;

            Vector2 position = player.transform.position;
            bool inside = position.x > 86f && position.x < 94f
                && position.y > -7f && position.y < 7f;
            if (!inside)
            {
                if (canvas != null) canvas.gameObject.SetActive(false);
                return;
            }

            EnsureCanvas();
            canvas.gameObject.SetActive(true);
            UpdateMask(position);
            if (!openingPlayed)
            {
                DialogueController dialogue = player.GetComponent<DialogueController>();
                if (dialogue != null && dialogue.StartDialogue(OpeningPages)) openingPlayed = true;
            }
        }

        private void EnsureCanvas()
        {
            if (canvas != null) return;
            GameObject canvasObject = new GameObject("Storage Room Darkness Canvas");
            canvasObject.transform.SetParent(transform, false);
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());
            for (int index = 0; index < panels.Length; index++)
            {
                GameObject panelObject = new GameObject("Darkness Panel " + index);
                panelObject.transform.SetParent(canvasObject.transform, false);
                Image image = panelObject.AddComponent<Image>();
                image.color = new Color(0f, 0f, 0f, 0.92f);
                panels[index] = image.rectTransform;
            }
        }

        private void UpdateMask(Vector2 position)
        {
            Camera camera = Camera.main;
            if (camera == null) return;
            Vector3 viewport = camera.WorldToViewportPoint(position);
            float left = Mathf.Clamp01(viewport.x - 0.14f);
            float right = Mathf.Clamp01(viewport.x + 0.14f);
            float bottom = Mathf.Clamp01(viewport.y - 0.16f);
            float top = Mathf.Clamp01(viewport.y + 0.16f);
            SetPanel(panels[0], new Vector2(0f, 0f), new Vector2(left, 1f));
            SetPanel(panels[1], new Vector2(right, 0f), new Vector2(1f, 1f));
            SetPanel(panels[2], new Vector2(left, 0f), new Vector2(right, bottom));
            SetPanel(panels[3], new Vector2(left, top), new Vector2(right, 1f));
        }

        private static void SetPanel(RectTransform panel, Vector2 min, Vector2 max)
        {
            panel.anchorMin = min;
            panel.anchorMax = max;
            panel.offsetMin = Vector2.zero;
            panel.offsetMax = Vector2.zero;
        }
    }
}
