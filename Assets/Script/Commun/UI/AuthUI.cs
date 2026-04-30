using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Services;
using BioAdventure.Assets.Script.Managers;

// AuthUI.cs
/*
Gerencia a interface da cena de autenticação.
Captura dados do InputField, exibe status e coordena com o AuthService e SaveManager.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class AuthUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [Tooltip("Campo de texto onde o jogador digita o nome.")]
        [SerializeField] private TMP_InputField usernameInput;
        
        [Tooltip("Texto de feedback visual (ex: 'Usuário logado', 'Nome curto').")]
        [SerializeField] private TMP_Text statusText;
        
        [Tooltip("Botão de login. Desativado durante o processamento para evitar duplo clique.")]
        [SerializeField] private Button loginButton;
        
        [Tooltip("Botão para abrir o menu de configurações.")]
        [SerializeField] private GameObject settingsButton;

        private AuthService _authService;

        private void Awake()
        {
            _authService = new AuthService();
        }

        private void Start()
        {
            StartCoroutine(LateStart());
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForSeconds(0.2f);
            if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic("musicMenu");
        }

        // Atualiza o texto de status na tela
        public void ShowStatusMessage(string message, bool isSuccess)
        {
            if (statusText != null)
            {
                statusText.color = isSuccess ? Color.green : Color.red;
                statusText.text = message;
            }
        }

        // Limpa o campo de texto
        public void ClearUsernameInput()
        {
            if (usernameInput != null)
            {
                usernameInput.text = "";
            }
        }

        // Aguarda um tempo antes de trocar de cena
        private IEnumerator GoToMainMenuAfterDelay(float delay, bool isNewUser)
        {
            yield return new WaitForSeconds(delay);
            
            if (isNewUser)
            {
                SceneController.Instance.StartTutorial();
            } 
            else 
            {
                SceneController.Instance.GoToMainMenu();
            }
        }

        // Função chamada pelo botão de Login (via Inspector OnClick)
        public void OnLoginButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");

            if (loginButton != null) loginButton.interactable = false;

            string username = usernameInput.text.Trim();

            var (isSuccess, message, user) = _authService.Authenticate(username);

            ShowStatusMessage(message, isSuccess);

            if (isSuccess)
            {
                GameManager.Instance.StartSession(user);
                AchievementsManager.Instance.LoadAchievements(user);
                StartCoroutine(GoToMainMenuAfterDelay(1.0f, !user.TutorialComplete));
            }
            else
            {
                if (loginButton != null) loginButton.interactable = true;
                ClearUsernameInput();
            }
        }
    }
}