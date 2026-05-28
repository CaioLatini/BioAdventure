using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.Gameplay;
using System.Collections;
using System;

// MenuUI.cs
/*
Controla a interface do menu principal (seleção de níveis).
Exibe a pontuação máxima, estrelas de performance e atualiza o 
fundo com base no status do nível atual.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private TutorialController _tutorialController;

        [Header("Referências da UI")]
        [Tooltip("Texto que exibe o recorde do nível selecionado.")]
        [SerializeField] private TMP_Text bestScoreText;

        [Tooltip("Texto que informa o nível selecionado.")]
        [SerializeField] private TextMeshProUGUI _level;

        [Tooltip("Imagem de fundo principal do menu.")]
        [SerializeField] private Image backgroundImage;

        [Tooltip("Lista das 3 imagens de estrelas para exibir a performance.")]
        [SerializeField] private List<Image> starImage;

        [Tooltip("Sprites das estrelas: 0=Vazia, 1=Preenchida, 2=Dourada (Perfect).")]
        [SerializeField] private List<Sprite> starSprites;

        private void Start()
        {
            // Atualiza a UI apenas uma vez ao carregar a tela, economizando processamento
            StartCoroutine(LateStart());
            UpdateUIForCurrentLevel();
            if (!GameManager.Instance.CurrentUser.TutMenuComplete && GameManager.Instance.CurrentUser.levelScore[0] > 0)
            {
                _tutorialController.TutorialStep("ChangeLevel");
                Debug.Log("Condicional do menu tutorial");
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForSeconds(0.2f);
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicMenu");
        }

        // Atualiza as informações visuais da tela com base no nível atual selecionado
        private void UpdateUIForCurrentLevel()
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentUser == null) return;
            

            int currentLevel = GameManager.Instance.CurrentLevel;
            _level.text = (currentLevel+1).ToString();
            bool isUnlocked = GameManager.Instance.UnlockedLevel(currentLevel);

            Debug.LogWarning("Reload CurrentLevel");
            Debug.LogWarning("Current performace:" + GameManager.Instance.CurrentUser.levelPerformace[currentLevel]);
            if (!GameManager.Instance.CurrentUser.TutMenuComplete && GameManager.Instance.CurrentUser.levelPerformace[currentLevel] == 0 && currentLevel != 0)  
            {
                _tutorialController.FinalizarTutorial(3);
                Debug.LogWarning("Tutorial de menu finalizado");
            }

            string score = "0";
            int performace = 0;


            Debug.Log("isunloked? " + isUnlocked);
            Debug.Log("currentLevel = " + currentLevel);
            Debug.Log("GameManager.Instance.CurrentUser.levelScore.Count = " + GameManager.Instance.CurrentUser.levelScore.Count);

            if (isUnlocked && currentLevel < GameManager.Instance.CurrentUser.levelScore.Count)
            {
                score = GameManager.Instance.CurrentUser.levelScore[currentLevel].ToString();
                performace = GameManager.Instance.CurrentUser.levelPerformace[currentLevel];
                Debug.Log("score" + score);
                Debug.Log("performace" + performace);
            }

            UpdateBestScore(score);
            UpdateBestPerformace(performace);
            backgroundImage.sprite = GetBackgroundForLevel(currentLevel);
        }

        public void UpdateBestScore(string score)
        {
            if (bestScoreText != null) bestScoreText.text = score;
        }

        public void UpdateBestPerformace(int performace)
        {
            if (starImage == null || starSprites == null || starSprites.Count < 3) return;

            for (int i = 0; i < starImage.Count; i++)
            {
                if (starImage[i] == null) continue;

                if (performace == 4)
                {
                    starImage[i].sprite = starSprites[2];
                    starImage[i].color = Color.white;
                }
                else
                {
                    bool earned = i < performace;
                    starImage[i].sprite = starSprites[1];
                    starImage[i].color = earned ? Color.white : Color.black;
                }
            }
        }

        public Sprite GetBackgroundForLevel(int levelIndex)
        {
            if (GameManager.Instance == null || levelIndex < 0 || levelIndex >= GameManager.Instance.levels.Count) return null;

            bool isUnlocked = GameManager.Instance.UnlockedLevel(levelIndex);
            return isUnlocked ? GameManager.Instance.WinBackgrounds[levelIndex] : GameManager.Instance.LoseBackgrounds[levelIndex];
        }

        #region Button Events

        public void OnChangeLevelButtonPressed(bool goToNext)
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("next");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeLevel(goToNext);
                UpdateUIForCurrentLevel(); // Atualiza a interface apenas quando muda de nível
            }
        }

        public void OnPlayButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.StartGame();
        }

        public void OnHomeButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.GoToMainMenu();
        }

        #endregion
    }
}