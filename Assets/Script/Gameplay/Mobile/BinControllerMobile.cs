using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BioAdventure.Assets.Script.Gameplay;
using System;

namespace BioAdventure.Assets.Script.Gameplay.Mobile
{
    public class UIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private BinAssistenceMobile assistenceMobile;
        private RectTransform rectTransform;
        private Canvas canvas;
        private Image image;

        private Color dragColor;
        private Color originalColor;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            image = GetComponent<Image>();
        }

        void Start()
        {
            originalColor = image.color;
            dragColor = originalColor;
            dragColor.a = 0.5f;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
            image.raycastTarget = false;
            image.color = dragColor;

            assistenceMobile.SetTransformTempSpot(rectTransform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            assistenceMobile.SetIndicator(hitObject);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

            // Localiza o índice do SpotDetected atingido
            int newIndex = -1;
            if (hitObject != null)
            {
                newIndex = Array.IndexOf(assistenceMobile.SpotDetected, hitObject);
            }

            // Envia o RectTransform desta lixeira para o controller reordenar
            assistenceMobile.RefreshListFixedBin(rectTransform, newIndex);
            assistenceMobile.SetIndicator(null);

            image.color = originalColor;
            image.raycastTarget = true;
        }
    }
}