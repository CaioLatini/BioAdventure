using UnityEngine;
using BioAdventure.Assets.Script.Data;
using System;
using System.IO;

namespace BioAdventure.Assets.Script.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        private string _saveFilePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
                Debug.Log("Caminho do save é: "+_saveFilePath);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public UserSave LoadUser()
        {
            if (!File.Exists(_saveFilePath))
            {
                Debug.LogError("Arquivo de usuário não encontrado");
                return null;
            }

            try
            {
                string json = File.ReadAllText(_saveFilePath);
                if (string.IsNullOrEmpty(json)) return null;
                return JsonUtility.FromJson<UserSave>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Falha ao parsear save: {e.Message}");
                return null;
            }
        }

        public void SaveUser(UserSave userToSave)
        {
            if (userToSave == null) return;

            try
            {
                string json = JsonUtility.ToJson(userToSave, true);
                File.WriteAllText(_saveFilePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Erro ao gravar arquivo: {e.Message}");
            }
        }
    }
}