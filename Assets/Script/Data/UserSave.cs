using System;
using UnityEngine;
using System.Collections.Generic;

// UserSave.cs
/*
Define a estrutura de dados que será salva e carregada do disco/navegador.
Esta classe não contém lógica de jogo, servindo apenas como um "molde" (modelo de dados)
cujos campos públicos serão convertidos para o formato JSON pelo SaveManager.
Representa o progresso completo de um jogador específico.
*/

namespace BioAdventure.Assets.Script.Data
{
    // A tag [Serializable] é obrigatória para que o Unity e o JsonUtility consigam converter esta classe em texto (JSON) e vice-versa.
    [Serializable]
    public class UserSave
    {
        public string UserName;
        public List<int> levelScore = new List<int>();
        public List<int> levelPerformace = new List<int>();
        public List<AchievementID> unLockedAchievements = new List<AchievementID>();

        public bool Lenguage = false; //false = en-us true= pt-br
        public bool TutMoveComplete;
        public bool TutCaptureComplete;
        public bool TutMenuComplete;
    }
}