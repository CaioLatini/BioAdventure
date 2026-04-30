using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;

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
        [Header("Referências da UI")]
        [Tooltip("Texto que exibe o recorde do nível selecionado.")]
        [SerializeField] private TMP_Text bestScoreText;
        
        [Tooltip("Imagem de fundo principal do menu.")]
        [SerializeField] private Image backgroundImage;
        
        [Tooltip("Lista das 3 imagens de estrelas para exibir a performance.")]
        [SerializeField] private List<Image> starImage;
        
        [Header("Assets Visuais")]
        [Tooltip("Fundos exibidos quando o nível já foi vencido (índice = nível).")]
        [SerializeField] private List<Sprite> winBackgrounds;
        
        [Tooltip("Fundos exibidos quando o nível não foi vencido ou está bloqueado (índice = nível).")]
        [SerializeField] private List<Sprite> loseBackgrounds;
        
        [Tooltip("Sprites das estrelas: 0=Vazia, 1=Preenchida, 2=Dourada (Perfect).")]
        [SerializeField] private List<Sprite> starSprites;

        private void Start()
        {
            // Atualiza a UI apenas uma vez ao carregar a tela, economizando processamento
            UpdateUIForCurrentLevel();
        }

        // Atualiza as informações visuais da tela com base no nível atual selecionado
        private void UpdateUIForCurrentLevel()
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentUser == null) return;

            int currentLevel = GameManager.Instance.CurrentLevel;
            bool isUnlocked = GameManager.Instance.UnlockedLevel(currentLevel);

            string score = "0";
            int performace = 0;

            if (isUnlocked && currentLevel < GameManager.Instance.CurrentUser.levelScore.Count)
            {
                score = GameManager.Instance.CurrentUser.levelScore[currentLevel].ToString();
                performace = GameManager.Instance.CurrentUser.levelPerformace[currentLevel];
            }

            UpdateBestScore(score);
            UpdateBestPerformace(performace);
            UpdateBackground(currentLevel, isUnlocked);
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

        public void UpdateBackground(int levelIndex, bool isUnlocked)
        {
            if (backgroundImage == null) return;

            if (isUnlocked && levelIndex < winBackgrounds.Count)
            {
                backgroundImage.sprite = winBackgrounds[levelIndex];
            }
            else if (!isUnlocked && levelIndex < loseBackgrounds.Count)
            {
                backgroundImage.sprite = loseBackgrounds[levelIndex];
            }
        }

        #region Button Events

        public void OnChangeLevelButtonPressed(bool goToNext)
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect(goToNext ? "go" : "back");

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

        public void OnLogoutButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.GoToAuthScene();
        }

        public void OnHomeButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.GoToMainMenu();
        }

        #endregion
    }
}