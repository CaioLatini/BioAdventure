using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;
using System.Collections.Generic;

// AuthService.cs
/*
Esta classe é um serviço de lógica pura, responsável pelo processo de autenticação.
Ela valida os dados de entrada, coordena com o SaveManager para carregar e salvar
perfis de usuário, e contém a regra de negócio para criar um novo usuário.
*/

namespace BioAdventure.Assets.Script.Services
{
    public class AuthService
    {
        private const int MinUsernameLength = 3;
        private const int TotalLevels = 5;

        // Autentica o usuário pelo nome. Verifica se existe; se não, cria um novo.
        public (bool isSuccess, string message, UserSave user) Authenticate(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < MinUsernameLength)
            {
                return (false, $"The username must be at least {MinUsernameLength} characters long.", null);
            }

            List<UserSave> allUsers = SaveManager.Instance.LoadAllUsers();
            UserSave existingUser = allUsers.Find(u => u.UserName.Equals(username, System.StringComparison.OrdinalIgnoreCase));

            if (existingUser != null)
            {
                return (true, "User found!", existingUser);
            }
            else
            {
                UserSave newUser = new UserSave
                {
                    UserName = username,
                    levelScore = new List<int>(new int[TotalLevels]),
                    levelPerformace = new List<int>(new int[TotalLevels])
                };

                allUsers.Add(newUser);
                SaveManager.Instance.SaveAllUsers(allUsers);

                return (true, "New user created successfully!", newUser);
            }
        }

        // Realiza o login silencioso no Mobile utilizando um perfil local padrão.
        public (bool isSuccess, string message, UserSave user) AutoLoginMobile()
        {
            return Authenticate("LocalPlayer");
        }
    }
}