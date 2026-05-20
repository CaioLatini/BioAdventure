using UnityEngine;
using UnityEngine.SceneManagement;
using BioAdventure.Assets.Script.Core;

namespace BioAdventure.Assets.Script.Managers
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

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


        // Carrega o Menu Principal
        public void GoToMainMenu()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicMenu");

            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuScene");
        }


        // Carrega a cena principal de Gameplay
        public void StartGame()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicGame");

            Time.timeScale = 1f;
            SceneManager.LoadScene("GameScene");
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
            
            SceneManager.LoadScene("EndGameScene");
        }
    }
}