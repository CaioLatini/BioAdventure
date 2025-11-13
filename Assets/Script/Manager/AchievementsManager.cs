using System;
using System.Collections.Generic;
using UnityEngine;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Core; // Para acessar o UserSave

namespace BioAdventure.Assets.Script.Managers
{
    public class AchievementsManager : MonoBehaviour
    {
        public static AchievementsManager Instance { get; private set; }

        [Header("Definições das Conquistas")]
        [SerializeField] private List<Achievement> allAchievementsDefs;

        private Dictionary<AchievementID, Achievement> _achievementDictionary;
        private HashSet<AchievementID> _unlockedAchievements;
        public Queue<Achievement> AchievementsToDisplay = new Queue<Achievement>();


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDictionary();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeDictionary()
        {
            _achievementDictionary = new Dictionary<AchievementID, Achievement>();
            foreach (var achievement in allAchievementsDefs)
            {
                _achievementDictionary[achievement.id] = achievement;
            }
        }

        public void LoadAchievements(UserSave user)
        {
            if (user.unLockedAchievements == null)
            {
                user.unLockedAchievements = new List<AchievementID>();
            }
            _unlockedAchievements = new HashSet<AchievementID>(user.unLockedAchievements);
        }

        public void UnlockAchievement(AchievementID id)
        {
            if (_unlockedAchievements == null || _unlockedAchievements.Contains(id))
            {
                Debug.Log("Conquista já desbloqueada");
                return;
            }

            Debug.Log($"Conquista Desbloqueada: {id}");

            _unlockedAchievements.Add(id);

            if (_achievementDictionary.TryGetValue(id, out Achievement achievement)) AchievementsToDisplay.Enqueue(achievement);
        }

        public Achievement GetAchievementData(AchievementID id)
        {
            if (_achievementDictionary.TryGetValue(id, out Achievement achievement)) return achievement;
            
            Debug.LogWarning($"Dados da conquista não encontrados para: {id}");
            return null;
        }

        public bool IsUnlocked(AchievementID id)
        {
            return _unlockedAchievements != null && _unlockedAchievements.Contains(id);
        }

        public List<AchievementID> GetUnlockedAchievementsAsList()
        {
            if (_unlockedAchievements == null)
            {
                return new List<AchievementID>();
            }
            return new List<AchievementID>(_unlockedAchievements);
        }
    }
}