using System.Collections.Generic;
using UnityEngine;

namespace BioAdventure.Assets.Script.Game
{
    public class TrashSpawner : MonoBehaviour
    {
        //Gera itens de lixo na cena com base em parâmetros do nível
        
        [Header("Spawn Settings")]
        [SerializeField] private List<GameObject> _trashPrefabs;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _xMin, _xMax, _y, _z;
        
        public void Spawn(int _maxTrashTypes)
        {
            var prefab = _trashPrefabs[Random.Range(0, _maxTrashTypes)];
            Instantiate(prefab, RandomPosition(), Quaternion.identity);
        }

        private Vector3 RandomPosition()
        {
            float x = Random.Range(_xMin, _xMax);
            return new Vector3(x, _y, _z);
        }
    }
}