using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class SandbagBoxingMinigame : MonoBehaviour
    {
        private static readonly KeyCode[][] StageKeys =
        {
            new[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D },
            new[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.E },
            new[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.E, KeyCode.Q }
        };

        private static readonly KeyCode[] AllKeys =
        {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.E, KeyCode.Q
        };

        private static readonly float[] FallSpeeds = { 175f, 260f, 360f };
        private static readonly float[] SpawnIntervals = { 0f, .8f, .48f };

        private sealed class FallingKey
        {
            public KeyCode Key;
            public Text Text;
        }

        private readonly List<KeyCode> pendingKeys = new List<KeyCode>();
        private readonly List<FallingKey> fallingKeys = new List<FallingKey>();
        private Canvas canvas;
        private Text stageText;
        private Text[] targetTexts;
        private MainCharacterMovement movement;
        private PlayerInteraction interaction;
        private PlayerInventory inventory;
        private DialogueController dialogue;
        private SpriteRenderer sandbagRenderer;
        private bool movementWasEnabled;
        private bool interactionWasEnabled;
        private bool inventoryWasEnabled;
        private bool dialogueWasEnabled;
        private bool isPlaying;
        private bool isBroken;
        private int stageIndex;
        private float targetY;
        private float targetX;
        private float spawnTimer;
        private Vector3 originalScale;

        public bool IsBroken => isBroken;

        public bool StartBoxing(PlayerInteraction player)
        {
            if (isPlaying || isBroken || player == null) return false;
            movement = player.GetComponent<MainCharacterMovement>();
            interaction = player;
            inventory = player.GetComponent<PlayerInventory>();
            dialogue = player.GetComponent<DialogueController>();
            if (movement == null || inventory == null || dialogue == null || dialogue.IsPlaying) return false;

            movementWasEnabled = movement.enabled;
            interactionWasEnabled = interaction.enabled;
            inventoryWasEnabled = inventory.enabled;
            dialogueWasEnabled = dialogue.enabled;
            movement.enabled = false;
            interaction.enabled = false;
            inventory.enabled = false;
            dialogue.enabled = false;
            sandbagRenderer = GetComponent<SpriteRenderer>();
            originalScale = transform.localScale;
            EnsureCanvas();
            canvas.gameObject.SetActive(true);
            isPlaying = true;
            stageIndex = 0;
            BeginStage();
            return true;
        }

        private void Update()
        {
            if (!isPlaying || Time.timeScale <= 0f) return;
            if (stageIndex > 0 && pendingKeys.Count > 0)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0f)
                {
                    SpawnNextKey();
                    spawnTimer = SpawnIntervals[stageIndex];
                }
            }

            for (int index = fallingKeys.Count - 1; index >= 0; index--)
            {
                RectTransform rect = fallingKeys[index].Text.rectTransform;
                rect.anchoredPosition += Vector2.down * FallSpeeds[stageIndex] * Time.deltaTime;
                if (rect.anchoredPosition.y <= targetY)
                {
                    RestartStage();
                    return;
                }
            }

            foreach (KeyCode key in StageKeys[stageIndex])
            {
                if (!Input.GetKeyDown(key)) continue;
                FallingKey hit = FindHittableKey(key);
                if (hit != null)
                {
                    fallingKeys.Remove(hit);
                    Destroy(hit.Text.gameObject);
                    RegisterPunch();
                }
                else
                {
                    RestartStage();
                }
                return;
            }
        }

        private void BeginStage()
        {
            pendingKeys.Clear();
            ClearFallingKeys();
            pendingKeys.AddRange(StageKeys[stageIndex]);
            for (int index = pendingKeys.Count - 1; index > 0; index--)
            {
                int swap = Random.Range(0, index + 1);
                KeyCode value = pendingKeys[index];
                pendingKeys[index] = pendingKeys[swap];
                pendingKeys[swap] = value;
            }
            stageText.text = "拳击训练  第 " + (stageIndex + 1) + " / 3 阶段";
            RefreshTargetLabels();
            SpawnNextKey();
            spawnTimer = SpawnIntervals[stageIndex];
        }

        private void RestartStage()
        {
            BeginStage();
        }

        private void SpawnNextKey()
        {
            if (pendingKeys.Count == 0) return;
            KeyCode activeKey = pendingKeys[0];
            pendingKeys.RemoveAt(0);
            Text keyText = CreateText("Falling " + activeKey, canvas.transform, 48);
            keyText.fontStyle = FontStyle.Bold;
            keyText.text = activeKey.ToString();
            keyText.rectTransform.anchoredPosition = new Vector2(
                LaneX(System.Array.IndexOf(AllKeys, activeKey)), 560f);
            fallingKeys.Add(new FallingKey { Key = activeKey, Text = keyText });
        }

        private void RegisterPunch()
        {
            if (sandbagRenderer != null) sandbagRenderer.color = new Color(.75f, .25f, .12f, 1f);
            transform.localScale = originalScale * 1.08f;
            Invoke(nameof(ResetPunchVisual), .09f);
            if (stageIndex == 0 && pendingKeys.Count > 0)
            {
                SpawnNextKey();
                return;
            }

            if (pendingKeys.Count > 0 || fallingKeys.Count > 0) return;

            stageIndex++;
            if (stageIndex < StageKeys.Length)
            {
                BeginStage();
                return;
            }
            FinishBoxing();
        }

        private void ResetPunchVisual()
        {
            transform.localScale = originalScale;
            if (sandbagRenderer != null) sandbagRenderer.color = new Color(.42f, .12f, .1f, 1f);
        }

        private void FinishBoxing()
        {
            isPlaying = false;
            isBroken = true;
            canvas.gameObject.SetActive(false);
            RestorePlayerControls();
            if (sandbagRenderer != null) sandbagRenderer.enabled = false;
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null) collider.enabled = false;
            CreateBrokenSandbagAndNote();
        }

        private void RestorePlayerControls()
        {
            movement.enabled = movementWasEnabled;
            interaction.enabled = interactionWasEnabled;
            inventory.enabled = inventoryWasEnabled;
            dialogue.enabled = dialogueWasEnabled;
        }

        private void EnsureCanvas()
        {
            if (canvas != null) return;
            GameObject canvasObject = new GameObject("Sandbag Boxing Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 35;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            stageText = CreateText("Stage", canvas.transform, 25);
            stageText.rectTransform.anchoredPosition = new Vector2(0f, 470f);
            Camera camera = Camera.main;
            Vector3 viewport = camera != null ? camera.WorldToViewportPoint(transform.position)
                : new Vector3(.5f, .35f, 0f);
            targetX = (viewport.x - .5f) * 1920f;
            targetY = (viewport.y - .5f) * 1080f;
            targetTexts = new Text[AllKeys.Length];
            for (int index = 0; index < AllKeys.Length; index++)
            {
                targetTexts[index] = CreateText("Target " + AllKeys[index], canvas.transform, 27);
                targetTexts[index].text = AllKeys[index].ToString();
                targetTexts[index].rectTransform.anchoredPosition = new Vector2(LaneX(index), targetY);
            }
        }

        private void RefreshTargetLabels()
        {
            for (int index = 0; index < targetTexts.Length; index++)
            {
                bool available = System.Array.IndexOf(StageKeys[stageIndex], AllKeys[index]) >= 0;
                targetTexts[index].color = available
                    ? new Color(1f, 1f, 1f, .58f)
                    : new Color(1f, 1f, 1f, .2f);
            }
        }

        private float LaneX(int index)
        {
            return targetX + (index - (AllKeys.Length - 1) * .5f) * 72f;
        }

        private FallingKey FindHittableKey(KeyCode key)
        {
            FallingKey nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (FallingKey falling in fallingKeys)
            {
                if (falling.Key != key) continue;
                float distance = Mathf.Abs(falling.Text.rectTransform.anchoredPosition.y - targetY);
                if (distance <= 90f && distance < nearestDistance)
                {
                    nearest = falling;
                    nearestDistance = distance;
                }
            }
            return nearest;
        }

        private void ClearFallingKeys()
        {
            foreach (FallingKey falling in fallingKeys)
            {
                if (falling.Text != null) Destroy(falling.Text.gameObject);
            }
            fallingKeys.Clear();
        }

        private static Text CreateText(string name, Transform parent, int fontSize)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            Text text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform rect = text.rectTransform;
            rect.anchorMin = new Vector2(.5f, .5f);
            rect.anchorMax = new Vector2(.5f, .5f);
            rect.sizeDelta = new Vector2(240f, 80f);
            return text;
        }

        private void CreateBrokenSandbagAndNote()
        {
            GameObject broken = new GameObject("Broken Living Room Sandbag");
            broken.transform.SetParent(transform.parent, false);
            broken.transform.position = transform.position + Vector3.down * .45f;
            broken.transform.localScale = new Vector3(.85f, .5f, 1f);
            SpriteRenderer brokenRenderer = broken.AddComponent<SpriteRenderer>();
            brokenRenderer.sprite = sandbagRenderer != null ? sandbagRenderer.sprite : null;
            brokenRenderer.sharedMaterial = sandbagRenderer != null ? sandbagRenderer.sharedMaterial : null;
            brokenRenderer.color = new Color(.22f, .07f, .06f, 1f);
            brokenRenderer.sortingOrder = 3;

            GameObject note = new GameObject("Password Note 6");
            note.transform.SetParent(transform.parent, false);
            note.transform.position = transform.position + Vector3.down * 1.2f;
            note.transform.localScale = new Vector3(.65f, .42f, 1f);
            SpriteRenderer noteRenderer = note.AddComponent<SpriteRenderer>();
            noteRenderer.sprite = brokenRenderer.sprite;
            noteRenderer.sharedMaterial = brokenRenderer.sharedMaterial;
            noteRenderer.color = Color.white;
            noteRenderer.sortingOrder = 5;
            PasswordNote noteHandler = note.AddComponent<PasswordNote>();
            BoxCollider2D noteCollider = note.AddComponent<BoxCollider2D>();
            noteCollider.isTrigger = true;
            InvestigationPoint point = note.AddComponent<InvestigationPoint>();
            point.Configure(InvestigationKind.PasswordNote, noteHandler);

            GameObject number = new GameObject("Password Number 6");
            number.transform.SetParent(note.transform, false);
            TextMesh text = number.AddComponent<TextMesh>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = "6";
            text.fontSize = 72;
            text.characterSize = .12f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.color = Color.black;
        }
    }
}
