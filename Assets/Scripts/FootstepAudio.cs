using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public enum FootstepKind { Human, LowMember, AdvancedMember, Boss, Thirteen }

    public sealed class FootstepAudio : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallForLoadedScenes()
        {
            SceneManager.sceneLoaded += (_, __) => Install();
            Install();
        }

        private static void Install()
        {
            Add<MainCharacterMovement>(FootstepKind.Human);
            Add<LinChunbaoVisualAnimator>(FootstepKind.Human);
            Add<ProfessorVisualAnimator>(FootstepKind.Human);
            Add<ClassroomStudentVisualAnimator>(FootstepKind.Human);
            Add<GangDogLowMember>(FootstepKind.LowMember);
            Add<GangDogAdvancedMember>(FootstepKind.AdvancedMember);
            Add<GangDogBoss>(FootstepKind.Boss);
            Add<ThirteenVisualAnimator>(FootstepKind.Thirteen);
            foreach (Transform target in Object.FindObjectsOfType<Transform>())
            {
                if (!target.name.StartsWith("猎魔机构行政人员")) continue;
                FootstepAudio audio = target.GetComponent<FootstepAudio>();
                if (audio == null) audio = target.gameObject.AddComponent<FootstepAudio>();
                audio.Configure(FootstepKind.Human);
            }
        }

        private static void Add<T>(FootstepKind value) where T : Component
        {
            foreach (T target in Object.FindObjectsOfType<T>())
            {
                FootstepAudio audio = target.GetComponent<FootstepAudio>();
                if (audio == null) audio = target.gameObject.AddComponent<FootstepAudio>();
                audio.Configure(value);
            }
        }
        [SerializeField] private FootstepKind kind;
        private Vector2 lastPosition;
        private float distance;
        private AudioSource source;
        private AudioClip clip;

        public void Configure(FootstepKind value) { kind = value; }

        private void Awake()
        {
            lastPosition = transform.position;
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            clip = Resources.Load<AudioClip>("Audio/Footsteps/" + ClipName(kind));
        }

        private void LateUpdate()
        {
            distance += Vector2.Distance(transform.position, lastPosition);
            if (clip != null && distance >= StepDistance(kind)) { source.PlayOneShot(clip, .42f); distance = 0f; }
            lastPosition = transform.position;
        }

        private static string ClipName(FootstepKind value)
        {
            return value == FootstepKind.LowMember ? "low_member" : value == FootstepKind.AdvancedMember ? "advanced_boot" : value == FootstepKind.Boss ? "boss_boot" : value == FootstepKind.Thirteen ? "thirteen_paw" : "human_indoor";
        }

        private static float StepDistance(FootstepKind value) { return value == FootstepKind.Thirteen ? .24f : value == FootstepKind.Boss ? .52f : .36f; }
    }
}
