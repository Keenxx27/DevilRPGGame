using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public static class GameStartBootstrap
    {
        private const string LanguageSelectionSceneName = "LanguageSelection";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ShowInstructionsBeforeGameplay()
        {
            if (SceneManager.GetActiveScene().name != LanguageSelectionSceneName)
            {
                SceneManager.LoadScene(LanguageSelectionSceneName, LoadSceneMode.Single);
            }
        }
    }
}
