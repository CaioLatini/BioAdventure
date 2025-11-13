using UnityEngine;
using System.Collections;
using TMPro;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Services;
using BioAdventure.Assets.Script.Managers;
using System.Linq.Expressions;
using NUnit.Framework;

// AuthUI.cs
/*
Esse script será responsável por gerenciar a interface da cena de autenticação.
Ele capturará os dados dos InputFields, exibirá mensagens de status ao usuário
e chamará os sistemas de autenticação e salvamento quando o botão de login for pressionado.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class AuthUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private UnityEngine.UI.Button loginButton;

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
            SoundManager.Instance.PlayMusic("musicMenu");
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.onSubmit += OnLoginButtonPressed;
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.onSubmit -= OnLoginButtonPressed;
            }
        }

        public void ShowStatusMessage(string message, bool isSuccess)
        {
            if (statusText != null)
            {
                if (isSuccess)
                { 
                    statusText.color = Color.green;
                }
                else statusText.color = Color.red;

                statusText.text = message;
            }
        }

        public void ClearUsernameInput()
        {
            if (usernameInput != null)
            {
                usernameInput.text = "";
            }
        }

        private System.Collections.IEnumerator GoToMainMenuAfterDelay(float delay, bool isNew)
        {
            yield return new WaitForSeconds(delay);
            if(isNew)
            {
                SceneController.Instance.StartTuturial();
            } else SceneController.Instance.GoToMainMenu();
        }

        public void OnLoginButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            loginButton.interactable = false;

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
                loginButton.interactable = true;
                ClearUsernameInput();
            }
        }
    }
}