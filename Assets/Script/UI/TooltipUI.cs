using UnityEngine;
using UnityEngine.EventSystems;
using BioAdventure.Assets.Script.Data;
using TMPro;
using Unity.VisualScripting;
using BioAdventure.Assets.Script.Managers;
// Essencial para detectar o mouse na UI
namespace BioAdventure.Assets.Script.UI
{
    public class HoverTooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text messagePanel;
        [SerializeField] private bool isSecret;
        [SerializeField] private AchievementID achievement;

        private void Start()
        {
            if (messagePanel != null)
            {
                messagePanel.text = "";
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (messagePanel != null)
            {
                if (!isSecret)
                {
                    messagePanel.text = AchievementsManager.Instance.GetAchievementData(achievement).description;
                }
                else if (AchievementsManager.Instance.IsUnlocked(achievement))
                {
                    messagePanel.text = AchievementsManager.Instance.GetAchievementData(achievement).description;
                } else messagePanel.text = "???";
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (messagePanel != null)
            {
                messagePanel.text = "";
            }
        }
    }
}