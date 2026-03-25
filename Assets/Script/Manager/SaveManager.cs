using UnityEngine;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Data;

// Importa System.IO apenas se NÃO for WebGL para evitar conflitos ou erros de compilação
#if !UNITY_WEBGL
using System.IO;
#endif

/*
SaveManager.cs (Adaptado para WebGL)
Este script gerencia o salvamento de dados.
- Em WebGL: Usa PlayerPrefs (seguro para navegadores).
- Em Outras Plataformas (Windows, Android, etc.): Usa arquivos JSON no disco (System.IO).
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        // Nome da chave para o PlayerPrefs (funciona como o nome do arquivo no navegador)
        private const string WebGLSaveKey = "BioAdventureSaveData";
        private string _saveFileName = "saveData.json";
        private string _saveFilePath;

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

        private void Start()
        {
            #if !UNITY_WEBGL
                // Só define o caminho do arquivo se NÃO for WebGL
                _saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, _saveFileName);
            #endif
        }

        public List<UserSave> LoadAllUsers()
        {
            string json = "";

            // --- LÓGICA WEBGL (Navegador) ---
            #if UNITY_WEBGL
            
                if (PlayerPrefs.HasKey(WebGLSaveKey))
                {
                    json = PlayerPrefs.GetString(WebGLSaveKey);
                }
                else 
                {
                    // Se não tem save, retorna lista vazia
                    return new List<UserSave>();
                }

            // --- LÓGICA PADRÃO (PC, Android, Editor) ---
            #else
            
                if (!File.Exists(_saveFilePath))
                {
                    return new List<UserSave>();
                }

                json = File.ReadAllText(_saveFilePath);
                
            #endif

            // Converte o JSON recuperado de volta para a lista de usuários
            if (string.IsNullOrEmpty(json)) return new List<UserSave>();

            UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(json);
            return wrapper?.Users ?? new List<UserSave>();
        }

        public void SaveAllUsers(List<UserSave> users)
        {
            UserListWrapper wrapper = new UserListWrapper { Users = users };
            string json = JsonUtility.ToJson(wrapper, true);

            // --- LÓGICA WEBGL (Navegador) ---
            #if UNITY_WEBGL
            
                PlayerPrefs.SetString(WebGLSaveKey, json);
                PlayerPrefs.Save(); // Força a gravação no disco do navegador
                Debug.Log("WebGL: Dados salvos no PlayerPrefs.");

            // --- LÓGICA PADRÃO (PC, Android, Editor) ---
            #else
            
                File.WriteAllText(_saveFilePath, json);
                Debug.Log($"Nativo: Dados salvos no disco em: {_saveFilePath}");
                
            #endif
        }

        // O método SaveUser não precisa mudar, pois ele chama LoadAllUsers e SaveAllUsers,
        // que já tratam as diferenças de plataforma internamente.
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