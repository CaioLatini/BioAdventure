using UnityEngine;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;

// LevelControllerMobile.cs
/*
Controla o fluxo do nível na versão Mobile.
Lê as configurações de LevelConfigMobile, gerencia a rotina do Spawner de lixos,
contabiliza os acertos e erros, e avalia a condição de vitória ou derrota.
*/

namespace BioAdventure.Assets.Script.Gameplay.Mobile
{
    public class LevelControllerMobile : MonoBehaviour
    {
        [Header("Configurações do Nível")]
        [Tooltip("Configuração de regras específicas para os níveis Mobile (ScriptableObject).")]
        [SerializeField] private LevelConfigMobile[] _levelConfig;
        private LevelConfigMobile currentLevel;
        
        [Header("Referências")]
        [Tooltip("Referência ao Spawner responsável por instanciar os lixos na cena.")]
        [SerializeField] private TrashSpawner _trashSpawner;

        public int CollectedCount { get; private set; }
        public int MissedCount { get; private set; }
        private bool _isLevelActive;

        // Inicia as variáveis e dispara a rotina de spawn
        public void StartLevel()
        {
            if (_levelConfig == null || _trashSpawner == null)
            {
                Debug.LogError("[LevelControllerMobile] Configuração ou Spawner ausentes!");
                return;
            }
            currentLevel = _levelConfig[GameManager.Instance.CurrentLevel];
            Debug.Log("Nivel atual: " + GameManager.Instance.CurrentLevel);

            CollectedCount = 0;
            MissedCount = 0;
            _isLevelActive = true;
            
            StartCoroutine(SpawnerRoutine());
        }

        // Interrompe a geração de lixo e a lógica da fase
        public void StopLevel()
        {
            _isLevelActive = false;
            StopAllCoroutines();
        }

        // Rotina de geração contínua de lixos no intervalo de tempo
        private IEnumerator SpawnerRoutine()
        { // Pode ser movido para LevelConfigMobile futuramente
            while (_isLevelActive)
            {
                float temp = UnityEngine.Random.Range(currentLevel.spawnInterval[0], currentLevel.spawnInterval[1]);
                Debug.Log("Geração em... "+ temp);
                
                _trashSpawner.SpawTrash(4, currentLevel.gravityInterval, currentLevel.AdvancedLevel);
                yield return new WaitForSeconds(temp);;
            }
        }

        public void RegisterCollection()
        {
            if (!_isLevelActive) return;

            CollectedCount++;

            CheckWinCondition();
        }

        // Incrementa o contador de lixos perdidos (caíram no chão)
        public void RegisterMiss()
        {
            if (!_isLevelActive) return;
            
            MissedCount++;
            CheckWinCondition();
        }

        // Verifica constantemente se as metas de vitória ou limites de derrota foram atingidos
        private void CheckWinCondition()
        {
            if (CollectedCount >= currentLevel.requiredCount)
            {
                StopLevel();
                if (GameControllerMobile.Instance != null) GameControllerMobile.Instance.WinGame();
            }
            else if (MissedCount >= 3) 
            {
                StopLevel();
                if (GameControllerMobile.Instance != null) GameControllerMobile.Instance.LoseGame();
            }
        }
    }
}