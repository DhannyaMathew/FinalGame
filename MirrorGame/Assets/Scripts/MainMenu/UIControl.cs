using System;
using UnityEngine;

namespace MainMenu
{
    public class UiControl : MonoBehaviour
    {
        private static UiControl instance;

        [Serializable]
        public enum PauseMenuState
        {
            Off,
            MainPauseMenu,
            Settings
        }

        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject settingsScreen;
        [SerializeField] private GameObject mainMenuButton;
        [SerializeField] private GameObject interactUi;
        private PauseMenuState _pauseState = PauseMenuState.Off;


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
            
            _pauseState = PauseMenuState.Off;
        
        }

        private static PauseMenuState PauseState
        {
            get => instance == null ? PauseMenuState.Off : instance._pauseState;
            set
            {
                if (instance == null) return;
                instance._pauseState = value;
                switch (value)
                {
                    case PauseMenuState.Off:
                        instance.settingsScreen.SetActive(false);
                        instance.pauseScreen.SetActive(false);
                        GameManager.Paused = false;
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        return;
                    case PauseMenuState.MainPauseMenu:
                        instance.settingsScreen.SetActive(false);
                        instance.pauseScreen.SetActive(true);
                        instance.mainMenuButton.SetActive(GameManager.CurrentLevelIndex != 1);
                        GameManager.Paused = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        return;
                    case PauseMenuState.Settings:
                        instance.settingsScreen.SetActive(true);
                        instance.pauseScreen.SetActive(false);
                        GameManager.Paused = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        return;
                }
            }
        }

        public static bool MustPause => PauseState != PauseMenuState.Off;

        public static void OnEscape()
        {
            switch (PauseState)
            {
                case PauseMenuState.Off:
                    PauseState = PauseMenuState.MainPauseMenu;
                    break;
                case PauseMenuState.MainPauseMenu:
                    PauseState = PauseMenuState.Off;
                    break;
                case PauseMenuState.Settings:
                    PauseState = PauseMenuState.MainPauseMenu;
                    break;
            }
        }

        public static void ShowInteractUI()
        {
            if (instance != null) instance.interactUi.SetActive(true);
        }

        public static void HideInteractUI()
        {
            
            if (instance != null) instance.interactUi.SetActive(false);
        }

        public void OnResume()
        {
            PauseState = PauseMenuState.Off;
        }


        public void OnSettings()
        {
            PauseState = PauseMenuState.Settings;
            
        }

        public void OnMainMenu()
        {
            PauseState = PauseMenuState.MainPauseMenu;
        }


        public static void TurnOffMenu()
        {
            PauseState = PauseMenuState.Off;
        }
    }
}