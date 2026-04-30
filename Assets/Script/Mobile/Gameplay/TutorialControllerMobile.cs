using UnityEngine;
using TMPro;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.Core;

// TutorialControllerMobile.cs
/*
Gerencia a exibição dos textos do tutorial na versão Mobile.
Lê as strings carregadas pelo TextData e avança as etapas 
até concluir e redirecionar para o jogo principal.
*/

namespace BioAdventure.Assets.Script.Gameplay.Mobile
{
    public class TutorialControllerMobile : MonoBehaviour
    {
        [Header("Referências da UI")]
        [Tooltip("Texto que exibe as instruções do tutorial.")]
        [SerializeField] private TMP_Text _tutorialText;
        
        private TextData _textData = new TextData();
        private int _currentStep = 0;

        private void Start()
        {
            // Carrega o arquivo TXT com as frases do tutorial
            _textData.StartTutorial();
            ShowCurrentStep();
        }

        // Exibe a frase correspondente ao passo atual
        private void ShowCurrentStep()
        {
            if (_textData.tutorialString != null && _currentStep < _textData.tutorialString.Count)
            {
                if (_tutorialText != null) 
                {
                    _tutorialText.text = _textData.tutorialString[_currentStep];
                }
            }
            else
            {
                FinishTutorial();
            }
        }

        // Vincular ao evento OnClick do botão "Próximo" ou toque na tela
        public void OnNextStepPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            
            _currentStep++;
            ShowCurrentStep();
        }

        // Marca o tutorial como completo no save e inicia a gameplay
        private void FinishTutorial()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentUser != null)
            {
                GameManager.Instance.CurrentUser.TutorialComplete = true;
                GameManager.Instance.SaveProgress();
            }

            if (SceneController.Instance != null)
            {
                SceneController.Instance.StartGame();
            }
        }
    }
}