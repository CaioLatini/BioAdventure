using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Gameplay;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.UI;
using Unity.VisualScripting;



namespace BioAdventure.Assets.Script.Gameplay
{
    public class LevelController : MonoBehaviour
    {
        [Header("Referencia de Script")]
        [SerializeField] private GameController _gameController;

        [Header("Configuração do Nível")]
        [SerializeField] private List<LevelConfig> LevelConfig;

        [Header("Referências de Level")]
        [SerializeField] private TrashSpawner _trashSpawner;
        [SerializeField] private GameObject backGroundImage;
        [SerializeField] private List<Sprite> backGroundSprites;

        private LevelConfig _currentLevelConfig;


        private void Awake()
        {
            InitializeLevel(LevelConfig[GameManager.Instance.CurrentLevel]);
        }

        private void Start()
        {
            backGroundImage.GetComponent<SpriteRenderer>().sprite = backGroundSprites[GameManager.Instance.CurrentLevel];
        }

        public void InitializeLevel(LevelConfig levelConfig)
        {
            if (levelConfig == null)
            {
                Debug.LogError("LevelConfig não foi atribuído ao GameController!");
                this.enabled = false;
                return;
            }
            _currentLevelConfig = levelConfig;
        }

        public IEnumerator SpawnerRoutine()
        {
            Debug.Log("Spawn iniciado LevelController");
            while (_gameController.isGameRunning)
            {
                _trashSpawner.SpawTrash(_currentLevelConfig.maxTrashTypes, _currentLevelConfig.gravityInterval);
                float waitTime = UnityEngine.Random.Range(_currentLevelConfig.spawnInterval.x, _currentLevelConfig.spawnInterval.y);
                yield return new WaitForSeconds(waitTime);
            }
        }

        public void CheckWinCondition()
        {
            if (_gameController.collectedCount >= _currentLevelConfig.requiredCount)
            {
                StartCoroutine(WinSequence());
            }
            else if (_gameController.currentLives < 0)
            {
                StartCoroutine(LoseSequence());
            }
        }

        private IEnumerator WinSequence()
        {
            // 1. Chama e ESPERA o FinishGame
            yield return StartCoroutine(_gameController.FinishGame());

            // 2. DEPOIS que o jogador apertar "OK" em ShowInfo,

            // Achievement primeira vitoria
            AchievementsManager.Instance.UnlockAchievement(AchievementID.Victory);

            // Achievement venca todos os niveis
            if (GameManager.Instance.CurrentLevel == 4) AchievementsManager.Instance.UnlockAchievement(AchievementID.TheEnd);

            GameManager.Instance.SetCurrentPerformace(CalculatePerformance());

            // Achievement primeira vitoria perfeita
            if (CalculatePerformance() == 4) AchievementsManager.Instance.UnlockAchievement(AchievementID.Perfect);

            GameManager.Instance.SetCurrentWinState(true);
            GameManager.Instance.SetCurrentScore(_gameController.score);

            // Achievement pontuacao minima do level (errou tudo)
            if (_gameController.score == _currentLevelConfig.requiredCount) AchievementsManager.Instance.UnlockAchievement(AchievementID.WhatYouDoing);

            // Achievement venca todos os niveis com 4 estrelas
            if (LastAchievement()) AchievementsManager.Instance.UnlockAchievement(AchievementID.AbsoluteCinema);

            // 3. Mude a cena
            SceneController.Instance.GoToEndGameScene(true);
        }

        private IEnumerator LoseSequence()
        {
            // 1. Chama e ESPERA o FinishGame terminar
            yield return StartCoroutine(_gameController.FinishGame());

            // 2. DEPOIS que o jogador apertar "OK"

            // Achievement primeira derrota
            AchievementsManager.Instance.UnlockAchievement(AchievementID.Defeat);

            GameManager.Instance.SetCurrentWinState(false);
            GameManager.Instance.SetCurrentScore(_gameController.score);
            GameManager.Instance.SetCurrentPerformace(0);

            // 3. Mude a cena
            SceneController.Instance.GoToEndGameScene(false);
        }

        private int CalculatePerformance()
        {
            float required = _currentLevelConfig.requiredCount;
            if (_gameController.score * (_gameController.currentLives + 1) >= required * 2 * 4) return 4;
            if (_gameController.score * (_gameController.currentLives + 1) >= required * 1.7f * 3) return 3;
            if (_gameController.score * (_gameController.currentLives + 1) >= required * 1.4f * 2) return 2;
            return 1;
        }

        private bool LastAchievement()
        {
            int totalStar = 0;
            foreach (var performace in GameManager.Instance.CurrentUser.levelPerformace)
            {
                totalStar += performace;
            }
            Debug.Log("Total star:" + totalStar);
            return totalStar >= 20 ? true : false;
        }
    }
}