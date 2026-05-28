using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.Core;
using System;


namespace BioAdventure.Assets.Script.Gameplay
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class BinController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Referências")]
        [Tooltip("Referência ao gerenciador que controla os Spots fixos e temporários.")]
        [SerializeField] private BinAssistence _assistence;
        [SerializeField] private TutorialController _tutorialController;
        [SerializeField] private Animator _captureAnimation;

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

        private void OnEnable()
        {
            TrashItem.OnCollected += TrashCollected;
        }

        private void OnDisable()
        {
            TrashItem.OnCollected -= TrashCollected;
        }

        private void Start()
        {
            _originalColor = _image.color;
            _dragColor = _originalColor;
            _dragColor.a = 0.5f; // Fica semi-transparente durante o arraste
        }

        private void TrashCollected(bool wasCorrect, string currentTag)
        { 
            if (wasCorrect && gameObject.tag == currentTag)
            {
                _captureAnimation.Play("FireWork" + currentTag, -1, 0f);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click", 0.8f);

            transform.SetAsLastSibling(); // Traz a lixeira para o primeiro plano visual

            _image.raycastTarget = false; // Desliga a colisão de UI para o raio atingir os Spots que estão por trás
            GetComponent<Collider2D>().enabled = false;
            _image.color = _dragColor;

            if (_assistence != null) _assistence.SetTransformTempSpot(_rectTransform);

            if (GameManager.Instance != null && !GameManager.Instance.CurrentUser.TutMoveComplete) _tutorialController.TutorialStep("Drag1");

        }

        private bool _onTut = false;
        public void OnDrag(PointerEventData eventData)
        {
            _assistence.DesableRayCast(gameObject);
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

            if (_assistence != null) _assistence.SetIndicator(hitObject);
            Debug.Log("Altura atual: " + _rectTransform.offsetMax.y);
            if (GameManager.Instance != null && !GameManager.Instance.CurrentUser.TutMoveComplete && _rectTransform.offsetMax.y > 400f && !_onTut)
            {
                _tutorialController.TutorialStep("Drag2");
                _onTut = true;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _assistence.EnableRayCast();
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

            int newIndex = -1;
            if (hitObject != null && _assistence != null && _assistence.SpotDetected != null)
            {
                newIndex = System.Array.IndexOf(_assistence.SpotDetected, hitObject);
            }

            if (_assistence != null)
            {
                _assistence.RefreshListFixedBin(_rectTransform, newIndex);
                _assistence.SetIndicator(null); // Apaga os indicadores de brilho
            }

            _image.color = _originalColor;
            _image.raycastTarget = true; // Volta a permitir o clique na lixeira
            GetComponent<Collider2D>().enabled = true;

            if (GameManager.Instance != null && !GameManager.Instance.CurrentUser.TutMoveComplete) _tutorialController.FinalizarTutorial(1);

            if (GameManager.Instance != null && !GameManager.Instance.CurrentUser.TutCaptureComplete)
            {
                for (int i = 0; i < _assistence.Spot.Length; i++)
                {
                    if (Vector2.Distance(transform.position, _assistence.Spot[i].position) < 0.1f)
                    {
                        _tutorialController.CheckTutCapture(i, gameObject.name);
                    }
                }
            }

        }
    }
}