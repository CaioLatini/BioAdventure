using UnityEngine;
using TMPro;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Data;
using System.Collections;

// Game.cs
/*
Esse script será responsável por atualizar a interface do usuário (HUD) durante o gameplay.
Sua função é exibir informações como a pontuação atual e o número de vidas restantes.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private GameObject countdownPanel;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private List<GameObject> livesDisplay;
        [SerializeField] private GameObject infoDisplay;
        [SerializeField] private TMP_Text infoText;

        private TextData textData = new TextData();
        private bool _isWaitingForOk = false;

        private void OnEnable()
        {
            InputManager.Instance.onSubmit += OnOkPressed;
        }

        private void OnDisable()
        {
            InputManager.Instance.onSubmit -= OnOkPressed;
        }
        private void Start()
        {
            infoDisplay.SetActive(false);
            textData.StartInfo();
        }
        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateLives(int currentLives)
        {
            livesDisplay[currentLives].SetActive(false);
        }

        public void ShowCountdown(string countdownValue)
        {
            countdownText.text = countdownValue;
        }
    
        public void HideCountdown()
        {
            countdownPanel.SetActive(false);
        }

        public IEnumerator ShowInfo()
        {
            if (textData.infoString.Count >= 0 && infoDisplay!=null)
            {
                infoDisplay.SetActive(true);
                infoText.text = textData.infoString[Random.Range(0, textData.infoString.Count)];
                //precisa de ajuste aqui
                _isWaitingForOk = false;
                yield return new WaitUntil(() => _isWaitingForOk);
            }
            yield return null;
        }

        private void OnOkPressed() { _isWaitingForOk = true; }
    }
}