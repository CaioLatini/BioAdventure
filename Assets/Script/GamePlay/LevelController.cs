using UnityEngine;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Data;


namespace BioAdventure.Assets.Script.Gameplay
{
    public class LevelController : MonoBehaviour
    {
        private LevelConfig currentLevel;
        
        [Header("Referências")]
        [Tooltip("Referência ao Spawner responsável por instanciar os lixos na cena.")]
        [SerializeField] private TrashSpawner _trashSpawner;

        [SerializeField] private GameObject _tutorial;

        public int CollectedCount { get; private set; }
        public int MissedCount { get; private set; }
        private bool _isLevelActive;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            if(GameManager.Instance.CurrentUser.TutCaptureComplete) Destroy(_tutorial);
        }

        public void StartLevel()
        {
            if (_gameManager == null || _trashSpawner == null)
            {
                Debug.LogError("[LevelController] Configuração ou Spawner ausentes!");
                return;
            }
            currentLevel = _gameManager.levels[_gameManager.CurrentLevel];
            Debug.Log("Nivel atual: " + _gameManager.CurrentLevel);

            CollectedCount = 0;
            MissedCount = 0;
            _isLevelActive = true;
            
            StartCoroutine(SpawnerRoutine());
        }

        // Interrompe a geração de lixo e a lógica da fase
        public void StopLevel()
        {
            _isLevelActive = false;
            StopAllCoroutines();
        }

        // Rotina de geração contínua de lixos no intervalo de tempo
        private IEnumerator SpawnerRoutine()
        {
            while (_isLevelActive)
            {
                _trashSpawner.SpawTrash(currentLevel.gravity, currentLevel.HardSpot, currentLevel.DobleTrash, currentLevel.OddDouble);
                if(!GameManager.Instance.CurrentUser.TutCaptureComplete) yield return new WaitForSeconds(currentLevel.spawnTemp+2);
                yield return new WaitForSeconds(currentLevel.spawnTemp);
            }
        }

        public void RegisterCollection()
        {
            if (!_isLevelActive) return;

            CollectedCount++;

            CheckWinCondition();
        }

        // Incrementa o contador de lixos perdidos (caíram no chão)
        public void RegisterMiss()
        {
            if (!_isLevelActive) return;
            
            MissedCount++;
            CheckWinCondition();
        }

        // Verifica constantemente se as metas de vitória ou limites de derrota foram atingidos
        private void CheckWinCondition()
        {
            if (CollectedCount >= currentLevel.requiredCount)
            {
                StopLevel();
                if (GameController.Instance != null) GameController.Instance.WinGame();
            }
            else if (MissedCount >= 3) 
            {
                StopLevel();
                if (GameController.Instance != null) GameController.Instance.LoseGame();
            }
        }
    }
}