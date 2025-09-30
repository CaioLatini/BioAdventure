using System;

// UserSave.cs
/*
Esse script será responsável por definir a estrutura de dados que será salva e carregada do disco.
Ele não contém lógica, apenas campos públicos que serão serializados para o formato JSON,
representando o progresso e as configurações do usuário.
*/



namespace BioAdventure.Assets.Script.Data // Novo namespace
{
    [Serializable]
    public class UserSaveData
    {
        public string UserName;
        public string Directory;

        [Header("Configurações")]
        public float Music;
        public float Effects;

        [Header("Progresso nos Níveis")]
        public List<int> levelScore = new List<int>();
    }
}