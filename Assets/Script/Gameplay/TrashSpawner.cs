using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TrashSpawner.cs
/*
Esse script será responsável por instanciar os prefabs de lixo na cena.
Ele receberá as regras do GameController para gerar os itens de forma procedural durante a partida.
*/

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TrashSpawner : MonoBehaviour
    {
        [Header("Trash Spawner area")]
        [SerializeField] private float _rengeY;
        [SerializeField] private float _rengeZ;
        [SerializeField] private float _minRangeX;
        [SerializeField] private float _maxRengeX;

        [Header("Trash Spawner Settings")]
        [SerializeField] private List<Sprite> _trashSprite;
        [SerializeField] private GameObject _trashGameObject;

        public void SpawTrash(int maxTrashType, Vector2 gravity)
        {
            if (_trashSprite.Count == 0 || _trashSprite == null || maxTrashType <= 0)
            {
                Debug.LogWarning("TrashSpawner: No trash prefabs assigned or invalid maxTrashType.");
                return;
            }
            Debug.Log("Spawn iniciado TrashSpawner");
            int trashIndex = UnityEngine.Random.Range(0, Math.Min(maxTrashType * 3, _trashSprite.Count));
            SetSpriteAndTagTrash(trashIndex);
            SetGravityTrash(UnityEngine.Random.Range(gravity.x, gravity.y));
            Instantiate(_trashGameObject, RandomPosition(), Quaternion.identity);
        }

        private Vector3 RandomPosition()
        {
            return new Vector3(
            UnityEngine.Random.Range(_minRangeX, _maxRengeX), _rengeY, _rengeZ);
        }

        private void SetSpriteAndTagTrash(int spriteIndex)
        {
            //Set Sprite
            _trashGameObject.GetComponent<SpriteRenderer>().sprite = _trashSprite[spriteIndex];

            //Set tag
            string tagname = _trashSprite[spriteIndex].name.Split('-')[1];
            _trashGameObject.tag = tagname;
        }

        private void SetGravityTrash(float gravity)
        {
            _trashGameObject.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
    }
}