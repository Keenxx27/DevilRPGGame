using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private DialoguePage[] firstBookcasePages;
        private DialoguePage[] bookcasePagesOne;
        private DialoguePage[] bookcasePagesTwo;
        private DialoguePage[] bookcasePagesThree;
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
        private bool chairPrompted;
        private bool chairMoving;
        private bool wallPrompted;
        private bool wallOpened;
        private bool placementFinished;
        private int gluePulls;
        private bool counterInvestigated;
        private bool broomPrompted;
        private bool gluePulling;
        private bool gluePulledOut;
        private bool gluePrompted;
        private int glueApplications;
        private bool pageCompleted;

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
            firstBookcasePages = BuildPages("library.environment.bookcase.first");
            bookcasePagesOne = BuildPages("library.environment.bookcase.1");
            bookcasePagesTwo = BuildPages("library.environment.bookcase.2");
            bookcasePagesThree = BuildPages("library.environment.bookcase.4.1",
                "library.environment.bookcase.4.2");
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
                    return bookcaseInvestigations < 4;
                case InvestigationKind.LibraryChair:
                    return bookcaseInvestigations == 4 && chairPushes < 3 && !chairMoving;
                case InvestigationKind.LibraryWallBrick:
                    return bookcaseInvestigations == 4 && !wallOpened
                        && (!wallPrompted || IsSelectedItemId(LibraryCardItemId));
                case InvestigationKind.LibraryReadingTable:
                    return CanInvestigateReadingTable();
                case InvestigationKind.LibraryCounter:
                    return !gluePulledOut && (!counterInvestigated
                        || (IsSelectedItemId(BroomItemId) && !broomPrompted)
                        || (IsSelectedItemId(BroomItemId) && gluePulls < 3 && !gluePulling));
                case InvestigationKind.LibraryCompletePage:
                    return pageCompleted;
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
                    return dialogueController.StartDialogue(revisitCompletedPagePages);
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
            DialoguePage[] pages = bookcaseInvestigations == 0 ? firstBookcasePages
                : bookcaseInvestigations == 1 ? bookcasePagesOne
                : bookcaseInvestigations == 2 ? bookcasePagesTwo : bookcasePagesThree;
            int investigation = bookcaseInvestigations;
            return dialogueController.StartDialogue(pages,
                () => FinishBookcaseInvestigation(investigation));
        }

        private void FinishBookcaseInvestigation(int investigation)
        {
            bookcaseInvestigations++;
            if (investigation == 3 && !inventory.TryStore(firstFragment))
            {
                firstFragment.gameObject.SetActive(true);
            }
        }

        private bool TryInvestigateChair()
        {
            if (!chairPrompted)
            {
                return dialogueController.StartDialogue(chairPages, () =>
                {
                    chairPrompted = true;
                    ShowPushHint();
                });
            }

            StartCoroutine(PushChair());
            return true;
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
            chairPushes++;
            chairMoving = false;
            if (chairPushes == 3)
            {
                pushHint.Hide();
                secondFragment.gameObject.SetActive(true);
                yield break;
            }

            ShowPushHint();
        }

        private bool TryInvestigateWallBrick()
        {
            if (!wallPrompted)
            {
                return dialogueController.StartDialogue(wallPages, () => wallPrompted = true);
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
            Transform nearestTable = FindNearestTable();
            bool canTakeFragment = inventory.SelectedItem == null && placedFragments > 0
                && fragmentTable != null && nearestTable == fragmentTable && !gluePrompted;
            if (!placementFinished)
            {
                return canTakeFragment;
            }

            return canTakeFragment || (nearestTable == fragmentTable && !pageCompleted
                && (!gluePrompted || glueApplications <= 3 || IsSelectedItemId(GlueItemId))
                && (gluePrompted || IsSelectedItemId(GlueItemId)));
        }

        private bool TryInvestigateReadingTable()
        {
            if (inventory.SelectedItem == null && placedFragments > 0 && !gluePrompted)
            {
                return TryTakePlacedFragment();
            }

            if (!placementFinished)
            {
                return false;
            }

            if (!gluePrompted)
            {
                return dialogueController.StartDialogue(gluePages, () =>
                {
                    inventory.TryRemoveSelectedById(GlueItemId, out _);
                    gluePrompted = true;
                    ShowGlueApplicationHint();
                });
            }

            if (glueApplications < 3)
            {
                glueApplications++;
                ShowGlueApplicationHint();
                return true;
            }

            CompletePage();
            return true;
        }

        private void CompletePage()
        {
            pageCompleted = true;
            pushHint.Hide();
            for (int index = 1; index <= 3; index++)
            {
                GameObject fragment = FindObject("Placed Library Fragment " + index);
                if (fragment != null) fragment.SetActive(false);
            }

            completePage.transform.position = fragmentTable.position;
            completePage.gameObject.SetActive(true);
            dialogueController.StartDialogue(completedPagePages);
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

            if (!broomPrompted)
            {
                return dialogueController.StartDialogue(broomPages, () =>
                {
                    broomPrompted = true;
                    ShowGlueHint();
                });
            }

            StartCoroutine(PullGlueBottle());
            return true;
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
            gluePulls++;
            gluePulling = false;
            if (gluePulls < 3)
            {
                ShowGlueHint();
                yield break;
            }

            gluePulledOut = true;
            pushHint.Hide();
            glueBottle.SetPickupEnabled(true);
            inventory.TryStore(glueBottle);
        }

        private void HandleSecondFragmentPickedUp(WorldItem pickedUp)
        {
            if (pickedUp == secondFragment) dialogueController.StartDialogue(secondFragmentPages);
        }

        private void ShowPushHint()
        {
            pushHint.Show(GameLocalization.Format("library.hint.push", chairPushes));
        }

        private void ShowGlueHint()
        {
            pushHint.Show(GameLocalization.Format("library.hint.hook", gluePulls));
        }

        private void ShowGlueApplicationHint()
        {
            pushHint.Show(GameLocalization.Format("library.hint.apply", glueApplications));
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
