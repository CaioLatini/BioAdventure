using System;
using UnityEngine;

// Achievement.cs
/*
Define a estrutura de dados para uma conquista (Achievement) no jogo.
Não possui lógica, apenas campos que serão preenchidos via Unity Editor (Inspector).
*/

namespace BioAdventure.Assets.Script.Data
{   
    [Serializable]
    public class Achievement
    {
        [Tooltip("Identificador único da conquista (selecionado a partir do Enum).")]
        public AchievementID id;
        
        [Tooltip("Título da conquista exibido para o jogador.")]
        public string title;
        
        [Tooltip("Descrição de como desbloquear (ou a lore) da conquista.")]
        public string description;
        
        [Tooltip("Ícone exibido no popup e no menu de conquistas.")]
        public Sprite icon;
    }

    // Enumeração centralizada de todas as conquistas do jogo
    public enum AchievementID
    {
        Defeat,
        Victory,
        Perfect,
        TheEnd,
        WhatYouDoing,
        AbsoluteCinema
    }
}