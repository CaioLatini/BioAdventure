using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BioAdventure.Assets.Script.Managers;

// BinControllerMobile.cs
/*
Controla individualmente cada lixeira na versão Mobile.
Permite que o jogador toque e arraste o elemento da interface (UI).
Comunica-se com o BinAssistenceMobile para validar as posições de largada.
*/

namespace BioAdventure.Assets.Script.Gameplay.Mobile
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class BinControllerMobile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Referências")]
        [Tooltip("Referência ao gerenciador que controla os Spots fixos e temporários.")]
        [SerializeField] private BinAssistenceMobile _assistenceMobile;
        
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Image _image;

        private Color _dragColor;
        private Color _originalColor;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _originalColor = _image.color;
            _dragColor = _originalColor;
            _dragColor.a = 0.5f; // Fica semi-transparente durante o arraste
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Impede a interação se o jogo estiver em contagem decrescente, pausa ou terminado
            if (GameControllerMobile.Instance != null && !GameControllerMobile.Instance.isGameRunning) return;

            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click", 0.8f);

            transform.SetAsLastSibling(); // Traz a lixeira para o primeiro plano visual
            
            _image.raycastTarget = false; // Desliga a colisão de UI para o raio atingir os Spots que estão por trás
            GetComponent<Collider2D>().enabled = false;
            _image.color = _dragColor;

            if (_assistenceMobile != null) _assistenceMobile.SetTransformTempSpot(_rectTransform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (GameControllerMobile.Instance != null && !GameControllerMobile.Instance.isGameRunning) return;

            // Move a lixeira acompanhando o dedo do jogador (ajustado pela escala do Canvas)
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            
            // Lança um raio do dedo para descobrir se está por cima de algum SpotDetector
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            
            if (_assistenceMobile != null) _assistenceMobile.SetIndicator(hitObject);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

            int newIndex = -1;
            if (hitObject != null && _assistenceMobile != null && _assistenceMobile.SpotDetected != null)
            {
                newIndex = System.Array.IndexOf(_assistenceMobile.SpotDetected, hitObject);
            }

            if (_assistenceMobile != null) 
            {
                _assistenceMobile.RefreshListFixedBin(_rectTransform, newIndex);
                _assistenceMobile.SetIndicator(null); // Apaga os indicadores de brilho
            }

            _image.color = _originalColor;
            _image.raycastTarget = true; // Volta a permitir o clique na lixeira
            GetComponent<Collider2D>().enabled = true;
        }
    }
}