using System;
using UnityEngine;
using System.Collections.Generic;

// UserSave.cs
/*
Esse script será responsável por definir a estrutura de dados que será salva e carregada do disco.
Ele não contém lógica, apenas campos públicos que serão serializados para o formato JSON,
representando o progresso e as configurações do usuário.
*/



namespace BioAdventure.Assets.Script.Data // Novo namespace
{
    [Serializable]
    public class UserSave
    {
        public string UserName;

        [Header("Progresso nos Níveis")]
        public List<int> levelScore = new List<int>();
        public List<int> levelPerformace = new List<int>();
        public List<AchievementID> unLockedAchievements = new List<AchievementID>();
        public bool TutorialComplete;
    }
}