using System;
using UnityEngine;

// InputManager.cs
/*
Esse script será responsável por capturar e centralizar todos os inputs do jogador.
Ele detectará as teclas de movimento, ações, pausa e a seleção das lixeiras.
*/


namespace BioAdventure.Assets.Script.Core
{
    public class InputManager : MonoBehaviour, IInputManager
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
        }

        public event Action<float> onMove;
        public event Action onBoost;
        public event Action<char> onBinChange;
        public event Action onPause;
        public event Action onSubmit;

        // Futura melhoria: permitir configuração personalizada das teclas
        private readonly string _binChangeKeys = "qwerasdf";

        private List<var> ouvintesOnMove;

        awake()
        {
            ouvintes = new List<var>(BinController.HandleMovement, etc);
        }

        void Update()
        {
            float moveInput = 0f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveInput = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = -1f;
            }
            onMove?.Invoke(moveInput); // se houver algum inscrito, invoca o evento com a informação do moveInput

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

            // O 'Input.inputString' é uma forma eficiente de capturar qualquer tecla pressionada
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                // Pega o primeiro caractere pressionado neste frame
                char keyPressed = Input.inputString[0];

                if (_binChangeKeys.Contains(keyPressed.ToString()))
                {
                    onBinChange?.Invoke(keyPressed);
                }
            }
        }
    }
}