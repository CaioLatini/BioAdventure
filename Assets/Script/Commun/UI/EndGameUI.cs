using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Managers;

// EndGameUI.cs
/*
Gerencia a tela de fim de jogo (Vitória ou Derrota).
Exibe a pontuação final, animação de estrelas (performance), 
atualiza o background e libera ou bloqueia o botão de próxima fase.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class EndGameUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [Tooltip("Botão para avançar para o próximo nível (oculto em caso de derrota).")]
        [SerializeField] private GameObject _nextLevelButton;
        
        [Tooltip("Texto que exibe a pontuação final da partida.")]
        [SerializeField] private TMP_Text _scoreText;

        [Header("Background")]
        [Tooltip("Imagem de fundo da tela de EndGame.")]
        [SerializeField] private Image _backgroundImage;
        
        [Tooltip("Lista de fundos para quando o jogador vence (índice = nível).")]
        [SerializeField] private List<Sprite> _winBackgrounds;
        
        [Tooltip("Lista de fundos para quando o jogador perde (índice = nível).")]
        [SerializeField] private List<Sprite> _loseBackgrounds;

        [Header("Painel de Fim de Jogo")]
        [Tooltip("Imagem central do painel (ex: prancheta ou quadro).")]
        [SerializeField] private Image _endGamePanel;
        
        [Tooltip("Sprites do painel: 0 = Derrota, 1 = Vitória.")]
        [SerializeField] private List<Sprite> _endGamePanelSprite;

        [Header("Performance")]
        [Tooltip("Lista das imagens das estrelas na UI.")]
        [SerializeField] private List<Image> _starImage;
        
        [Tooltip("Sprites das estrelas: 0 = Vazia, 1 = Preenchida, 2 = Dourada (Perfect).")]
        [SerializeField] private List<Sprite> _starSprites;

        private void Start()
        {
            if (GameManager.Instance == null) return;

            // Salva o progresso e as conquistas recém-obtidas
            GameManager.Instance.SaveProgress();

            UpdateScore();
            StartCoroutine(UpdatePerformace(GameManager.Instance.CurrentPerformace, GameManager.Instance.HasWon));

            UpdateBackground(GameManager.Instance.CurrentLevel, GameManager.Instance.HasWon);
            UpdateImagePanel(GameManager.Instance.HasWon);
            UpdateEnableNextLevel(GameManager.Instance.HasWon);
        }

        public void UpdateScore()
        {
            if (_scoreText != null && GameManager.Instance != null)
            {
                _scoreText.text = GameManager.Instance.CurrentScore.ToString();
            }
        }

        // Coroutine para animar o preenchimento das estrelas uma a uma
        public IEnumerator UpdatePerformace(int performace, bool hasWon)
        {
            if (!hasWon || _starImage == null) yield break;
                
            for (int i = 0; i < _starImage.Count; i++)
            {
                bool scored = i < performace;
                _starImage[i].sprite = _starSprites[1];
                _starImage[i].color = scored ? Color.white : Color.black;

                if (scored)
                {
                    if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("scorePoint");
                    yield return new WaitForSeconds(0.5f);    
                }
            }

            // Animação bônus se conseguiu pontuação perfeita (4 estrelas)
            if (performace == 4)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("perfectScore");
                
                foreach (var star in _starImage)
                {
                    star.sprite = _starSprites[2];
                    star.color = Color.white;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void UpdateBackground(int currentLevel, bool hasWon)
        {
            if (_backgroundImage == null) return;

            List<Sprite> targetList = hasWon ? _winBackgrounds : _loseBackgrounds;
            
            if (currentLevel < targetList.Count)
            {
                _backgroundImage.sprite = targetList[currentLevel];
            }
        }

        public void UpdateImagePanel(bool hasWon)
        {
            if (_endGamePanel != null && _endGamePanelSprite.Count >= 2)
            {
                _endGamePanel.sprite = hasWon ? _endGamePanelSprite[1] : _endGamePanelSprite[0];
            }
        }

        public void UpdateEnableNextLevel(bool hasWon)
        {
            if (_nextLevelButton != null)
            {
                _nextLevelButton.SetActive(hasWon);
            }
        }

        #region Button Events

        public void OnReplayButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.StartGame();
        }

        public void OnHomeButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (SceneController.Instance != null) SceneController.Instance.GoToMainMenu();
        }

        public void OnNextLevelButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            if (GameManager.Instance != null) GameManager.Instance.ChangeLevel(true);
            if (SceneController.Instance != null) SceneController.Instance.StartGame();
        }

        #endregion
    }
}