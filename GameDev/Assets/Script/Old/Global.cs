namespace BioAdventure.Assets.Script
{
    public static class Global
    {
        /*Gerencia os dados de sessão do usuário durante a execução*/
        private static int[] _rengScene = new int[2] { 0, 4 };
        public static UserSaveData CurrentUser { get; private set; }
        public static int CurrentLevel { get; private set; }
        public static int LevelPerformace { get; private set; }
        public static string CurrentScore { get; private set; }
        public static float VolumeEffects { get; private set; }
        public static float VolumeMusic { get; private set; }
        public static void StartSession(UserSaveData user)
        {
            CurrentUser = user;
        }
        public static void ChangeLevel(bool operacao)
        {
            if (CurrentLevel < _rengScene[1] && operacao && GlobalServices.IsLevelUnlocked(CurrentLevel))
                CurrentLevel++;
            else if (CurrentLevel > _rengScene[0] && !operacao)
                CurrentLevel--;
        }
        public static void SetLevelPerformace(int performace)
        {
            LevelPerformace = performace;
        }
        public static void SetCurrentScore(int score)
        {
            CurrentScore = score.ToString();
        }
        public static void SetVolumeEffects(float effects)
        {
            VolumeEffects = effects;
        }
        public static void SetVolumeMusic(float music)
        {
            VolumeMusic = music;
        }
    }
}