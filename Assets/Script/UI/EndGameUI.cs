using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;
using System.Collections;
using Unity.VisualScripting;

// MainMenuUI.cs (Antigo MainMenu)
/*
Esse script será responsável por controlar a lógica da interface do menu principal.
Ele exibirá a pontuação máxima do nível selecionado, atualizará a imagem de fundo
e gerenciará a interação com os botões de seleção de nível.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class EndGameUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private GameObject _nextLevelButton;
        [SerializeField] private TMP_Text _scoreText;

        [Header("Background")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private List<Sprite> _winBackgrounds;
        [SerializeField] private List<Sprite> _loseBackgrounds;

        [Header("Painel de Fim de Jogo")]
        [SerializeField] private Image _endGamePanel;
        [Tooltip("0 - lose --- 1 - win")]
        [SerializeField] private List<Sprite> _endGamePanelSprite;

        [Header("Performace")]
        [SerializeField] private List<Image> _starImage;
        [SerializeField] private List<Sprite> _starSprites;

        void Start()
        {
            //Salva para todos devido aos Achivements
            GameManager.Instance.SaveProgress();

            UpdateScore();

            StartCoroutine(UpdatePerformace(GameManager.Instance.CurrentPerformace, GameManager.Instance.HasWon));

            UpdateBackground(GameManager.Instance.CurrentLevel, GameManager.Instance.HasWon);
            UpdateImagePanel(GameManager.Instance.HasWon);
            UpdateEnableNextLevel(GameManager.Instance.HasWon);
        }

        public void UpdateScore()
        {
            _scoreText.text = GameManager.Instance.CurrentScore.ToString();
        }

        public IEnumerator UpdatePerformace(int performace, bool hasWon)
        {
            if (!hasWon)
                yield break;
                
            for (int i = 0; i < _starImage.Count; i++)
            {
                bool scored = i < performace;
                _starImage[i].sprite = scored ? _starSprites[1] : _starSprites[0];
                _starImage[i].color = scored ? Color.white : Color.black;

                if (scored)
                {
                    SoundManager.Instance.PlayEffect("scorePoint");
                    yield return new WaitForSeconds(0.5f);    
                }
            }
            if (performace == 4)
            {
                SoundManager.Instance.PlayEffect("perfectScore");
                foreach(var star in _starImage)
                {
                    star.sprite = _starSprites[2];
                    star.color = Color.white;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void UpdateBackground(int currentLevel, bool hasWon)
        {
            _backgroundImage.sprite = hasWon ? _winBackgrounds[currentLevel] : _loseBackgrounds[currentLevel];
        }

        public void UpdateImagePanel(bool hasWon)
        {
            _endGamePanel.sprite = hasWon ? _endGamePanelSprite[1] : _endGamePanelSprite[0];
        }

        public void UpdateEnableNextLevel(bool hasWon)
        {
            _nextLevelButton.SetActive(hasWon ? true : false);
        }

        #region Button Events

        public void OnReplayButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            SceneController.Instance.StartGame();
        }

        public void OnHomeButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            SceneController.Instance.GoToMainMenu();
        }

        public void OnNextLevelButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            GameManager.Instance.ChangeLevel(true);

            SceneController.Instance.StartGame();
        }

        #endregion
    }
}