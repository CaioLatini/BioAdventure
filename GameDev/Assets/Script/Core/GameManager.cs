using UnityEngine;
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
        public UserSave CurrentUser { get; private set; }
        public int CurrentLevel { get; private set; }
        public int LevelPerformace { get; private set; }
        public int CurrentScore { get; private set; }
        public float VolumeEffects { get; private set; }
        public float VolumeMusic { get; private set; }

        //Range of scenes for each level
        private int[] _rengScene = new int[2] { 0, 4 };


        public void StartSession(UserSave user)
        {
            if (user == null) return;

            CurrentUser = user;
            VolumeEffects = user.Effects;
            VolumeMusic = user.Music;
        }

        public void ChengeLevel(bool operacao)
        {

        }

        public bool UnlockedLevel(int indexLevel)
        {

        }

        public void SetCurrentScore()
        {

        }

        public void SetLevelPerformace()
        {

        }
        public void SaveProgress()
        {

        }

        public void SetVolumeEffects()
        {

        }

        public void SetVolumeMusic()
        {

        }
    }
}
