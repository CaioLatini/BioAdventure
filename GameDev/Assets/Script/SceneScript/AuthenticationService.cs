using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

namespace BioAdventure.Assets.Script.SceneScript
{
    public class AuthenticationService : MonoBehaviour
    {
        [Header("Dependences")]
        [SerializeField] private BtnController _btnController;
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private TMP_InputField _directoryInput;

        private string _saveFilePath;

        private void Start()
        {
            string savedDirectoryFile = Path.Combine(Application.persistentDataPath, "directorySave.txt");

            if (File.Exists(savedDirectoryFile))
            {
                string savedDirectory = File.ReadAllText(savedDirectoryFile);
                SetDirectory(savedDirectory);
            }
            else
            {
                SetDirectory(Application.persistentDataPath);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                _btnController.Auth();
        }

        public void SetDirectory(string directorySave)
        {
            var directory = _directoryInput.text;
            if (Directory.Exists(directory))
                _saveFilePath = directory;
            else if (Directory.Exists(directorySave))
                _saveFilePath = directorySave;
            else
                _saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

            Debug.Log(_saveFilePath);
        }

        public (bool success, UserSaveData user) Authenticate()
        {
            var name = _usernameInput.text.Trim();

            if (name.Length < 3)
            {
                _statusText.text = "Username must have at least 3 characters";
                return (false, null);
            }

            SetDirectory(Application.persistentDataPath);
            string pathToSaveFile = Path.Combine(Application.persistentDataPath, "directorySave.txt");
            File.WriteAllText(pathToSaveFile, _saveFilePath);
            Debug.Log("Directory write");

            var users = LoadAllUsers(_saveFilePath);
            var existing = users.Find(u => u.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                _statusText.text = "User found";
                return (true, existing);
            }

            var newUser = new UserSaveData
            {
                UserName = name,
                Directory = _saveFilePath,
                Effects = 1f,
                Music = 1f
            };

            users.Add(newUser);
            _statusText.text = "Create user";
            SaveAllUsers(users, _saveFilePath);
            return (true, newUser);
        }

        // -----------------------------
        // MÉTODOS ESTÁTICOS PARA USO GLOBAL
        // -----------------------------

        public static void SaveUser(UserSaveData updatedUser)
        {
            var users = LoadAllUsers(updatedUser.Directory);

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].UserName.Equals(updatedUser.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    users[i] = updatedUser;
                    break;
                }
            }

            SaveAllUsers(users, updatedUser.Directory);
        }

        private static List<UserSaveData> LoadAllUsers(string directory)
        {
            var saveFile = Path.Combine(directory, "saveData.json");
            if (!File.Exists(saveFile)) return new List<UserSaveData>();

            var json = File.ReadAllText(saveFile);
            var wrapper = JsonUtility.FromJson<UserListWrapper>(json);
            return wrapper?.Users ?? new List<UserSaveData>();
        }

        private static void SaveAllUsers(List<UserSaveData> users, string directory)
        {
            var saveFile = Path.Combine(directory, "saveData.json");
            var wrapper = new UserListWrapper { Users = users };
            var json = JsonUtility.ToJson(wrapper, true);
            Directory.CreateDirectory(Path.GetDirectoryName(saveFile));
            File.WriteAllText(saveFile, json);
        }
    }

    [Serializable]
    public class UserListWrapper
    {
        public List<UserSaveData> Users;
    }
}
