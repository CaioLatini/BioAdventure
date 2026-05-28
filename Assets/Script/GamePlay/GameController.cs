using UnityEngine;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.UI;

namespace BioAdventure.Assets.Script.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [Header("Referências de Scripts")]
        [SerializeField] private LevelController _levelController;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private TutorialController _tutorialController;

        public bool isGameRunning { get; private set; } = false;
        public int score { get; private set; } = 0;
        public int currentLives { get; private set; } = 3;
        public int currentRequiredCount { get; private set; } = 0;

        private void Awake()
        {
            Instance = this;
            currentRequiredCount = GameManager.Instance.levels[GameManager.Instance.CurrentLevel].requiredCount;
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
            if(GameManager.Instance.CurrentUser.TutMoveComplete) StartCoroutine(StartCountDown());
            else StartTutorial();
        }

        public IEnumerator StartCountDown()
        {
            _gameUI.ShowCountdown();
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
            if (_levelController != null)
            {
                isGameRunning = true;
                _levelController.StartLevel(); // Chama a inicialização correta no LevelController
            }
        }

        private void StartTutorial()
        {
            _gameUI.HideCountdown();
            _tutorialController.TutorialStep("Grab");
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

            _levelController.RegisterCollection();
        }

        private void HandleTrashMissed()
        {
            if (!isGameRunning) return;

            _gameUI.PulseBackGround();

            currentLives--;
            _gameUI.UpdateLives(currentLives);

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEffect("wrongCatch", 0.7f);

            _levelController.RegisterMiss();
        }

        // Chamados pelo LevelController quando as condições são atingidas
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


            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetCurrentScore(score);
                GameManager.Instance.SetCurrentPerformace(CalculatePerformace(score, currentLives, currentRequiredCount));
                GameManager.Instance.SetCurrentWinState(hasWon);
            }

            if (AchievementsManager.Instance != null)
            {
                AchievementsManager.Instance.ValidateEndGameAchievements(hasWon);
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
        private int CalculatePerformace(int score, int lives, int currentRequiredCount)
        {
            if(lives <= 0) return 0;
            if (score >= currentRequiredCount * 2 && lives == 3) //perfeito
            {
                return 4;
            }
            else if (score >= currentRequiredCount * 1.8f && lives >= 2) //80% de acerto 2 vidas
            {
                return 3;
            }
            else if (score >= currentRequiredCount * 1.5f) //50% de acerto
            {
                return 2;
            }
            else return 1; // resto
        }
    }
}