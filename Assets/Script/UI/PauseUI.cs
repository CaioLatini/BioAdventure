using UnityEngine;
using BioAdventure.Assets.Script.Core; // Para InputManager
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.Gameplay; // Para SceneController

// PauseMenu.cs 
/*
Esse script será responsável por gerenciar o menu de pausa.
Ele controlará a visibilidade do painel de pausa e pausará/despausará o jogo
*/

namespace BioAdventure.Assets.Script.UI
{
    public class PauseUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private GameObject pauseMenuPanel;

        [SerializeField] private GameObject conteinerBin;
        [SerializeField] private GameObject conteinerBinPage1;
        [SerializeField] private GameObject conteinerBinPage2;


        private bool _isPaused = false;

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.onPause += Toggle;
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.onPause -= Toggle;
            }
        }
        public void Toggle()
        {
            pauseMenuPanel.SetActive(!_isPaused);
            _isPaused = !_isPaused;
        }

        public void OnPauseButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");
            InputManager.Instance.TriggerPause();
        }

        public void OnMainMenuButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            SceneController.Instance.GoToMainMenu();
        }

        public void OnMoreButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            Application.OpenURL("https://github.com/CaioLatini/BioAdventure");
        }

        public void OnQuitButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            Application.Quit();
        }

        public void OnBinButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            conteinerBin.SetActive(!conteinerBin.activeSelf);
        }

        public void OnChangePageBinPressed(bool next)
        {
            if (next)
            {
                SoundManager.Instance.PlayEffect("go");

                conteinerBinPage1.SetActive(false);
                conteinerBinPage2.SetActive(true);
            }
            else
            {
                SoundManager.Instance.PlayEffect("back", 0.7f);

                conteinerBinPage1.SetActive(true);
                conteinerBinPage2.SetActive(false);
            }
        }
        
        public void OnRestartLevelButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            SceneController.Instance.StartGame();
        }
    }
}