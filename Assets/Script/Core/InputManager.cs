using System;
using UnityEngine;

// InputManager.cs
/*
Esse script será responsável por capturar e centralizar todos os inputs do jogador.
Ele detectará as teclas de movimento, ações, pausa e a seleção das lixeiras.
*/


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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                onBoost?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                onPause?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                onSubmit?.Invoke();
            }

            float moveInput = 0f;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
                Console.WriteLine("Movendo para a direita");
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
                Console.WriteLine("Movendo para a esquerda");
            }
            onMove?.Invoke(moveInput);

            // 1. ABRIR O MENU RADIAL (Ao Pressionar o botão esquerdo)
            if (Input.GetMouseButtonDown(0))
            {
                onRadialMenuOpen?.Invoke(Input.mousePosition);
            }

            // 2. FECHAR O MENU RADIAL (Ao Soltar o botão esquerdo)
            if (Input.GetMouseButtonUp(0))
            {
                onRadialMenuSelect?.Invoke(Input.mousePosition);
            }
        }
        public void TriggerPause()
        {
            onPause?.Invoke();
        }
    }
}