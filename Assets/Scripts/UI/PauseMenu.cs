using UnityEngine;
using RDCT.Menu.SettingsMenu;
using UnityEngine.SceneManagement;

namespace RDCT.Menu
{
    public class PauseMenu : MonoBehaviour, IMenuWindow
    {
        private static CanvasGroup cg;
        private static bool currentlyPaused;
        private static bool isInPauseMenu;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        private void TogglePauseMenu()
        {
            if (!isInPauseMenu && currentlyPaused) return;

            if (currentlyPaused)
            {
                currentlyPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1.0f;
                CloseWindow();
            }
            else
            {
                currentlyPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                OpenWindow();
            }
        }

        public void OpenSettings()
        {
            CloseWindow();
            Settings.Instance.OpenWindow();
        }

        public void ClosePauseMenu()
        {
            currentlyPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1.0f;
            CloseWindow();
        }

        public void QuitGame()
        {
            SceneManager.LoadScene("MainMenu 1");
        }

        public void CloseWindow()
        {
            isInPauseMenu = false;

            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }

        public void OpenWindow()
        {
            isInPauseMenu = true;

            cg.alpha = 1f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }
    }
}
