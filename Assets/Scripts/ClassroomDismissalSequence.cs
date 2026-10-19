using System.Collections;
using UnityEngine;

namespace RPG
{
    public sealed class ClassroomDismissalSequence : MonoBehaviour
    {
        private DialogueController dialogueController;
        private BroadcastDialogueTrigger openingBroadcast;
        private Transform professor;
        private readonly System.Collections.Generic.List<Transform> students =
            new System.Collections.Generic.List<Transform>();

        private void Start()
        {
            dialogueController = FindObjectOfType<DialogueController>();
            openingBroadcast = FindObjectOfType<BroadcastDialogueTrigger>();
            professor = FindObject("Professor")?.transform;
            for (int index = 1; index <= 5; index++)
            {
                Transform student = FindObject("Classroom Student " + index)?.transform;
                if (student != null) students.Add(student);
            }

            if (dialogueController == null || openingBroadcast == null
                || professor == null || students.Count != 4)
            {
                Debug.LogError("ClassroomDismissalSequence 缺少课堂离场对象。", this);
                enabled = false;
                return;
            }

            openingBroadcast.SetPages(BuildAfterClassPages());
            dialogueController.StartDialogue(BuildLecturePages(), PlayDismissal);
        }

        private void PlayDismissal()
        {
            dialogueController.StartDialogue(BuildDismissalPages(), () => StartCoroutine(DismissClass()));
        }

        private IEnumerator DismissClass()
        {
            var departures = new System.Collections.Generic.List<Coroutine>();
            for (int index = 0; index < students.Count; index++)
                departures.Add(StartCoroutine(MoveStudentOut(students[index], index * 0.35f)));

            yield return new WaitForSeconds(0.2f);
            bool studentDialogueFinished = false;
            if (!dialogueController.StartDialogue(BuildStudentDismissalPages(), () => studentDialogueFinished = true))
                studentDialogueFinished = true;

            foreach (Coroutine departure in departures)
                yield return departure;

            while (!studentDialogueFinished)
                yield return null;

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

        private static GameObject FindObject(string name)
        {
            foreach (Transform item in FindObjectsOfType<Transform>(true))
            {
                if (item.name == name) return item.gameObject;
            }

            return null;
        }

        private static DialoguePage[] BuildLecturePages()
        {
            string professor = GameLocalization.Get("classroom.speaker.professor");
            return new[]
            {
                new DialoguePage(professor, GameLocalization.Get("classroom.lecture.1")),
                new DialoguePage(professor, GameLocalization.Get("classroom.lecture.2")),
                new DialoguePage(professor, GameLocalization.Get("classroom.lecture.3"))
            };
        }

        private static DialoguePage[] BuildDismissalPages()
        {
            return new[] { new DialoguePage(GameLocalization.Get("classroom.speaker.professor"),
                GameLocalization.Get("classroom.dismissal.professor")) };
        }

        private static DialoguePage[] BuildStudentDismissalPages()
        {
            return new[]
            {
                new DialoguePage(GameLocalization.Get("classroom.speaker.student1"),
                    GameLocalization.Get("classroom.dismissal.student1")),
                new DialoguePage(GameLocalization.Get("classroom.speaker.student2"),
                    GameLocalization.Get("classroom.dismissal.student2"))
            };
        }

        private static DialoguePage[] BuildAfterClassPages()
        {
            return new[]
            {
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist1")),
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist2")),
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist3")),
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist4")),
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist5")),
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist6")),
                new DialoguePage("主角", GameLocalization.Get("classroom.afterClass.protagonist7"))
            };
        }
    }
}
