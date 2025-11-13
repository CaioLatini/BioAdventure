// RadialMenuController.cs
using UnityEngine;
using UnityEngine.UI; // Para Button
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Gameplay;
using Unity.VisualScripting;
using System;
using BioAdventure.Assets.Script.Managers;

namespace BioAdventure.Assets.Script.UI // Coloque na pasta UI
{ //Tenho que ler e entender esse script ainda
    public class RadialMenuController : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private GameObject radialMenuContainer;
        [SerializeField] private List<Image> optionButtons;
        [SerializeField] private GameController _gameController;

        [HideInInspector] public bool isMenuOpen = false;
        private readonly string[] _binType = { "paper", "radioactive", "plastic", "organic", "glass", "hospital", "metal", "wood" };

        private void Start()
        {
            isMenuOpen = false;
            radialMenuContainer.SetActive(false);
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                // InputManager.Instance.onRadialMenuToggle += HandleToggleInput; // <<< REMOVA
                InputManager.Instance.onRadialMenuOpen += HandleOpenMenu; // <<< ADICIONE
                InputManager.Instance.onRadialMenuSelect += HandleSelectOption; // <<< ADICIONE
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                // InputManager.Instance.onRadialMenuToggle -= HandleToggleInput; // <<< REMOVA
                InputManager.Instance.onRadialMenuOpen -= HandleOpenMenu; // <<< ADICIONE
                InputManager.Instance.onRadialMenuSelect -= HandleSelectOption; // <<< ADICIONE
            }
        }

        // RadialMenuController.cs
        public void HandleOpenMenu(Vector2 mousePosition)
        {
            // Se o jogo está em PAUSA TOTAL (Time.timeScale == 0f) ou o menu JÁ ESTÁ ABERTO,
            // ignore este evento de "abrir".
            if (Time.timeScale == 0f || isMenuOpen)
            {
                return;
            }

            // Lógica de "MouseDown" (Abrir o menu)
            isMenuOpen = true;
            Time.timeScale = 0.5f;
            SoundManager.Instance.PlayEffect("slowSound", 0.6f);
            radialMenuContainer.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            radialMenuContainer.SetActive(isMenuOpen);
        }

        private void HandleSelectOption(Vector2 mousePosition)
        {
            // Se o menu NÃO ESTIVER ABERTO, ignore este evento de "selecionar" (MouseUp).
            // Isso impede que um "MouseUp" solto (de quando você clicou no menu de pause)
            // seja processado.
            if (!isMenuOpen)
            {
                return;
            }

            // Lógica de "MouseUp" (Fechar o menu e selecionar)
            isMenuOpen = false;
            Time.timeScale = 1f;
            SoundManager.Instance.RestartEffects();
            _gameController.HandleBinChange(HandleSelection(mousePosition));
            radialMenuContainer.SetActive(isMenuOpen);
        }

        private string HandleSelection(Vector2 mousePos)
        {
            float materialSesion = 45f;

            float angleSelectec = GetAngleToTarget(radialMenuContainer.transform.position, mousePos);

            //escolhe retorno com base no angulo
            for (int a = 0; a < optionButtons.Count; a++)
            {
                if (angleSelectec >= materialSesion * a && angleSelectec < materialSesion * (a + 1))
                {
                    Debug.Log("Selecionado: " + _binType[a]);
                    return _binType[a];
                }
            }
            return " ";
        }

        public float GetAngleToTarget(Vector3 origin, Vector2 target)
        {
            // Direção real para o alvo
            Vector3 direction = new Vector3(target.x, target.y, 0) - origin;

            float angleRad = Mathf.Atan2(direction.x, direction.y);

            // 3. Converter para Graus
            float angleDegrees = angleRad * Mathf.Rad2Deg;

            // 4. Corrigir para 0 a 360 (A Unity retorna de -180 a 180 neste caso)
            if (angleDegrees < 0)
            {
                angleDegrees += 360f;
            }

            return angleDegrees;
        }

    }
}