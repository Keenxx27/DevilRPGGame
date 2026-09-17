using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public sealed class LibraryBookPuzzle : MonoBehaviour, IInvestigationHandler,
        ISelectedItemUseHandler
    {
        private const string FragmentName = "残页碎片";
        private const string BroomName = "扫帚";
        private const string GlueName = "胶水";

        private static readonly DialoguePage[] OpeningPages =
        {
            new DialoguePage("主角", "这里比我想象中更荒凉。\n书架积着灰，连管理员都不在。"),
            new DialoguePage("主角", "电子检索只会给出被整理过的答案；\n真正难以归档的东西，往往躲在这些无人问津的角落。"),
            new DialoguePage("主角", "脆弱的手稿、私印的游记、没有被官方档案收录的地方志异……\n如果有关于真实的恶魔的线索，也许就在这里。"),
            new DialoguePage("主角", "先从书架开始找吧。")
        };
        private static readonly DialoguePage[] FirstBookcasePages =
        {
            new DialoguePage("主角", "这些古籍书名夸张，内容真伪难辨，但这正是我需要的——主流叙事之外的、残破的、带着人味的声音。")
        };
        private static readonly DialoguePage[] BookcasePagesOne =
        {
            new DialoguePage("主角", "某村樵夫坠入地穴，被发光蘑菇所救。")
        };
        private static readonly DialoguePage[] BookcasePagesTwo =
        {
            new DialoguePage("主角", "某族世代祭祀‘地母’，换取矿产丰饶。")
        };
        private static readonly DialoguePage[] BookcasePagesThree =
        {
            new DialoguePage("主角", "……暮年老叟言，其祖曾于矿道深处，遇一能言之石精。石精不害人，反指路避塌方。问其何所求，答曰：‘勿再深掘，惊我眠乡。’后矿竭，人散，地动亦息。"),
            new DialoguePage("主角", "……果真是荒谬至极。\n等等，掉出了什么东西？")
        };
        private static readonly DialoguePage[] ChairPages =
        {
            new DialoguePage("主角", "椅子底下像是压着什么。\n先把它推开看看。")
        };
        private static readonly DialoguePage[] WallPages =
        {
            new DialoguePage("主角", "这块墙砖松得不太自然，里面似乎卡着什么。\n借书卡边缘够硬，也许能把它撬出来。")
        };
        private static readonly DialoguePage[] SecondFragmentPages =
        {
            new DialoguePage("主角", "拼起来似乎能显示出一段话，\n最好把它们放到一张桌子上摊开来看。")
        };
        private static readonly DialoguePage[] FragmentsPlacedPages =
        {
            new DialoguePage("主角", "还得拿点东西将它们粘起来。")
        };
        private static readonly DialoguePage[] CounterPages =
        {
            new DialoguePage("主角", "柜台底下有一瓶胶水。\n可我的手够不到它。"),
            new DialoguePage("主角", "得找个长一点的东西，\n把它勾出来。")
        };
        private static readonly DialoguePage[] BroomPages =
        {
            new DialoguePage("主角", "扫帚够得着。\n得慢慢把它勾出来。")
        };
        private static readonly DialoguePage[] GluePages =
        {
            new DialoguePage("主角", "终于是找到能把它们粘起来的东西了……")
        };
        private static readonly DialoguePage[] CompletedPagePages =
        {
            new DialoguePage("主角", "粘好了。\n（被归墟侵蚀的痕迹在微微发热，不是刺痛，\n而是一种温暖的、引导般的脉动。）"),
            new DialoguePage("主角", "地母。石精。\n在更古老的记录里，恶魔似乎有过别的名字，\n不那么绝对，甚至带着一丝敬畏。"),
            new DialoguePage("主角", "让我读读这残页上写着什么……"),
            new DialoguePage("主角", "其形虽异，其求存之心同。\n（痕迹的灼热达到了顶点，不再是不适，\n而是一种清晰的、近乎共鸣的震颤。）"),
            new DialoguePage("主角", "求存之心。\n我要自己去看看。\n不是透过报告，不是透过枪口。"),
            new DialoguePage("主角", "去和它们……对话。\n先回家去吧。")
        };
        private static readonly DialoguePage[] RevisitCompletedPagePages =
        {
            new DialoguePage("主角", "让我读读这残页上写着什么……"),
            new DialoguePage("主角", "其形虽异，其求存之心同。"),
            new DialoguePage("主角", "我想我已经读过了，回家去吧。")
        };

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

            secondFragment.PickedUp += HandleSecondFragmentPickedUp;
            if (!dialogueController.StartDialogue(OpeningPages, () => openingFinished = true))
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
                        && (!wallPrompted || IsSelected("Library Card"));
                case InvestigationKind.LibraryReadingTable:
                    return CanInvestigateReadingTable();
                case InvestigationKind.LibraryCounter:
                    return !gluePulledOut && (!counterInvestigated
                        || (IsSelected(BroomName) && !broomPrompted)
                        || (IsSelected(BroomName) && gluePulls < 3 && !gluePulling));
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
                    return dialogueController.StartDialogue(RevisitCompletedPagePages);
                default: return false;
            }
        }

        public bool TryUseSelectedItem(InvestigationKind kind)
        {
            return kind == InvestigationKind.LibraryReadingTable
                && !placementFinished && IsSelected(FragmentName)
                && TryPlaceFragment();
        }

        private bool TryInvestigateBookcase()
        {
            DialoguePage[] pages = bookcaseInvestigations == 0 ? FirstBookcasePages
                : bookcaseInvestigations == 1 ? BookcasePagesOne
                : bookcaseInvestigations == 2 ? BookcasePagesTwo : BookcasePagesThree;
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
                return dialogueController.StartDialogue(ChairPages, () =>
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
                return dialogueController.StartDialogue(WallPages, () => wallPrompted = true);
            }

            wallOpened = true;
            wallBrick.SetActive(false);
            thirdFragment.gameObject.SetActive(true);
            return true;
        }

        private bool TryPlaceFragment()
        {
            if (!inventory.TryRemoveSelected(FragmentName, out WorldItem fragment)) return false;

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
                dialogueController.StartDialogue(FragmentsPlacedPages);
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
                && (!gluePrompted || glueApplications <= 3 || IsSelected(GlueName))
                && (gluePrompted || IsSelected(GlueName)));
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
                return dialogueController.StartDialogue(GluePages, () =>
                {
                    inventory.TryRemoveSelected(GlueName, out _);
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
            dialogueController.StartDialogue(CompletedPagePages);
        }

        private bool TryInvestigateCounter()
        {
            if (!counterInvestigated)
            {
                return dialogueController.StartDialogue(CounterPages, () =>
                {
                    counterInvestigated = true;
                });
            }

            if (!broomPrompted)
            {
                return dialogueController.StartDialogue(BroomPages, () =>
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
            if (pickedUp == secondFragment) dialogueController.StartDialogue(SecondFragmentPages);
        }

        private void ShowPushHint()
        {
            pushHint.Show("按 E 用力推开（" + chairPushes + "/3）");
        }

        private void ShowGlueHint()
        {
            pushHint.Show("按 E 用扫帚勾取（" + gluePulls + "/3）");
        }

        private void ShowGlueApplicationHint()
        {
            pushHint.Show("按 E 涂抹胶水（" + glueApplications + "/3）");
        }

        private bool IsSelected(string displayName)
        {
            return inventory.SelectedItem != null
                && inventory.SelectedItem.DisplayName == displayName;
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
