using UnityEngine;
using UnityEngine.SceneManagement;
using BioAdventure.Assets.Script.Core;
using NUnit.Framework;

// SceneController.cs
/*
Esse script será responsável pelo gerenciamento e navegação de cenas.
Ele conterá métodos públicos de navegação que serão chamados pelos botões da UI. 
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SceneController : MonoBehaviour
    {
        private string AuthSceneName;
        private string MainMenuSceneName;
        private string GameSceneName;
        private string EndGameScene;
        private string TutorialSceneName;


        public static SceneController Instance { get; private set; }
        private void Awake()
        {
            #if UNITY_ANDROID  
            AuthSceneName = "LoginScene - Mobile";
            MainMenuSceneName = "MenuScene - Mobile";
            GameSceneName = "GameScene - Mobile";
            EndGameScene = "EndGameScene - Mobile";
            TutorialSceneName = "TutorialScene - Mobile";
            #else
            AuthSceneName = "LoginScene";
            MainMenuSceneName = "MenuScene";
            GameSceneName = "GameScene";
            EndGameScene = "EndGameScene";
            TutorialSceneName = "TutorialScene";
            #endif
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            Debug.Log("SceneController Initialized");
        }


        public void GoToAuthScene()
        {
            SoundManager.Instance.PlayMusic("musicMenu");

            Time.timeScale = 1f;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartSession(null);
            }
            SceneManager.LoadScene(AuthSceneName);
        }

        public void GoToMainMenu()
        {
            SoundManager.Instance.PlayMusic("musicMenu");

            Time.timeScale = 1f;
            SceneManager.LoadScene(MainMenuSceneName);
        }
        public void StartTuturial()
        {
            SoundManager.Instance.PlayMusic("musicGame");
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(TutorialSceneName);
        }

        public void StartGame()
        {
            SoundManager.Instance.PlayMusic("musicGame");

            Time.timeScale = 1f;
            SceneManager.LoadScene(GameSceneName);
        }

        public void GoToNextLevel()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeLevel(true);
            }
            StartGame();
        }

        public void GoToEndGameScene(bool hasWon)
        {
            //TODO - Músicas específica Win and Lose
            SoundManager.Instance.PlayMusic(hasWon? "musicMenu" : "musicLose");
            
            SceneManager.LoadScene(EndGameScene);
        }

    }
}