using UnityEngine;

namespace RPG
{
    /// <summary>
    /// Keeps the existing scene component identity, but Escape now exits the game instead of
    /// opening the former pause overlay.
    /// </summary>
    public sealed class GamePauseController : MonoBehaviour
    {
        private static bool quitting;

        private void Awake()
        {
            quitting = false;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!quitting && Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
        }

        private static void QuitGame()
        {
            quitting = true;
            Time.timeScale = 1f;

#if UNITY_EDITOR
            // During testing, stop Play Mode without closing the Unity Editor itself.
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
