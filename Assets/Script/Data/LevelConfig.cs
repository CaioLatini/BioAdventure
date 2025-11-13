using UnityEngine;

// LevelConfig.cs
/*
Esse script será responsável por definir os parâmetros de cada nível de forma desacoplada.
Como um Scriptable Object, ele permitirá criar "assets" de configuração no editor da Unity
para cada fase, definindo variáveis como a quantidade de lixo a ser coletada,
os tipos de lixo que podem aparecer e o intervalo de spawn.
*/



namespace BioAdventure.Assets.Script.Data 
{
    [CreateAssetMenu(fileName = "Level_Config_", menuName = "BioAdventure/Level Configuration")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Regras do Nível")]
        [Tooltip("Index do tipos de lixo que podem aparecer.")]
        public int maxTrashTypes;

        [Tooltip("Quantidade de lixo a ser gerado.")]
        public int requiredCount;

        [Header("Configurações do Spawner")]
        [Tooltip("Intervalo de tempo para o spawn de lixo. X = Mínimo, Y = Máximo.")]
        public Vector2 spawnInterval = new Vector2(1.8f, 2.2f);

        [Tooltip("Intervalo de gravidade(velocidade de queda) para o spawn de lixo. X = Mínimo, Y = Máximo.")]
        public Vector2 gravityInterval = new Vector2(2.5f, 7.0f);
    }
}