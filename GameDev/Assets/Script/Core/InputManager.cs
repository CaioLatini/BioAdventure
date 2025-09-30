using System;
using UnityEngine;

// InputManager.cs
/*
Como um Singeton esse script será responsável por capturar e centralizar todos os inputs do jogador.
Ele detectará as teclas de movimento, ações, pausa e a seleção das lixeiras.
*/


namespace BioAdventure.Assets.Script.Core
{
    public class InputManager : MonoBehaviour, IInputManager
    {

        public static InputManager inputManager { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }









        event Action<float> onMove;
        event Action onBoost;
        event Action<char> onBinChange;
        event Action onPause;
        event Action onSubmit;
    }
}
