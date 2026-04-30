using UnityEngine;
using UnityEngine.SceneManagement;
using BioAdventure.Assets.Script.Core;

// SceneController.cs
/*
Responsável pelo gerenciamento e navegação fluida entre as cenas do jogo.
Ele avalia a plataforma em tempo de execução e carrega a cena apropriada 
(versão Desktop ou Mobile) sem depender de diretivas de pré-compilação no código.
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        [Header("Nomes Base das Cenas")]
        [Tooltip("Sufixo adicionado automaticamente às cenas quando rodando em dispositivos móveis.")]
        private const string MobileSuffix = " - Mobile";

        private const string AuthSceneName = "LoginScene";
        private const string MainMenuSceneName = "MenuScene";
        private const string GameSceneName = "GameScene";
        private const string EndGameSceneName = "EndGameScene";
        private const string TutorialSceneName = "TutorialScene";

        // Configura o Singleton
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Produção - Método auxiliar que anexa o sufixo " - Mobile" se estiver rodando em Android/iOS
        // private string GetPlatformSceneName(string baseName)
        // {
        //     if (Application.isMobilePlatform)
        //     {
        //         return baseName + MobileSuffix;
        //     }
        //     return baseName;
        // }

        // Temporario para testes
        private string GetPlatformSceneName(string baseName)
        {
            return baseName + MobileSuffix;
        }

        // Carrega a tela de Login (ou desloga o usuário caso chamado do Menu)
        public void GoToAuthScene()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicMenu");

            Time.timeScale = 1f; // Garante que o jogo não fique pausado

            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartSession(null); // Limpa a sessão atual
            }

            SceneManager.LoadScene(GetPlatformSceneName(AuthSceneName));
        }

        // Carrega o Menu Principal
        public void GoToMainMenu()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicMenu");

            Time.timeScale = 1f;
            SceneManager.LoadScene(GetPlatformSceneName(MainMenuSceneName));
        }

        // Carrega a cena de Tutorial
        public void StartTutorial()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicGame");
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(GetPlatformSceneName(TutorialSceneName));
        }

        // Carrega a cena principal de Gameplay
        public void StartGame()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicGame");

            Time.timeScale = 1f;
            SceneManager.LoadScene(GetPlatformSceneName(GameSceneName));
        }

        // Avança o nível no GameManager e reinicia a cena de Gameplay
        public void GoToNextLevel()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeLevel(true);
            }
            StartGame();
        }

        // Carrega a cena de Fim de Jogo (Vitória ou Derrota)
        public void GoToEndGameScene(bool hasWon)
        {
            if (SoundManager.Instance != null) 
            {
                SoundManager.Instance.PlayMusic(hasWon ? "musicMenu" : "musicLose");
            }
            
            SceneManager.LoadScene(GetPlatformSceneName(EndGameSceneName));
        }
    }
}