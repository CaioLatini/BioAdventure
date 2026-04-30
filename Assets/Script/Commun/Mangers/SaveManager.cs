using UnityEngine;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Data;
using System;

// Importa System.IO apenas se NÃO for WebGL para evitar conflitos ou erros de compilação
#if !UNITY_WEBGL
using System.IO;
#endif

// SaveManager.cs
/*
Gerencia a persistência de dados do jogo.
Adapta-se automaticamente à plataforma:
- Em WebGL: Usa PlayerPrefs (seguro para navegadores).
- Em PC/Mobile: Usa arquivos JSON locais (System.IO) no diretório persistente da Unity.
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        // Nome da chave para o PlayerPrefs (funciona como o nome do arquivo no navegador)
        private const string WebGLSaveKey = "BioAdventureSaveData";
        
        [Tooltip("Nome do arquivo JSON que será salvo localmente no PC/Mobile.")]
        [SerializeField] private string _saveFileName = "saveData.json";
        
        private string _saveFilePath;

        // Configura o Singleton
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

        // Define o caminho seguro de salvamento baseado no sistema operacional
        private void Start()
        {
            #if !UNITY_WEBGL
                _saveFilePath = Path.Combine(Application.persistentDataPath, _saveFileName);
                Debug.Log("Caminho do Save: " + _saveFilePath);
            #endif
        }

        // Recupera todos os perfis de usuários salvos no disco ou navegador
        public List<UserSave> LoadAllUsers()
        {
            string json = "";

            #if UNITY_WEBGL
                if (PlayerPrefs.HasKey(WebGLSaveKey))
                {
                    json = PlayerPrefs.GetString(WebGLSaveKey);
                }
                else 
                {
                    return new List<UserSave>(); // Sem save anterior
                }
            #else
                if (!File.Exists(_saveFilePath))
                {
                    return new List<UserSave>(); // Sem save anterior
                }

                try
                {
                    json = File.ReadAllText(_saveFilePath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[SaveManager] Erro ao ler arquivo de save: {e.Message}");
                    return new List<UserSave>();
                }
            #endif

            if (string.IsNullOrEmpty(json)) return new List<UserSave>();

            // Converte o JSON recuperado de volta para a estrutura de dados
            UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(json);
            return wrapper?.Users ?? new List<UserSave>();
        }

        // Sobrescreve o arquivo de save com a lista de usuários atualizada
        public void SaveAllUsers(List<UserSave> users)
        {
            UserListWrapper wrapper = new UserListWrapper { Users = users };
            string json = JsonUtility.ToJson(wrapper, true);

            #if UNITY_WEBGL
                PlayerPrefs.SetString(WebGLSaveKey, json);
                PlayerPrefs.Save(); // Força a gravação imediata
                Debug.Log("[SaveManager] Dados salvos no PlayerPrefs (WebGL).");
            #else
                try
                {
                    File.WriteAllText(_saveFilePath, json);
                    Debug.Log($"[SaveManager] Dados salvos no disco em: {_saveFilePath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[SaveManager] Erro ao gravar arquivo de save: {e.Message}");
                }
            #endif
        }

        // Atualiza o progresso de um usuário específico e aciona o salvamento global
        public void SaveUser(UserSave userToSave)
        {
            if (userToSave == null) return;

            List<UserSave> allUsers = LoadAllUsers();
            
            // Procura se o usuário já existe na lista
            int userIndex = allUsers.FindIndex(u => u.UserName.Equals(userToSave.UserName, StringComparison.OrdinalIgnoreCase));

            if (userIndex != -1)
            {
                allUsers[userIndex] = userToSave; // Atualiza existente
            }
            else
            {
                allUsers.Add(userToSave); // Adiciona novo
            }

            SaveAllUsers(allUsers);
        }

        // Classe auxiliar necessária porque o JsonUtility da Unity não serializa List<T> diretamente no nível raiz
        [Serializable]
        private class UserListWrapper
        {
            public List<UserSave> Users;
        }
    }
}