using System;

// InputManager.cs
/*
Esse script será responsável por capturar e centralizar todos os inputs do jogador.
Ele detectará as teclas de movimento, ações, pausa e a seleção das lixeiras.
*/



public class InputManager : IInputManager 
{
    event Action<float> onMove;
    event Action onBoost;
    event Action<char> onBinChange;
    event Action onPause;
    event Action onSubmit;
}