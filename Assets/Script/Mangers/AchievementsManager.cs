using System.Collections.Generic;
using UnityEngine;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Core;
using System.Linq;

// AchievementsManager.cs
/*
Responsável por gerenciar o estado das conquistas do jogador.
Ele verifica quais já foram desbloqueadas, registra novos desbloqueios,
e mantém uma fila de conquistas recém-desbloqueadas para a UI exibir popups.
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class AchievementsManager : MonoBehaviour
    {
        public static AchievementsManager Instance { get; private set; }

        [Header("Definições das Conquistas")]
        [Tooltip("Lista com os dados (título, ícone, descrição) de todas as conquistas. Configure no Inspector.")]
        [SerializeField] private List<Achievement> allAchievementsDefs;

        // Dicionário para busca rápida O(1) dos dados de uma conquista pelo seu ID
        private Dictionary<AchievementID, Achievement> _achievementDictionary;

        // HashSet garante que não tenhamos conquistas duplicadas no save
        private HashSet<AchievementID> _unlockedAchievements;
        private GameManager _gameManager;

        // Fila consumida pela UI (EndGameUI/AchievementUI) para exibir os popups um por um
        [HideInInspector] public Queue<Achievement> AchievementsToDisplay = new Queue<Achievement>();

        // Configura o Singleton e inicializa o dicionário de consulta
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

        // Converte a lista do Inspector em um dicionário para facilitar e otimizar as buscas futuras
        private void InitializeDictionary()
        {
            _achievementDictionary = new Dictionary<AchievementID, Achievement>();

            if (allAchievementsDefs == null) return;

            foreach (var achievement in allAchievementsDefs)
            {
                _achievementDictionary[achievement.id] = achievement;
            }
        }

        // Chamado após o AuthService carregar o save do usuário para injetar as conquistas salvas
        public void LoadAchievements(UserSave user)
        {
            if (user.unLockedAchievements == null)
            {
                Debug.LogWarning("Nenhuma lista de achievement encontrada, criando nova...");
                user.unLockedAchievements = new List<AchievementID>();
            }
            _unlockedAchievements = new HashSet<AchievementID>(user.unLockedAchievements);
        }

        // Desbloqueia uma conquista, se ela ainda não foi conquistada pelo jogador
        public void UnlockAchievement(AchievementID id)
        {
            // Valida se o sistema foi carregado e se a conquista já existe no HashSet
            if (_unlockedAchievements == null || _unlockedAchievements.Contains(id))
            {
                return;
            }

            Debug.Log($"[AchievementsManager] Conquista Desbloqueada: {id}");
            _unlockedAchievements.Add(id);

            // Adiciona a conquista na fila para a UI de Fim de Jogo exibir a notificação
            if (_achievementDictionary.TryGetValue(id, out Achievement achievement))
            {
                AchievementsToDisplay.Enqueue(achievement);
            }
        }

        // Retorna os dados visuais (ícone, título, etc) de uma conquista específica para a UI
        public Achievement GetAchievementData(AchievementID id)
        {
            if (_achievementDictionary != null && _achievementDictionary.TryGetValue(id, out Achievement achievement))
            {
                return achievement;
            }

            Debug.LogWarning($"[AchievementsManager] Dados da conquista não encontrados para o ID: {id}");
            return null;
        }

        // Verifica o status de uma conquista (usado pela UI para saber se pinta o ícone ou mostra a descrição)
        public bool IsUnlocked(AchievementID id)
        {
            return _unlockedAchievements != null && _unlockedAchievements.Contains(id);
        }

        // Converte o HashSet de volta para Lista para o GameManager salvar no formato JSON do UserSave
        public List<AchievementID> GetUnlockedAchievementsAsList()
        {
            if (_unlockedAchievements == null)
            {
                return new List<AchievementID>();
            }
            return new List<AchievementID>(_unlockedAchievements);
        }

        #region Validação de Conquistas de Fim de Jogo
        public void ValidateEndGameAchievements(bool hasWon)
        {
            _gameManager = GameManager.Instance;
            
            if (hasWon)
                UnlockAchievement(AchievementID.Victory);
            else{
                UnlockAchievement(AchievementID.Defeat);
                return;
            }

            if (_gameManager.CurrentPerformace == 4)
                UnlockAchievement(AchievementID.Perfect);
            
            if(_gameManager.CurrentScore == _gameManager.levels[GameManager.Instance.CurrentLevel].requiredCount)
                UnlockAchievement(AchievementID.WhatYouDoing);

            if(_gameManager.CurrentLevel == _gameManager.levels.Count)
                UnlockAchievement(AchievementID.TheEnd);

            if(_gameManager.CurrentUser.levelPerformace.Sum() == _gameManager.levels.Count*4)
                UnlockAchievement(AchievementID.AbsoluteCinema);

            return;
        }
        #endregion
    }
}