using UnityEngine;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;
using System.Collections.Generic;

// GameManager.cs
/*
Esse script será responsável por gerenciar o estado geral e persistente do jogo.
Atuando como Singleton, ele manterá os dados da sessão do usuário,
o nível atual, configurações de volume e outras informações globais
que precisam sobreviver entre as trocas de cena.
*/


namespace BioAdventure.Assets.Script.Core
{
    public class GameManager : MonoBehaviour
    {
        private TextData textaData = new TextData();
        public static GameManager Instance { get; private set; }

        void
        Awake()
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

            Debug.Log("GameManager Initialized");
        }

        //Global variables
        public UserSave CurrentUser { get; private set; }
        public int CurrentLevel { get; private set; }
        public int CurrentScore { get; private set; }
        public int CurrentPerformace { get; private set; }
        public bool HasWon { get; private set; }
        public bool ShowAchievementsOnMenuLoad { get; set; }

        //Range of scenes for each level
        private int[] _rengScene = new int[2] { 0, 4 };


        public void StartSession(UserSave user)
        {
            if (user == null) return;

            CurrentUser = user;
        }

        public void SaveProgress()
        {
            if (CurrentUser == null)
                return;

            CurrentUser.TutorialComplete = true;
            
            if (CurrentScore > CurrentUser.levelScore[CurrentLevel])
            {
                CurrentUser.levelScore[CurrentLevel] = CurrentScore;
            }
            if( CurrentPerformace > CurrentUser.levelPerformace[CurrentLevel])
            {
                CurrentUser.levelPerformace[CurrentLevel] = CurrentPerformace;
            }
            if (AchievementsManager.Instance != null)
            {
                CurrentUser.unLockedAchievements = AchievementsManager.Instance.GetUnlockedAchievementsAsList();
            }

            SaveManager.Instance.SaveUser(CurrentUser);
        }

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

        public bool UnlockedLevel(int indexLevel)
        {
            return CurrentUser.levelScore[indexLevel] > 0;
        }

        public void SetCurrentScore(int Score)
        {
            CurrentScore = Score;
        }
        
        public void SetCurrentPerformace(int Performace)
        {
            CurrentPerformace = Performace;
        }

        public void SetCurrentWinState(bool hasWon)
        {
            HasWon = hasWon;
        }
    }
}
