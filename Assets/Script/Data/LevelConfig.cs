using UnityEngine;

namespace BioAdventure.Assets.Script.Data 
{
    [CreateAssetMenu(fileName = "Level_Config", menuName = "BioAdventure/Level Configuration")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Regras do Nível")]
        [Tooltip("Quantidade total de lixo que deve ser coletada para vencer a fase.")]
        public int requiredCount;
        [Tooltip("Tempo entre gerações (temporizador de spawn) do lixo.")]
        public float spawnTemp;
        [Tooltip("Escala de gravidade (velocidade de queda) do lixo. ++ Gravidade ++ Velocidade")]
        public float gravity;
        [Tooltip("Mecanica de gerar lixo nos spots dificeis")]
        public bool HardSpot = false;
        [Tooltip("Mecanica de gerar dois lixos simultaneos")]
        public bool DobleTrash = false;
        [Tooltip("Chance de gerar lixos simultaneos, em % sendo 0, 0% e 50, 50%")]
        public int OddDouble;
    }
}