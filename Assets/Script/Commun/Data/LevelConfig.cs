using UnityEngine;

// LevelConfig.cs
/*
Define os parâmetros de cada nível do jogo.
Utiliza ScriptableObjects para permitir a criação e configuração de fases diretamente no Editor da Unity.
Armazena a versão de PC e a versão simplificada do Mobile.
*/

namespace BioAdventure.Assets.Script.Data 
{
    [CreateAssetMenu(fileName = "Level_Config_", menuName = "BioAdventure/Level Configuration")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Regras do Nível")]
        [Tooltip("Quantidade de tipos diferentes de lixo que podem aparecer na fase.")]
        public int maxTrashTypes;

        [Tooltip("Quantidade total de lixo que deve ser coletada para vencer a fase.")]
        public int requiredCount;

        [Header("Configurações do Spawner")]
        [Tooltip("Intervalo de tempo de geração do lixo (em segundos). X = Mínimo, Y = Máximo.")]
        public Vector2 spawnInterval = new Vector2(1.8f, 2.2f);

        [Tooltip("Intervalo da escala de gravidade (velocidade de queda) do lixo. X = Mínimo, Y = Máximo.")]
        public Vector2 gravityInterval = new Vector2(2.5f, 7.0f);
    }
}