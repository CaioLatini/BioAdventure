using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Core;

// AchievementsUI.cs
/*
Gerencia a interface visual das Conquistas em duas situações diferentes:
1. EndGame: Exibe pop-ups sequenciais das conquistas recém-desbloqueadas.
2. Menu/Pause: Exibe o painel completo listando todas as conquistas (bloqueadas e desbloqueadas).
*/

namespace BioAdventure.Assets.Script.UI
{
    public class AchievementUI : MonoBehaviour
    {
        private void Start()
        {
            // Segurança: Garante que os Managers existam antes de consultá-los
            if (AchievementsManager.Instance == null || GameManager.Instance == null) return;

            // [LÓGICA ENDGAME] Exibe conquistas na fila
            if (AchievementsManager.Instance.AchievementsToDisplay.Count > 0)
            {
                achievementQueue = AchievementsManager.Instance.AchievementsToDisplay;
                StartCoroutine(ShowPopupRoutine());
            }

            // [LÓGICA MENU] Abre o Menu de conquistas automaticamente se a flag estiver ativa
            if (GameManager.Instance.ShowAchievementsOnMenuLoad)
            {
                GameManager.Instance.ShowAchievementsOnMenuLoad = false;
                _wasShowAchievements = true;

                SetUnlockedAchievements();
                
                if (pauseUI != null) pauseUI.TogglePause();
                if (_panelAchievements != null) _panelAchievements.SetActive(true);
            }
        }
        
        #region Lógica de Fim de Jogo (EndGame)
        [Header("Referências da UI - EndGame")]
        [Tooltip("Painel que sobe na tela avisando sobre a nova conquista.")]
        [SerializeField] private GameObject popupPanel;
        [Tooltip("Imagem do ícone da conquista no pop-up.")]
        [SerializeField] private Image iconImage;
        [Tooltip("Tempo que o pop-up fica visível na tela.")]
        [SerializeField] private float displayDuration = 5.5f;

        private Queue<Achievement> achievementQueue = new Queue<Achievement>();
      
        // Coroutine que consome a fila e exibe os pop-ups um por um
        private IEnumerator ShowPopupRoutine()
        {
            if (popupPanel == null || iconImage == null) yield break;

            while (achievementQueue.Count > 0)
            {
                Achievement achievement = achievementQueue.Dequeue();

                iconImage.sprite = achievement.icon;
                popupPanel.SetActive(true);
                
                yield return new WaitForSecondsRealtime(displayDuration);

                popupPanel.SetActive(false);
                yield return new WaitForSecondsRealtime(0.5f); // Pausa breve entre múltiplos pop-ups
            }
        }

        // Botão clicado na tela de EndGame para ir ao menu principal e abrir o painel de conquistas
        public void OnButtonAchievementsUnlockedPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");

            if (GameManager.Instance != null) GameManager.Instance.ShowAchievementsOnMenuLoad = true;
            if (SceneController.Instance != null) SceneController.Instance.GoToMainMenu();
        }
        #endregion

        #region Lógica de Menu e Pause (Menu)
        [Header("Referências da UI - Menu")]
        [Tooltip("Referência ao script PauseUI, usado para intercalar os menus.")]
        [SerializeField] private PauseUI pauseUI;
        
        [Tooltip("O painel principal que contém a lista de conquistas.")]
        [SerializeField] private GameObject _panelAchievements;
        
        [Tooltip("ATENÇÃO: A ordem dos elementos aqui DEVE seguir a ordem do enum AchievementID (Defeat, Victory, Perfect, TheEnd, WhatYouDoing, AbsoluteCinema).")]
        [SerializeField] private List<Image> _iconAchievements;
        
        private bool _wasShowAchievements;

        // Alterna a visibilidade do painel de conquistas
        public void OnButtonAchievementsPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");

            _wasShowAchievements = !_wasShowAchievements;
            
            if (_wasShowAchievements) 
            {
                SetUnlockedAchievements();
            }
            
            if (_panelAchievements != null) 
            {
                _panelAchievements.SetActive(_wasShowAchievements);
            }
        }
        
        // Percorre a lista de imagens da UI e atualiza o visual (cor/título) com base no status de desbloqueio
        private void SetUnlockedAchievements()
        {
            if (_iconAchievements == null || AchievementsManager.Instance == null) return;

            for (int i = 0; i < _iconAchievements.Count; i++)
            {
                // Converte o índice atual para o Enum correspondente
                AchievementID currentID = (AchievementID)i; 
                Achievement data = AchievementsManager.Instance.GetAchievementData(currentID);

                if (_iconAchievements[i] == null) continue;

                TMP_Text titleText = _iconAchievements[i].GetComponentInChildren<TMP_Text>();

                if (data == null || titleText == null)
                {
                    Debug.LogWarning($"[AchievementUI] GameObject da Conquista {currentID} (índice {i}) está mal configurado.");
                    continue;
                }

                if (AchievementsManager.Instance.IsUnlocked(currentID))
                {
                    // Conquista Desbloqueada: Restaura a cor original e mostra o título
                    _iconAchievements[i].color = Color.white; 
                    titleText.text = data.title;   
                }
                else
                {
                    // Conquista Bloqueada: Escurece o ícone e esconde o título
                    _iconAchievements[i].color = Color.black; 
                    titleText.text = "???"; 
                }
            }
        }
        #endregion
    }
}