using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;

// GameUI.cs
/*
Atualiza a interface do usuário (HUD) durante o gameplay.
Exibe informações como pontuação atual, vidas restantes, contagem regressiva 
e curiosidades (info panel) no final ou durante pausas.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Referências da UI - HUD")]
        [Tooltip("Texto que exibe a pontuação atual.")]
        [SerializeField] private TMP_Text scoreText;
        
        [Tooltip("Lista de ícones que representam as vidas.")]
        [SerializeField] private List<GameObject> livesDisplay;

        [Header("Referências da UI - Countdown")]
        [Tooltip("Painel de contagem regressiva antes da fase começar.")]
        [SerializeField] private GameObject countdownPanel;
        [SerializeField] private TMP_Text countdownText;

        [Header("Referências da UI - Info Panel")]
        [Tooltip("Painel que exibe uma curiosidade (InfoString) após a fase.")]
        [SerializeField] private GameObject infoDisplay;
        [SerializeField] private TMP_Text infoText;

        private TextData textData = new TextData();
        private bool _isWaitingForOk = false;

        private void Start()
        {
            if (infoDisplay != null) infoDisplay.SetActive(false);
            textData.StartInfo();
        }

        public void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }

        public void UpdateLives(int currentLives)
        {
            if (livesDisplay != null && currentLives >= 0 && currentLives < livesDisplay.Count)
            {
                livesDisplay[currentLives].SetActive(false);
            }
        }

        public void ShowCountdown(string countdownValue)
        {
            if (countdownPanel != null && !countdownPanel.activeSelf) countdownPanel.SetActive(true);
            if (countdownText != null) countdownText.text = countdownValue;
        }
    
        public void HideCountdown()
        {
            if (countdownPanel != null) countdownPanel.SetActive(false);
        }

        // Exibe o painel de curiosidade e pausa a coroutine até o jogador clicar em OK
        public IEnumerator ShowInfo()
        {
            if (textData.infoString != null && textData.infoString.Count > 0 && infoDisplay != null && infoText != null)
            {
                Debug.Log("Chamado de informativo");

                infoDisplay.SetActive(true);
                infoText.text = textData.infoString[Random.Range(0, textData.infoString.Count)];

                Debug.Log("Informativo definido: "+ infoText.text);
                _isWaitingForOk = true;
                yield return new WaitUntil(() => !_isWaitingForOk);
                
                Debug.Log("Ocultando informativo");
                infoDisplay.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Falha com informativo");
                yield return new WaitForSeconds(1f);
            }
        }

        // Vincular esta função ao evento OnClick do botão "OK" no painel de Info
        public void OnInfoOkButtonPressed() 
        { 
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            _isWaitingForOk = false; 
        }
    }
}