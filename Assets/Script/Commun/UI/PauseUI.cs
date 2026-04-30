using UnityEngine;
using System;
using BioAdventure.Assets.Script.Managers;

// PauseUI.cs 
/*
Responsável por gerenciar o menu de pausa visualmente.
Ele controla a visibilidade do painel de pausa e dispara um evento global 
para que o GameController (PC ou Mobile) paralise ou retome o tempo de jogo.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class PauseUI : MonoBehaviour
    {
        // Instância local para que o DesktopInputManager possa acionar via tecla ESC
        public static PauseUI Instance { get; private set; }

        // Evento global que avisa aos Controllers de Gameplay se o jogo pausou ou despausou
        public static event Action<bool> OnPauseStateChanged;

        [Header("Referências da UI - Painéis")]
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject conteinerBin;
        [SerializeField] private GameObject conteinerBinPage1;
        [SerializeField] private GameObject conteinerBinPage2;
        
        [Header("Referências da UI - Botões")]
        [SerializeField] private GameObject buttonNextPage;
        [SerializeField] private GameObject buttonBackPage;

        private bool _isPaused = false;

        private void Awake()
        {
            Instance = this;
        }

        // Método central para alternar o estado de pausa. 
        // Pode ser chamado pelo botão na tela ou por atalhos de teclado.
        public void TogglePause()
        {
            _isPaused = !_isPaused;
            
            if (pauseMenuPanel != null) 
            {
                pauseMenuPanel.SetActive(_isPaused);
            }

            // Avisa o resto do jogo (especialmente GameController) para parar o Time.timeScale
            OnPauseStateChanged?.Invoke(_isPaused);
        }

        // Vincular ao botão de Pausa na HUD do jogo
        public void OnPauseButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            TogglePause();
        }

        public void OnMainMenuButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.GoToMainMenu();
        }

        public void OnMoreButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            Application.OpenURL("https://github.com/CaioLatini/BioAdventure");
        }

        public void OnQuitButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            Application.Quit();
        }

        public void OnRestartLevelButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.StartGame();
        }

        #region Lógica de Paginação da Lixeira (Bin Container)

        public void OnBinButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (conteinerBin != null) conteinerBin.SetActive(!conteinerBin.activeSelf);
        }

        public void OnChangePageBinPressed(bool next)
        {
            if (next)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("go");

                if (buttonNextPage != null) buttonNextPage.SetActive(false);
                if (buttonBackPage != null) buttonBackPage.SetActive(true);

                if (conteinerBinPage1 != null) conteinerBinPage1.SetActive(false);
                if (conteinerBinPage2 != null) conteinerBinPage2.SetActive(true);
            }
            else
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("back", 0.7f);

                if (buttonNextPage != null) buttonNextPage.SetActive(true);
                if (buttonBackPage != null) buttonBackPage.SetActive(false);

                if (conteinerBinPage1 != null) conteinerBinPage1.SetActive(true);
                if (conteinerBinPage2 != null) conteinerBinPage2.SetActive(false);
            }
        }

        #endregion
    }
}