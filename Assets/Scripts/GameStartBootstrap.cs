using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public static class GameStartBootstrap
    {
        private const string InstructionsSceneName = "GameInstructions";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ShowInstructionsBeforeGameplay()
        {
            if (SceneManager.GetActiveScene().name != InstructionsSceneName)
            {
                SceneManager.LoadScene(InstructionsSceneName, LoadSceneMode.Single);
            }
        }
    }
}
