using UnityEngine;

namespace BioAdventure.Assets.Script.Data 
{
    [CreateAssetMenu(fileName = "Level_Config", menuName = "BioAdventure/Level Configuration")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Regras do Nível")]
        [Tooltip("Quantidade total de lixo que deve ser coletada para vencer a fase.")]
        public int requiredCount;
        [Tooltip("Intervalo da tempo entre gerações (temporizador de spawn) do lixo. X = Mínimo, Y = Máximo.")]
        public Vector2 spawnInterval = new Vector2(0.6f, 1.5f);
        [Tooltip("Intervalo da escala de gravidade (velocidade de queda) do lixo. X = Mínimo, Y = Máximo. ++ Gravidade ++ Velocidade")]
        public Vector2 gravityInterval = new Vector2(0.1f, 0.35f);
        [Tooltip("Mecanica de gerar lixo nos spots dificeis")]
        public bool HardSpot = false;
        [Tooltip("Mecanica de gerar dois lixos simultaneos")]
        public bool DobleTrash = false;
        [Tooltip("Chance de gerar lixos simultaneos, em % sendo 0, 0% e 50, 50%")]
        public int OddDouble;
    }
}