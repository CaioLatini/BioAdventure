using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Gameplay;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.UI;
using Unity.VisualScripting;
using System;



namespace BioAdventure.Assets.Script.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [Header("Referencia de Script")]
        [SerializeField] private LevelController _levelController;

        [Header("Referências de Gameplay")]
        [SerializeField] private BinController _binController;
        [SerializeField] private RadialMenuController _radialMenuController;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private GameObject _radialMenu;

        public bool isGameRunning { get; private set; } = false;
        public int score { get; private set; } = 0;
        public int currentLives { get; private set; } = 3;
        public int collectedCount { get; private set; } = 0;
        
        private int _seconds = 3;

        private void OnEnable()
        {
            InputManager.Instance.onPause += TogglePause;
            TrashItem.OnCollected += HandleTrashCollected;
            TrashItem.OnMissed += HandleTrashMissed;
        }

        private void OnDisable()
        {
            InputManager.Instance.onPause -= TogglePause;
            TrashItem.OnCollected -= HandleTrashCollected;
            TrashItem.OnMissed -= HandleTrashMissed;
        }

        private void Start()
        {
            StartCoroutine(StartCoutnDown());
        }
        
        public IEnumerator StartCoutnDown()
        {
            while (_seconds >= 0)
            {
                _gameUI.ShowCountdown(_seconds.ToString());
                _seconds--;
                SoundManager.Instance.PlayEffect(_seconds < 0 ? "finish" : "timer");
                yield return new WaitForSeconds(1f);
            }
            _gameUI.HideCountdown();
            StartGameplay();
        }

        public void StartGameplay()
        {
            try
            {
                isGameRunning = true;
                StartCoroutine(_levelController.SpawnerRoutine());
                Debug.Log("Spawn iniciado GameControll");
            }
            catch
            {
                Debug.Log("tuturial scene");
            }
        }

        public void PauseGameplay()
        {
            _radialMenu.SetActive(false);
            Time.timeScale = 0f;
            SoundManager.Instance.RestartEffects();
        }

        public void ResumeGameplay()
        {
            _radialMenuController.isMenuOpen = false;
            _radialMenu.SetActive(true);
            Time.timeScale = 1f;
        }

        public void TogglePause()
        {
            if (Time.timeScale > 0f)
            {
                PauseGameplay();
            }
            else
            {
                ResumeGameplay();
            }
        }

        public void HandleBinChange(string key)
        {
            _binController.ChangeBinType(key);
            SoundManager.Instance.PlayEffect("switchBin");
        }

        private void HandleTrashCollected(bool wasCorrect, string curretnTag)
        {
            if (_levelController == null) return;
            collectedCount++;
            score += wasCorrect ? 2 : 1;
            _gameUI.UpdateScore(score);
            if (wasCorrect)
                SoundManager.Instance.PlayEffect("rightBin", 0.2f);
            else
                SoundManager.Instance.PlayEffect("wrongBin", 0.5f);
            
            _levelController.CheckWinCondition();
            
        }

        private void HandleTrashMissed()
        {
            if (_levelController == null) return;
            _gameUI.UpdateLives(currentLives);
            currentLives--;
            SoundManager.Instance.PlayEffect("wrongCatch", 0.7f);
            _levelController.CheckWinCondition();

        }

        public IEnumerator FinishGame()
        {
            isGameRunning = false;
            Time.timeScale = 0f;
            yield return StartCoroutine(_gameUI.ShowInfo());
            Time.timeScale = 1f;
        }
    }
}