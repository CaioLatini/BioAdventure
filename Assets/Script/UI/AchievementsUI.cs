using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using BioAdventure.Assets.Script.UI;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Core;
using UnityEngine.SceneManagement;

namespace BioAdventure.Assets.Script.UI
{
    public class AchievementUI : MonoBehaviour
    {
        private void Start()
        {
            //Exibe conquistas nas filas (funcao de EndGame)
            if (AchievementsManager.Instance.AchievementsToDisplay.Count > 0)
            {
                achievementQueue = AchievementsManager.Instance.AchievementsToDisplay;
                StartCoroutine(ShowPopupRoutine());
            }

            //Abre Menu de conquistas (variaveis de Menu)
            if (GameManager.Instance != null && GameManager.Instance.ShowAchievementsOnMenuLoad)
            {
                GameManager.Instance.ShowAchievementsOnMenuLoad = false;
                _wasShowAchievements = true;

                SetUnlockedAchievements();
                pauseUI.Toggle();
                _panelAchievements.SetActive(true);
            }
        }
        
        #region EndGame
        [Header("Referências da UI for EndGame")]
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private Image iconImage;
        [SerializeField] private float displayDuration = 3f;

        private Queue<Achievement> achievementQueue = new Queue<Achievement>();
      
        private IEnumerator ShowPopupRoutine()
        {
            while (achievementQueue.Count > 0)
            {
                Achievement achievement = achievementQueue.Dequeue();

                iconImage.sprite = achievement.icon;
                popupPanel.SetActive(true);
                Debug.Log("SetActive.True");
                yield return new WaitForSecondsRealtime(displayDuration);

                popupPanel.SetActive(false);
                Debug.Log("SetActive.False");
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }

        public void OnButtonAchievementsUnlockedPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            GameManager.Instance.ShowAchievementsOnMenuLoad = true;
            SceneController.Instance.GoToMainMenu();
        }

        #endregion

        #region Menu
        [Header("Referências da UI for Menu")]
        [SerializeField] private PauseUI pauseUI;
        [SerializeField] private GameObject _panelAchievements;
        [Tooltip("Precisa estar na ordem: Defeat - Victory - Perfect - TheEnd - WhatYouDoing? - AbsoluteCinema")]
        [SerializeField] private List<Image> _iconAchievements;
        private bool _wasShowAchievements;


        public void OnButtonAchievementsPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            _wasShowAchievements = !_wasShowAchievements;
            if (_wasShowAchievements) SetUnlockedAchievements();
            _panelAchievements.SetActive(_wasShowAchievements);
        }
        
        private void SetUnlockedAchievements()
        {
            for(int i = 0; _iconAchievements.Count > i; i++)
            {
                AchievementID currentID = (AchievementID)i; // Converte o índice (0, 1, 2...) no Enum (Lose, FirstWin, PerfectWin...)


                Achievement data = AchievementsManager.Instance.GetAchievementData(currentID);//Pega os dados de cada Achievements

                //Captura os texto do Achievements para edicao
                TMP_Text titleText = _iconAchievements[i].GetComponentInChildren<TMP_Text>();
                Debug.Log(titleText.name);

                if (data == null || _iconAchievements[i] == null || titleText == null)
                {
                    Debug.LogWarning($"GameObject da Conquista {currentID} (índice {i}) está mal configurado. Faltam dados, Image ou TMP_Text.");
                    continue;
                }


                if (AchievementsManager.Instance.IsUnlocked(currentID))
                {
                    // Conquista Desbloqueada
                    _iconAchievements[i].color = Color.white; // Cor de "desbloqueado"
                    titleText.text = data.title;   // Define o título real
                }
                else
                {
                    // Conquista Bloqueada (Garantia da confgiuração inicial)
                    _iconAchievements[i].color = Color.black; // Cor escura de "bloqueado"
                    titleText.text = "???"; 
                }
            }
        }
        #endregion
    }
}