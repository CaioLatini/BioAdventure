using System;
using UnityEngine;
using UnityEngine.EventSystems; // Essencial para UI

namespace BioAdventure.Assets.Script.Core
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
      
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            Debug.Log("InputManager Initialized");
        }

        public event Action<float> onMove;
        public event Action onBoost;
        public event Action onPause;
        public event Action onSubmit;
        public event Action<Vector2> onRadialMenuOpen;
        public event Action<Vector2> onRadialMenuSelect;

        public bool IsMenuOpen = false;

        void Update()
        {
            #if UNITY_ANDROID
                return;
            #else
            // --- 1. INPUTS DE CLIQUE ÚNICO ---
            if (Input.GetKeyDown(KeyCode.Space)) onBoost?.Invoke();
            if (Input.GetKeyDown(KeyCode.Escape)) onPause?.Invoke();
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) onSubmit?.Invoke();

            // --- 2. LÓGICA DE MOVIMENTO (Prioridade: Teclado -> Acelerômetro) ---
            float moveInput = 0f;

            // Verifica Teclado
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
            }

            // Garante limites entre -1 e 1
            moveInput = Mathf.Clamp(moveInput, -1f, 1f);

            // DISPARA O EVENTO DE MOVIMENTO (Apenas uma vez por frame, com o valor final)
            onMove?.Invoke(moveInput);


            // --- 3. MENU RADIAL E INTERAÇÃO DE MOUSE/TOUCH ---
            
            // Abrir Menu (Protegido pela UI)
            if (Input.GetMouseButtonDown(0))
            {
                // Só abre se NÃO estiver clicando em um botão/UI
                if (!IsPointerOverUI())
                {
                    onRadialMenuOpen?.Invoke(Input.mousePosition);
                }
            }

            // Fechar Menu/Selecionar
            if (Input.GetMouseButtonUp(0))
            {
                onRadialMenuSelect?.Invoke(Input.mousePosition);
            }
            #endif
        }

        public void TriggerPause() 
        {
            onPause?.Invoke(); 
            Debug.Log("Pause Triggered");
        }
        public void TriggerSubmit() 
        {
            onSubmit?.Invoke(); 
            Debug.Log("Subimit Triggered");
        }

        private bool IsPointerOverUI()
        {
            // 1. Verifica Mouse (PC / WebGL / Chromebook)
            // Se o cursor do mouse estiver sobre a UI, retorna true.
            if (EventSystem.current.IsPointerOverGameObject()) 
                return true;

            // 2. Verifica Toque (Mobile)
            // Só tenta pegar o toque se houver pelo menos um toque na tela.
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                // Para toque, precisamos passar o ID do dedo
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return true;
            }

            return false;
        } 
    }
}