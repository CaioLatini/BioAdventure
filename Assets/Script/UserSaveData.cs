using System;

namespace BioAdventure.Assets.Script
{
    /*Representa a estrutura de dados serializáveis para salvar progresso do usuário*/
    [Serializable]
    public class UserSaveData
    {
        public string UserName;
        public string Password;
        public string Directory;
        public float Music;
        public float Effects;

        public int Level1;
        public int Level2;
        public int Level3;
        public int Level4;
        public int Level5;
    }
}