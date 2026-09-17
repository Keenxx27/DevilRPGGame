using System.Collections;
using UnityEngine;

namespace RPG
{
    public sealed class ClassroomDismissalSequence : MonoBehaviour
    {
        private static readonly DialoguePage[] LecturePages =
        {
            new DialoguePage("教授", "所以同学们要明确，"),
            new DialoguePage("教授", "恶魔是归墟侵蚀具象化的、充满攻击性的副产物。"),
            new DialoguePage("教授", "恶魔猎人的任务，就是像清除癌细胞一样，\n将它们从地球的肌体上彻底清除。")
        };
        private static readonly DialoguePage[] DismissalPages =
        {
            new DialoguePage("教授", "下课铃响了，同学们下课吧。")
        };

        private DialogueController dialogueController;
        private BroadcastDialogueTrigger openingBroadcast;
        private DoorController door;
        private Transform professor;
        private readonly System.Collections.Generic.List<Transform> students =
            new System.Collections.Generic.List<Transform>();

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            openingBroadcast = FindObjectOfType<BroadcastDialogueTrigger>();
            door = FindObject("Door")?.GetComponent<DoorController>();
            professor = FindObject("Professor")?.transform;
            for (int index = 1; index <= 5; index++)
            {
                Transform student = FindObject("Classroom Student " + index)?.transform;
                if (student != null) students.Add(student);
            }

            if (dialogueController == null || openingBroadcast == null || door == null
                || professor == null || students.Count != 5)
            {
                Debug.LogError("ClassroomDismissalSequence 缺少课堂离场对象。", this);
                enabled = false;
                return;
            }

            dialogueController.StartDialogue(LecturePages, PlayDismissal);
        }

        private void PlayDismissal()
        {
            dialogueController.StartDialogue(DismissalPages, () => StartCoroutine(DismissClass()));
        }

        private IEnumerator DismissClass()
        {
            EnsureDoorOpen();
            var departures = new System.Collections.Generic.List<Coroutine>();
            for (int index = 0; index < students.Count; index++)
                departures.Add(StartCoroutine(MoveStudentOut(students[index], index * 0.35f)));

            foreach (Coroutine departure in departures)
                yield return departure;

            yield return MoveAlongRoute(professor, new[]
            {
                new Vector2(5.45f, 3.35f), new Vector2(5.45f, -3.25f),
                new Vector2(7.45f, -3.25f)
            });
            yield return new WaitForSeconds(0.2f);
            professor.gameObject.SetActive(false);
            openingBroadcast.Trigger();
        }

        private IEnumerator MoveStudentOut(Transform student, float delay)
        {
            yield return new WaitForSeconds(delay);
            float rowY = student.position.y;
            yield return MoveAlongRoute(student, new[]
            {
                new Vector2(5.45f, rowY), new Vector2(5.45f, -3.25f),
                new Vector2(7.45f, -3.25f)
            });
            yield return new WaitForSeconds(0.15f);
            student.gameObject.SetActive(false);
        }

        private IEnumerator MoveAlongRoute(Transform actor, Vector2[] points)
        {
            foreach (Vector2 point in points)
            {
                while (actor != null && Vector2.Distance(actor.position, point) > 0.02f)
                {
                    float step = 2.6f * Time.deltaTime;
                    actor.position = Vector2.MoveTowards(actor.position, point, step);
                    yield return null;
                }
            }
        }

        private void EnsureDoorOpen()
        {
            if (door.State == DoorState.Closed)
            {
                door.ToggleDoor();
            }
        }

        private static GameObject FindObject(string name)
        {
            foreach (Transform item in FindObjectsOfType<Transform>(true))
            {
                if (item.name == name) return item.gameObject;
            }

            return null;
        }
    }
}
