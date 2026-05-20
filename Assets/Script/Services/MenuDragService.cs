using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using BioAdventure.Assets.Script.UI;
using BioAdventure.Assets.Script.Core;

namespace BioAdventure.Assets.Script.Services
{
    public class MenuDragService : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _swipePanel;
        [SerializeField] private MenuUI _menuUI;
        [Tooltip("Imagem que fica por trás do Swipe Panel")]
        [SerializeField] private Image _previewBackground; 
        [SerializeField] private float _swipeThreshold = 0.2f;
        [SerializeField] private float _snapSpeed = 15f;

        private Vector2 _originalPos;
        private Vector2 _startDragPos;
        private Coroutine _snapCoroutine;
        private bool _previewSet;

        private void Start()
        {
            _originalPos = _swipePanel.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_snapCoroutine != null) StopCoroutine(_snapCoroutine);
            _startDragPos = eventData.position;
            _previewSet = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float distanceX = eventData.position.x - _startDragPos.x;
            _swipePanel.anchoredPosition = new Vector2(_originalPos.x + distanceX, _originalPos.y);

            // Define a imagem de fundo assim que a direção for identificada
            if (!_previewSet && Mathf.Abs(distanceX) > 10f)
            {
                int targetLevel = GameManager.Instance.CurrentLevel + (distanceX < 0 ? 1 : -1);
                Sprite nextBg = _menuUI.GetBackgroundForLevel(targetLevel);
                
                if (nextBg != null)
                {
                    _previewBackground.sprite = nextBg;
                    _previewBackground.color = Color.white;
                }
                else
                {
                    _previewBackground.color = Color.black; // Cor caso não tenha nível antes/depois
                }
                _previewSet = true;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float distanceX = eventData.position.x - _startDragPos.x;

            if (Mathf.Abs(distanceX) > Screen.width * _swipeThreshold)
            {
                bool goToNext = distanceX < 0;
                _menuUI.OnChangeLevelButtonPressed(goToNext);
            }

            _snapCoroutine = StartCoroutine(SnapBack());
        }

        private IEnumerator SnapBack()
        {
            while (Vector2.Distance(_swipePanel.anchoredPosition, _originalPos) > 0.5f)
            {
                _swipePanel.anchoredPosition = Vector2.Lerp(_swipePanel.anchoredPosition, _originalPos, Time.deltaTime * _snapSpeed);
                yield return null;
            }
            _swipePanel.anchoredPosition = _originalPos;
        }
    }
}