using System;
using System.Collections;
using UnityEngine;


namespace BioAdventure.Assets.Script.Gameplay
{
    public class teste : MonoBehaviour
    {
        [SerializeField] TrashSpawner _trashSpawner;

        private void Start()
        {
            Debug.Log("Start");
            StartCoroutine(SpawnTrash());
        }

        private IEnumerator SpawnTrash()
        {
            while(true){
            Debug.Log("Chamando SpawTrash");
            _trashSpawner.SpawTrash(4, new Vector2(1, 3));
            yield return new WaitForSeconds(1);
            } 

        }
    }
}