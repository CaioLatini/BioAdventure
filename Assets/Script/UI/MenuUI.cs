using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;

// MainMenuUI.cs (Antigo MainMenu)
/*
Esse script será responsável por controlar a lógica da interface do menu principal.
Ele exibirá a pontuação máxima do nível selecionado, atualizará a imagem de fundo
e gerenciará a interação com os botões de seleção de nível.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class MenuUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private TMP_Text bestScoreText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private List<Image> starImage;
        [SerializeField] private List<Sprite> winBackgrounds;
        [SerializeField] private List<Sprite> loseBackgrounds;
        [SerializeField] private List<Sprite> starSprites;


        private int _lastCheckedLevel = -1;

        private void Update()
        {
            UpdateUIForCurrentLevel();
        }

        private void UpdateUIForCurrentLevel()
        {
            if (GameManager.Instance == null) return;

            int currentLevel = GameManager.Instance.CurrentLevel;
            _lastCheckedLevel = currentLevel;

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
            if (bestScoreText != null)
            {
                bestScoreText.text = score;
            }
        }

        public void UpdateBestPerformace(int performace)
        {
            for(int i = 0; i < starImage.Count; i++)
            {
                if (performace == 4)
                {
                    starImage[i].sprite = starSprites[2];
                    starImage[i].color = Color.white;
                }
                else
                {
                    starImage[i].sprite = (i < performace) ? starSprites[1] : starSprites[0];
                    starImage[i].color = (i < performace) ? Color.white : Color.black;  
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
            SoundManager.Instance.PlayEffect(goToNext ? "go" : "back");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeLevel(goToNext);
            }
        }

        public void OnPlayButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            SceneController.Instance.StartGame();
        }

        public void OnLogoutButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");
            SceneController.Instance.GoToAuthScene();
        }

        public void OnHomeButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            SceneController.Instance.GoToMainMenu();
        }

        #endregion
    }
}