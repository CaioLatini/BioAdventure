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
        [Tooltip("Nome único do usuário. Usado como identificador no login.")]
        public string UserName;

        [Header("Progresso nos Níveis")]
        
        [Tooltip("Lista que armazena a maior pontuação (Score) do jogador em cada nível (o índice da lista representa o ID do nível).")]
        public List<int> levelScore = new List<int>();
        
        [Tooltip("Lista que armazena a quantidade máxima de estrelas (1 a 4) obtidas em cada nível. Mantida a grafia 'Performace' para não quebrar compatibilidade de saves antigos (JSON).")]
        public List<int> levelPerformace = new List<int>();
        
        [Tooltip("Lista de IDs de conquistas que o jogador já desbloqueou.")]
        public List<AchievementID> unLockedAchievements = new List<AchievementID>();
        
        [Tooltip("Flag que indica se o jogador já concluiu o tutorial inicial, para não exibi-lo novamente.")]
        public bool TutorialComplete;
    }
}