using UnityEngine;
using BioAdventure.Assets.Script.Data;
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
    public class GameManager : MonoBehaviour, IGameManager
    {

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
        }

        //Global variables
        public UserSaveData CurrentUser { get; private set; }
        public int CurrentLevel { get; private set; }
        public int CurrentScore { get; private set; }
        public float VolumeEffects { get; private set; }
        public float VolumeMusic { get; private set; }

        //Range of scenes for each level
        private int[] _rengScene = new int[2] { 0, 4 };


        public void StartSession(UserSave user)
        {
            if (user == null) return;

            CurrentUser = user;
            SetVolumeEffects(user.Effects);
            SetVolumeMusic(user.Music);
        }

        public void ChengeLevel(bool operacao)
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
            if (indexLevel == 0) return true;

            return CurrentUser.levelScore[indexLevel] > 0;
        }

        public void SetCurrentScore(int score)
        {
            CurrentScore = score; 
        }

        public void SaveProgress()
        {
            if (CurrentUser == null)
                return;

            if (CurrentScore > CurrentUser.levelScore[CurrentLevel])
            {
                CurrentUser.levelScore[CurrentLevel] = CurrentScore;
            }  
        }
        
        public void SetVolumeEffects(float volume)
        {
            VolumeEffects = Mathf.Clamp01(volume);
            CurrentUser.Effects = VolumeEffects;
        }

        public void SetVolumeMusic(float volume)
        {
            VolumeMusic = Mathf.Clamp01(volume);
            CurrentUser.Music = VolumeMusic;
        }
    }
}
