using UnityEngine;
using UnityEngine.EventSystems;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;
using TMPro;

namespace BioAdventure.Assets.Script.UI
{
    public class HoverTooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Referências da UI")]
        [Tooltip("Painel de texto onde a descrição será exibida.")]
        [SerializeField] private TMP_Text messagePanel;
        
        [Header("Configurações da Conquista")]
        [Tooltip("Se verdadeiro, a descrição só aparece se a conquista já estiver desbloqueada.")]
        [SerializeField] private bool isSecret;
        
        [Tooltip("A conquista associada a este elemento de UI.")]
        [SerializeField] private AchievementID achievement;

        private void Start()
        {
            if (messagePanel != null)
            {
                messagePanel.text = "";
            }
        }

        // Chamado quando o mouse entra na área do elemento
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (messagePanel == null || AchievementsManager.Instance == null) return;

            Achievement data = AchievementsManager.Instance.GetAchievementData(achievement);
            if (data == null) return;

            if (!isSecret)
            {
                messagePanel.text = data.description;
            }
            else if (AchievementsManager.Instance.IsUnlocked(achievement))
            {
                messagePanel.text = data.description;
            } 
            else 
            {
                messagePanel.text = "???";
            }
        }

        // Chamado quando o mouse sai da área do elemento
        public void OnPointerExit(PointerEventData eventData)
        {
            if (messagePanel != null)
            {
                messagePanel.text = "";
            }
        }
    }
}