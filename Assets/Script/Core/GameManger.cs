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
        [HideInInspector] public static GameManager Instance { get; private set; }
        [HideInInspector] public UserSave CurrentUser { get; private set; }
        [HideInInspector] public bool HasWon { get; private set; }
        [HideInInspector] public int CurrentLevel { get; private set; }
        [HideInInspector] public int CurrentScore { get; private set; }
        [HideInInspector] public int CurrentPerformace { get; private set; }
        [HideInInspector] public bool ShowAchievementsOnMenuLoad { get; set; }

        [Header("Levels")]
        public List<LevelConfig> levels = new List<LevelConfig>();
        public List<Sprite> WinBackgrounds;
        public List<Sprite> LoseBackgrounds;

        private int _rengScene;


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

        private void Start()
        {
            _rengScene = levels.Count;

            if (SaveManager.Instance != null)
            {

                var user = SaveManager.Instance.LoadUser();
                Debug.Log("user:" + user);

                if (user == null)
                {
                    user = new UserSave
                    {
                        UserName = "Player",
                        levelScore = new List<int>(new int[levels.Count]),
                        levelPerformace = new List<int>(new int[levels.Count])
                    };
                }
                StartSession(user);
                if (AchievementsManager.Instance != null)
                {
                    AchievementsManager.Instance.LoadAchievements(user);
                }
                else Debug.LogError("Achievements não carregados");
            }
            else Debug.LogError("Save manager não instanciado");
        }


        public void StartSession(UserSave user)
        {
            if (user == null)
            {
                Debug.LogError("Player não encontrado");
                return;
            }
            CurrentUser = user;
            Debug.Log(user.UserName + " foi carregado");
        }


        public void SaveProgress()
        {
            if (CurrentUser == null) return;


            if (CurrentScore > CurrentUser.levelScore[CurrentLevel])
            {
                CurrentUser.levelScore[CurrentLevel] = CurrentScore;
            }

            if (CurrentPerformace > CurrentUser.levelPerformace[CurrentLevel])
            {
                CurrentUser.levelPerformace[CurrentLevel] = CurrentPerformace;
            }

            if (AchievementsManager.Instance != null)
            {
                CurrentUser.unLockedAchievements = AchievementsManager.Instance.GetUnlockedAchievementsAsList();
            }

            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveUser(CurrentUser);
            }
        }


        public void ChangeLevel(bool operacao)
        {
            if (operacao && UnlockedLevel(CurrentLevel) && CurrentLevel < _rengScene)
            {
                CurrentLevel++;
            }
            if (!operacao && CurrentLevel > 0)
            {
                CurrentLevel--;
            }
        }


        public bool UnlockedLevel(int indexLevel)
        {
            if (CurrentUser == null || indexLevel >= CurrentUser.levelScore.Count) return false;
            return CurrentUser.levelPerformace[indexLevel] > 0;
        }


        public void SetCurrentScore(int Score)
        {
            CurrentScore = Score;
            Debug.Log("Seu score foi calculado: " + Score);
        }


        public void SetCurrentPerformace(int Performace)
        {
            CurrentPerformace = Performace;
            Debug.Log("Sua performace foi calculada: " + Performace);
        }


        public void SetCurrentWinState(bool hasWon)
        {
            HasWon = hasWon;
            Debug.Log("Você " + (hasWon ? "ganhou" : "perdeu"));
        }

        public void SetTutorialComplete(int Tutorial)
        {
            switch (Tutorial)
            {
                case 1:
                    CurrentUser.TutMoveComplete = true;
                    break;
                case 2:
                    CurrentUser.TutCaptureComplete = true;
                    break;
                case 3:
                    CurrentUser.TutMenuComplete = true;
                    break;
                default:
                    return;
            }

            SaveProgressTut();
        }

        public void SetLenguage(bool _lenguage)
        {
            CurrentUser.Lenguage = _lenguage;
            SaveLenguage();
        }

        private void SaveProgressTut()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveUser(CurrentUser);
            }
        }

        private void SaveLenguage()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveUser(CurrentUser);
            }
        }
    }
}