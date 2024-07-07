using UnityEngine;

namespace RDCT.Menu
{
    public class MainMenu : MonoBehaviour
    {
        private static CanvasGroup cg;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        public static void PlayGame()
        {
            CloseMainMenu();

            SaveMenu.Instance.OpenWindow();
        } 

        public void OpenSettings()
        {
            CloseMainMenu();

            Settings.Instance.OpenWindow();
        }

        public static void ExitGame()
        {
            CloseMainMenu();

            ExitMenu.Instance.OpenWindow();
        }

        private static void CloseMainMenu()
        {
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }

        public static void ReOpenMainMenu()
        {
            cg.alpha = 1f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }
    }
}