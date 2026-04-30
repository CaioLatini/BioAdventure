using UnityEngine;

// LevelConfigMobile.cs
/*
Define os parâmetros de cada nível do jogo.
Utiliza ScriptableObjects para permitir a criação e configuração de fases diretamente no Editor da Unity.
Armazena a versão de PC e a versão simplificada do Mobile.
*/

namespace BioAdventure.Assets.Script.Data 
{
    [CreateAssetMenu(fileName = "Level_Config_Mobile_", menuName = "BioAdventure/Level Configuration Mobile")]
    public class LevelConfigMobile : ScriptableObject
    {
        [Header("Regras do Nível (Mobile)")]
        [Tooltip("Quantidade total de lixo que deve ser coletada para vencer a fase.")]
        public int requiredCount;
        [Tooltip("Intervalo da tempo entre gerações (temporizador de spawn) do lixo. X = Mínimo, Y = Máximo.")]
        public Vector2 spawnInterval = new Vector2(0.6f, 2.0f);
        [Tooltip("Intervalo da escala de gravidade (velocidade de queda) do lixo. X = Mínimo, Y = Máximo. ++ Gravidade ++ Velocidade")]
        public Vector2 gravityInterval = new Vector2(0.6f, 7.0f);
        [Tooltip("Dificuldade do nivel, mecanica de gerar lixo nos spots dificeis")]
        public bool AdvancedLevel = false;
    }
}