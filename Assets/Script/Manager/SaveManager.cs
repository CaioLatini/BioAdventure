using UnityEngine;
using System.IO;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Data;

// SaveManager.cs
/*
Esse script será responsável por toda a lógica de manipulação de arquivos.
Sua única função é salvar e carregar os dados do jogo em formato JSON. 
Ele abstrai as operações de Input/Output do resto do sistema.
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

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

            Debug.Log("SaveManager Initialized");
        }

        private string _saveFileName = "saveData.json";
        private string _saveFilePath;

        private void Start()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, _saveFileName);
        }

        public List<UserSave> LoadAllUsers()
        {
            if (!File.Exists(_saveFilePath))
            {
                return new List<UserSave>();
            }

            string json = File.ReadAllText(_saveFilePath);

            UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(json);
            return wrapper?.Users ?? new List<UserSave>();
        }

        public void SaveAllUsers(List<UserSave> users)
        {
            UserListWrapper wrapper = new UserListWrapper { Users = users };

            string json = JsonUtility.ToJson(wrapper, true);

            File.WriteAllText(_saveFilePath, json);
            Debug.Log($"Dados salvos em: {_saveFilePath}");
        }

        public void SaveUser(UserSave userToSave)
        {
            List<UserSave> allUsers = LoadAllUsers();

            int userIndex = allUsers.FindIndex(u => u.UserName.Equals(userToSave.UserName, System.StringComparison.OrdinalIgnoreCase));

            if (userIndex != -1)
            {
                allUsers[userIndex] = userToSave;
            }
            else
            {
                allUsers.Add(userToSave);
            }

            SaveAllUsers(allUsers);
        }

        [System.Serializable]
        private class UserListWrapper
        {
            public List<UserSave> Users;
        }
    }
}