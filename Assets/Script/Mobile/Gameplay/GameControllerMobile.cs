using UnityEngine;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.UI;

namespace BioAdventure.Assets.Script.Gameplay.Mobile
{
    public class GameControllerMobile : MonoBehaviour
    {
        public static GameControllerMobile Instance { get; private set; }

        [Header("Referências de Scripts")]
        [SerializeField] private LevelControllerMobile _levelControllerMobile;
        [SerializeField] private GameUI _gameUI;

        public bool isGameRunning { get; private set; } = false;
        public int score { get; private set; } = 0;
        public int currentLives { get; private set; } = 3;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            TrashItem.OnCollected += HandleTrashCollected;
            TrashItem.OnMissed += HandleTrashMissed;
            PauseUI.OnPauseStateChanged += HandlePause;
        }

        private void OnDisable()
        {
            TrashItem.OnCollected -= HandleTrashCollected;
            TrashItem.OnMissed -= HandleTrashMissed;
            PauseUI.OnPauseStateChanged -= HandlePause;
        }

        private void Start()
        {
            StartCoroutine(StartCountDown());
        }
        
        public IEnumerator StartCountDown()
        {
            int seconds = 3;
            while (seconds >= 0)
            {
                _gameUI.ShowCountdown(seconds.ToString());
                if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect(seconds == 0 ? "finish" : "timer");
                
                seconds--;
                yield return new WaitForSeconds(1f);
            }
            
            _gameUI.HideCountdown();
            StartGameplay();
        }

        public void StartGameplay()
        {
            if (_levelControllerMobile != null)
            {
                isGameRunning = true;
                _levelControllerMobile.StartLevel(); // Chama a inicialização correta no LevelController
            }
        }

        // Chamado automaticamente pelo evento do PauseUI
        private void HandlePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0f : 1f;
            if (isPaused && SoundManager.Instance != null) 
            {
                SoundManager.Instance.RestartEffects(); // Corta sons residuais durante o pause
            }
        }

        private void HandleTrashCollected(bool wasCorrect, string currentTag)
        {
            if (!isGameRunning) return;

            score += wasCorrect ? 2 : 1;
            _gameUI.UpdateScore(score);

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEffect(wasCorrect ? "rightBin" : "wrongBin", wasCorrect ? 0.2f : 0.5f);
            
            _levelControllerMobile.RegisterCollection();
        }

        private void HandleTrashMissed()
        {
            if (!isGameRunning) return;

            currentLives--;
            _gameUI.UpdateLives(currentLives);

            if (SoundManager.Instance != null) 
                SoundManager.Instance.PlayEffect("wrongCatch", 0.7f);

            _levelControllerMobile.RegisterMiss();
        }

        // Chamados pelo LevelControllerMobile quando as condições são atingidas
        public void WinGame()
        {
            StartCoroutine(FinishGame(true));
        }

        public void LoseGame()
        {
            StartCoroutine(FinishGame(false));
        }

        private IEnumerator FinishGame(bool hasWon)
        {
            Debug.Log("Acabou a fase, você " + (hasWon ? "ganhou" : "perdeu"));
            isGameRunning = false;
            Time.timeScale = 0f;
            Debug.Log("O jogo parou");

            // Calcula estrelas (Performance) de forma simplificada. Ajuste a matemática conforme o seu design.
            int performance = hasWon ? currentLives + 1 : 0; 
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetCurrentScore(score);
                GameManager.Instance.SetCurrentPerformace(performance);
                GameManager.Instance.SetCurrentWinState(hasWon);
            }

            // Aguarda o jogador ler a curiosidade na tela
            Debug.Log("Exibindo informativo");
            yield return StartCoroutine(_gameUI.ShowInfo());

            Time.timeScale = 1f;
            Debug.Log("TimeScale retornando para 1");
            if (SceneController.Instance != null) 
            {
                SceneController.Instance.GoToEndGameScene(hasWon);
            }
        }
    }
}