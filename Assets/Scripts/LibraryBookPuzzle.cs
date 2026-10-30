using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public sealed class LibraryBookPuzzle : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string FragmentItemId = "library.fragment";
        private const string GlueItemId = "library.glue";
        private const string LibraryCardItemId = "library.card";
        private const string BroomItemId = "storage.broom";
        private DialoguePage[] openingPages;
        private DialoguePage[] bookcasePages;
        private DialoguePage[] chairPages;
        private DialoguePage[] wallPages;
        private DialoguePage[] secondFragmentPages;
        private DialoguePage[] fragmentsPlacedPages;
        private DialoguePage[] counterPages;
        private DialoguePage[] broomPages;
        private DialoguePage[] gluePages;
        private DialoguePage[] completedPagePages;
        private DialoguePage[] revisitCompletedPagePages;

        private DialogueController dialogueController;
        private PlayerInventory inventory;
        private LibraryPushHint pushHint;
        private WorldItem firstFragment;
        private WorldItem secondFragment;
        private WorldItem thirdFragment;
        private WorldItem glueBottle;
        private WorldItem completePage;
        private Transform chair;
        private Transform fragmentTable;
        private readonly List<WorldItem> placedFragmentItems = new List<WorldItem>();
        private GameObject wallBrick;
        private int bookcaseInvestigations;
        private int chairPushes;
        private int placedFragments;
        private bool openingFinished;
        private bool chairMoving;
        private bool wallPrompted;
        private bool wallOpened;
        private bool placementFinished;
        private bool counterInvestigated;
        private bool gluePulling;
        private bool gluePulledOut;
        private bool pageCompleted;
        private bool endingStarted;

        public bool HasDiscoveredGlue => counterInvestigated;
        public bool HasReadCompletePage => pageCompleted;

        private void Awake()
        {
            openingPages = new[]
            {
                new DialoguePage("主角", GameLocalization.Get("library.opening.1")),
                new DialoguePage("主角", GameLocalization.Get("library.opening.2")),
                new DialoguePage("主角", GameLocalization.Get("library.opening.3")),
                new DialoguePage("主角", GameLocalization.Get("library.opening.4"))
            };
            bookcasePages = BuildPages("library.environment.bookcase.first",
                "library.environment.bookcase.1", "library.environment.bookcase.2",
                "library.environment.bookcase.4.1", "library.environment.bookcase.4.2");
            chairPages = BuildPages("library.environment.chair");
            wallPages = BuildPages("library.environment.wall");
            secondFragmentPages = BuildPages("library.environment.fragments.second");
            fragmentsPlacedPages = BuildPages("library.environment.table.fragmentsPlaced");
            counterPages = BuildPages("library.environment.counter.1",
                "library.environment.counter.2");
            broomPages = BuildPages("library.environment.counter.broom");
            gluePages = BuildPages("library.glue.1");
            completedPagePages = BuildPages("library.completed.1", "library.completed.2",
                "library.completed.3", "library.completed.4", "library.completed.5",
                "library.completed.6", "library.completed.7");
            revisitCompletedPagePages = BuildPages("library.completed.3", "library.completed.4",
                "library.revisit.3");
        }

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            inventory = FindObjectOfType<PlayerInventory>();
            pushHint = GetComponent<LibraryPushHint>();
            firstFragment = FindItem("Library Fragment 1");
            secondFragment = FindItem("Library Fragment 2");
            thirdFragment = FindItem("Library Fragment 3");
            glueBottle = FindItem("Library Glue Bottle");
            completePage = FindItem("Complete Library Page");
            chair = FindObject("Reading Chair Left South")?.transform;
            wallBrick = FindObject("Library Loose Wall Brick");

            if (dialogueController == null || inventory == null || firstFragment == null
                || secondFragment == null || thirdFragment == null || chair == null
                || wallBrick == null || glueBottle == null || completePage == null || pushHint == null)
            {
                Debug.LogError("LibraryBookPuzzle 缺少谜题场景对象或玩家组件。", this);
                enabled = false;
                return;
            }

            firstFragment.ConfigureLocalization(FragmentItemId, "library.item.fragment");
            secondFragment.ConfigureLocalization(FragmentItemId, "library.item.fragment");
            thirdFragment.ConfigureLocalization(FragmentItemId, "library.item.fragment");
            glueBottle.ConfigureLocalization(GlueItemId, "library.item.glue");
            completePage.ConfigureLocalization("library.completePage", "library.item.completePage");

            secondFragment.PickedUp += HandleSecondFragmentPickedUp;
            if (!dialogueController.StartDialogue(openingPages, () => openingFinished = true))
            {
                Debug.LogError("LibraryBookPuzzle 无法开始入馆独白。", this);
            }
        }

        private void OnDestroy()
        {
            if (secondFragment != null) secondFragment.PickedUp -= HandleSecondFragmentPickedUp;
        }

        public bool CanInvestigate(InvestigationKind kind)
        {
            if (!openingFinished || dialogueController == null || dialogueController.IsPlaying)
            {
                return false;
            }

            switch (kind)
            {
                case InvestigationKind.LibraryBookcase:
                    return bookcaseInvestigations < 1;
                case InvestigationKind.LibraryChair:
                    return chairPushes < 1 && !chairMoving;
                case InvestigationKind.LibraryWallBrick:
                    // Keep the brick target interactable after the first clue. Previously it
                    // disappeared unless the library card was already selected, making E look
                    // broken even while the player remained beside the brick.
                    return !wallOpened;
                case InvestigationKind.LibraryReadingTable:
                    return CanInvestigateReadingTable();
                case InvestigationKind.LibraryCounter:
                    return !gluePulledOut && (!counterInvestigated
                        || (IsSelectedItemId(BroomItemId) && !gluePulling));
                case InvestigationKind.LibraryCompletePage:
                    return pageCompleted && !endingStarted;
                default:
                    return false;
            }
        }

        public bool TryInvestigate(InvestigationKind kind)
        {
            if (!CanInvestigate(kind)) return false;

            switch (kind)
            {
                case InvestigationKind.LibraryBookcase: return TryInvestigateBookcase();
                case InvestigationKind.LibraryChair: return TryInvestigateChair();
                case InvestigationKind.LibraryWallBrick: return TryInvestigateWallBrick();
                case InvestigationKind.LibraryReadingTable: return TryInvestigateReadingTable();
                case InvestigationKind.LibraryCounter: return TryInvestigateCounter();
                case InvestigationKind.LibraryCompletePage:
                    return BeginPageEnding();
                default: return false;
            }
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            return kind == InvestigationKind.LibraryReadingTable
                && !placementFinished && IsSelectedItemId(FragmentItemId)
                && TryPlaceFragment();
        }

        private bool TryInvestigateBookcase()
        {
            return dialogueController.StartDialogue(bookcasePages, FinishBookcaseInvestigation);
        }

        private void FinishBookcaseInvestigation()
        {
            bookcaseInvestigations = 1;
            if (!inventory.TryStore(firstFragment))
            {
                firstFragment.gameObject.SetActive(true);
            }
        }

        private bool TryInvestigateChair()
        {
            return dialogueController.StartDialogue(chairPages,
                () => StartCoroutine(PushChair()));
        }

        private IEnumerator PushChair()
        {
            chairMoving = true;
            Vector3 start = chair.position;
            Vector3 end = start + Vector3.right * 0.25f;
            float elapsed = 0f;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                chair.position = Vector3.Lerp(start, end, elapsed / 0.2f);
                yield return null;
            }

            chair.position = end;
            chairPushes = 1;
            chairMoving = false;
            pushHint.Hide();
            secondFragment.gameObject.SetActive(true);
        }

        private bool TryInvestigateWallBrick()
        {
            if (!wallPrompted)
            {
                return dialogueController.StartDialogue(wallPages, () => wallPrompted = true);
            }

            // The card remains the required tool, but pressing E without selecting it now
            // repeats the clue instead of silently dropping the interaction.
            if (!IsSelectedItemId(LibraryCardItemId))
            {
                return dialogueController.StartDialogue(wallPages);
            }

            wallOpened = true;
            wallBrick.SetActive(false);
            thirdFragment.gameObject.SetActive(true);
            return true;
        }

        private bool TryPlaceFragment()
        {
            if (!inventory.TryRemoveSelectedById(FragmentItemId, out WorldItem fragment)) return false;

            if (fragmentTable == null)
            {
                fragmentTable = FindNearestTable();
            }

            GameObject placed = FindObject("Placed Library Fragment " + (placedFragments + 1));
            if (placed != null)
            {
                if (fragmentTable != null)
                {
                    int offsetIndex = placedFragments % 3;
                    placed.transform.position = fragmentTable.position
                        + new Vector3(-0.75f + offsetIndex * 0.75f, 0f, 0f);
                }

                placed.SetActive(true);
            }

            placedFragmentItems.Add(fragment);
            placedFragments++;
            if (placedFragments == 3 && !placementFinished)
            {
                placementFinished = true;
                dialogueController.StartDialogue(fragmentsPlacedPages);
            }

            return true;
        }

        private bool TryTakePlacedFragment()
        {
            if (placedFragmentItems.Count == 0) return false;

            WorldItem fragment = placedFragmentItems[placedFragmentItems.Count - 1];
            if (!inventory.TryStore(fragment)) return false;

            GameObject placed = FindObject("Placed Library Fragment " + placedFragments);
            if (placed != null) placed.SetActive(false);
            placedFragmentItems.RemoveAt(placedFragmentItems.Count - 1);
            placedFragments--;
            placementFinished = false;
            return true;
        }

        private bool CanInvestigateReadingTable()
        {
            // Once the page is complete, the table must no longer compete with the
            // completed page for the player's E interaction.
            if (pageCompleted) return false;

            Transform nearestTable = FindNearestTable();
            bool canTakeFragment = inventory.SelectedItem == null && placedFragments > 0
                && fragmentTable != null && nearestTable == fragmentTable;
            bool canPlaceFragment = !placementFinished
                && IsSelectedItemId(FragmentItemId);
            if (!placementFinished)
            {
                return canTakeFragment || canPlaceFragment;
            }

            return canTakeFragment || (nearestTable == fragmentTable && !pageCompleted
                && IsSelectedItemId(GlueItemId));
        }

        private bool TryInvestigateReadingTable()
        {
            if (inventory.SelectedItem == null && placedFragments > 0)
            {
                return TryTakePlacedFragment();
            }

            if (!placementFinished)
            {
                return false;
            }

            return dialogueController.StartDialogue(gluePages, () =>
            {
                if (inventory.TryRemoveSelectedById(GlueItemId, out _)) CompletePage();
            });
        }

        private void CompletePage()
        {
            pageCompleted = true;
            pushHint.Hide();

            Transform root = transform;
            while (root.parent != null) root = root.parent;
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.StartsWith("Placed Library Fragment "))
                {
                    child.gameObject.SetActive(false);
                }
            }

            placedFragmentItems.Clear();
            placedFragments = 0;

            completePage.transform.position = fragmentTable.position;
            completePage.gameObject.SetActive(true);
        }

        private bool BeginPageEnding()
        {
            if (endingStarted || completePage == null) return false;

            endingStarted = true;
            StartCoroutine(PlayPageEnding());
            return true;
        }

        private IEnumerator PlayPageEnding()
        {
            MainCharacterMovement movement = inventory.GetComponent<MainCharacterMovement>();
            PlayerInteraction interaction = inventory.GetComponent<PlayerInteraction>();
            Rigidbody2D body = inventory.GetComponent<Rigidbody2D>();
            if (movement != null) movement.enabled = false;
            if (interaction != null) interaction.enabled = false;
            inventory.enabled = false;
            if (body != null) body.velocity = Vector2.zero;

            GameObject canvasObject = new GameObject("Library Page Ending Canvas");
            Canvas endingCanvas = canvasObject.AddComponent<Canvas>();
            endingCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            endingCanvas.sortingOrder = 100;
            UiScaleSettings.Configure(canvasObject.AddComponent<CanvasScaler>());

            Image shade = CreateEndingImage("Background Shade", canvasObject.transform,
                new Color(0f, 0f, 0f, 0f));
            StretchToScreen(shade.rectTransform);

            Image page = CreateEndingImage("Complete Page", canvasObject.transform, Color.white);
            page.sprite = completePage.Icon;
            page.preserveAspect = true;
            page.rectTransform.anchorMin = page.rectTransform.anchorMax = new Vector2(.5f, .5f);
            page.rectTransform.pivot = new Vector2(.5f, .5f);
            page.rectTransform.anchoredPosition = Vector2.zero;

            const float unfoldDuration = 1.15f;
            float elapsed = 0f;
            while (elapsed < unfoldDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = Mathf.SmoothStep(0f, 1f, elapsed / unfoldDuration);
                page.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(100f, 90f),
                    new Vector2(760f, 690f), progress);
                shade.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, .72f, progress));
                yield return null;
            }

            yield return new WaitForSecondsRealtime(.55f);

            Image glow = CreateEndingImage("Page White Light", canvasObject.transform, Color.white);
            glow.sprite = CreateRadialGlowSprite();
            glow.rectTransform.anchorMin = glow.rectTransform.anchorMax = new Vector2(.5f, .5f);
            glow.rectTransform.pivot = new Vector2(.5f, .5f);
            glow.rectTransform.anchoredPosition = Vector2.zero;

            const float glowDuration = 2.2f;
            elapsed = 0f;
            while (elapsed < glowDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = Mathf.SmoothStep(0f, 1f, elapsed / glowDuration);
                float size = Mathf.Lerp(24f, 2600f, progress);
                glow.rectTransform.sizeDelta = new Vector2(size, size);
                glow.color = new Color(1f, 1f, 1f, Mathf.Lerp(.15f, 1f, progress));
                yield return null;
            }

            Image whiteScreen = CreateEndingImage("Permanent White Screen",
                canvasObject.transform, Color.white);
            StretchToScreen(whiteScreen.rectTransform);
            whiteScreen.transform.SetAsLastSibling();
        }

        private static Image CreateEndingImage(string name, Transform parent, Color color)
        {
            GameObject imageObject = new GameObject(name);
            imageObject.transform.SetParent(parent, false);
            Image image = imageObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            return image;
        }

        private static void StretchToScreen(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static Sprite CreateRadialGlowSprite()
        {
            const int size = 128;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "Library Page Radial Glow";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[size * size];
            Vector2 center = new Vector2((size - 1) * .5f, (size - 1) * .5f);
            float radius = size * .5f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center) / radius;
                    float alpha = 1f - Mathf.SmoothStep(.12f, 1f, distance);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, size, size),
                new Vector2(.5f, .5f), 100f);
        }

        private bool TryInvestigateCounter()
        {
            if (!counterInvestigated)
            {
                return dialogueController.StartDialogue(counterPages, () =>
                {
                    counterInvestigated = true;
                });
            }

            return dialogueController.StartDialogue(broomPages,
                () => StartCoroutine(PullGlueBottle()));
        }

        private IEnumerator PullGlueBottle()
        {
            gluePulling = true;
            glueBottle.gameObject.SetActive(true);
            Vector3 start = glueBottle.transform.position;
            Vector3 end = start + Vector3.down * 0.15f;
            float elapsed = 0f;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                glueBottle.transform.position = Vector3.Lerp(start, end, elapsed / 0.2f);
                yield return null;
            }

            glueBottle.transform.position = end;
            gluePulling = false;
            gluePulledOut = true;
            pushHint.Hide();
            glueBottle.SetPickupEnabled(true);
            inventory.TryStore(glueBottle);
        }

        private void HandleSecondFragmentPickedUp(WorldItem pickedUp)
        {
            if (pickedUp == secondFragment) dialogueController.StartDialogue(secondFragmentPages);
        }

        private bool IsSelectedItemId(string itemId)
        {
            return inventory.SelectedItem != null && inventory.SelectedItem.ItemId == itemId;
        }

        private static DialoguePage[] BuildPages(params string[] keys)
        {
            DialoguePage[] pages = new DialoguePage[keys.Length];
            for (int index = 0; index < keys.Length; index++)
            {
                pages[index] = new DialoguePage("主角", GameLocalization.Get(keys[index]));
            }

            return pages;
        }

        private WorldItem FindItem(string name)
        {
            return FindObject(name)?.GetComponent<WorldItem>();
        }

        private GameObject FindObject(string name)
        {
            Transform root = transform;
            while (root.parent != null) root = root.parent;
            foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
            {
                if (item.name == name) return item.gameObject;
            }

            return null;
        }

        private Transform FindNearestTable()
        {
            Transform nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (string name in new[] { "Reading Table Left", "Reading Table Right" })
            {
                Transform table = FindObject(name)?.transform;
                if (table == null) continue;
                float distance = (table.position - dialogueController.transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = table;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }
    }
}
