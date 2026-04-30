using UnityEngine;
using UnityEngine.SceneManagement;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;
using System.Collections.Generic;

// GameManager.cs
/*
Responsável por gerenciar o estado global e persistente do jogo.
Atua como Singleton, mantendo os dados da sessão do usuário, o nível atual,
configurações de pontuação e cálculos de limites de tela que precisam
sobreviver entre as trocas de cena.
*/

namespace BioAdventure.Assets.Script.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Screen Boundaries")]
        [Tooltip("Limites laterais da câmera no mundo. X = Esquerda, Y = Direita.")]
        public Vector2 LimitMap;
        [Tooltip("Altura da metade da tela (Baseada no Orthographic Size da câmera).")]
        public float HeightMap;

        [Header("Global Variables")]
        [Tooltip("Referência aos dados salvos do jogador atual.")]
        public UserSave CurrentUser { get; private set; }
        
        [Tooltip("Índice do nível atual que o jogador está jogando.")]
        public int CurrentLevel { get; private set; }
        
        [Tooltip("Pontuação obtida na partida atual.")]
        public int CurrentScore { get; private set; }
        
        [Tooltip("Performance (estrelas de 1 a 4) obtida na partida atual.")]
        public int CurrentPerformace { get; private set; }
        
        [Tooltip("Define se a última partida resultou em vitória ou derrota.")]
        public bool HasWon { get; private set; }
        
        [Tooltip("Flag (sinalizador) para a UI de Menu abrir automaticamente a tela de conquistas.")]
        public bool ShowAchievementsOnMenuLoad { get; set; }

        // Intervalo de cenas para cada nível (Índice mínimo e máximo de fases)
        private int[] _rengScene = { 0, 4 };

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

        // Inscreve o evento de carregamento de cena
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Remove a inscrição do evento para evitar vazamento de memória
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // É chamado automaticamente sempre que uma nova cena termina de carregar
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CalculateScreenBoundaries();
        }

        // Calcula os limites físicos da tela para spawn de lixo e movimentação da lixeira
        private void CalculateScreenBoundaries()
        {
            if (Camera.main == null) return;

            HeightMap = Camera.main.orthographicSize;
            float screenWidth = HeightMap * 2.0f * Camera.main.aspect;

            float rightLimit = screenWidth / 2f;
            float leftLimit = -rightLimit;

            LimitMap = new Vector2(leftLimit, rightLimit);
            Debug.Log($"[GameManager] Limites calculados para cena ativa: Esq {LimitMap.x} | Dir {LimitMap.y} | Altura {HeightMap}");
        }

        // Inicia a sessão de jogo injetando o usuário carregado ou recém-criado
        public void StartSession(UserSave user)
        {
            if (user == null) return;
            CurrentUser = user;
        }

        // Consolida a pontuação atual no perfil do usuário e chama o SaveManager para persistir
        public void SaveProgress()
        {
            if (CurrentUser == null) return;

            CurrentUser.TutorialComplete = true;

            // Só salva o recorde se a pontuação atual for maior que a anterior
            if (CurrentScore > CurrentUser.levelScore[CurrentLevel])
            {
                CurrentUser.levelScore[CurrentLevel] = CurrentScore;
            }

            // Só salva a performance máxima se a atual for maior que a anterior
            if (CurrentPerformace > CurrentUser.levelPerformace[CurrentLevel])
            {
                CurrentUser.levelPerformace[CurrentLevel] = CurrentPerformace;
            }

            // Atualiza conquistas com segurança (evitando erro caso teste a cena isolada)
            if (AchievementsManager.Instance != null)
            {
                CurrentUser.unLockedAchievements = AchievementsManager.Instance.GetUnlockedAchievementsAsList();
            }

            // Pede ao SaveManager para escrever no disco/navegador
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveUser(CurrentUser);
            }
        }

        // Avança ou retrocede o índice de nível atual com base nas regras de desbloqueio
        public void ChangeLevel(bool operacao)
        {
            if (operacao && UnlockedLevel(CurrentLevel) && CurrentLevel < _rengScene[1])
            {
                CurrentLevel++;
            }
            if (!operacao && CurrentLevel > _rengScene[0])
            {
                CurrentLevel--;
            }
        }

        // Retorna verdadeiro se o nível possui alguma pontuação prévia (indicando que foi completado)
        public bool UnlockedLevel(int indexLevel)
        {
            if (CurrentUser == null || indexLevel >= CurrentUser.levelScore.Count) return false;
            return CurrentUser.levelScore[indexLevel] > 0;
        }

        // Define a pontuação da partida que acabou de acontecer
        public void SetCurrentScore(int Score)
        {
            CurrentScore = Score;
            Debug.Log("Seu score foi calculado: "+Score);
        }

        // Define a performance (estrelas) da partida que acabou de acontecer
        public void SetCurrentPerformace(int Performace)
        {
            CurrentPerformace = Performace;
            Debug.Log("Sua performace foi calculada: "+Performace);
        }

        // Registra se o jogador ganhou ou perdeu a partida atual
        public void SetCurrentWinState(bool hasWon)
        {
            HasWon = hasWon;
            Debug.Log("Você " + (hasWon ? "ganhou" : "perdeu"));

        }
    }
}