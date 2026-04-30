using System;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

// TrashSpawner.cs
/*
Esse script será responsável por instanciar os prefabs de lixo na cena.
Ele receberá as regras do LevelController para gerar os itens de forma procedural durante a partida.
*/

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TrashSpawner : MonoBehaviour
    {
        [Header("Trash Spawner Area - PC")]
        [SerializeField] private float _rangeY;
        [SerializeField] private float _rangeZ;
        [SerializeField] private float _offset = 0.95f;


        [Header("Trash Spawner Spot - Mobile")]
        [SerializeField] private GameObject[] _spot = new GameObject[7];


        [Header("Trash Spawner Settings")]
        [SerializeField] private List<Sprite> _trashSprite;
        [SerializeField] private GameObject _trashGameObject;


        public void SpawTrash(int maxTrashType, Vector2 gravity, bool spotAdvanced = false)
        {
            if (_trashSprite == null || _trashSprite.Count == 0 || maxTrashType <= 0)
            {
                Debug.LogWarning("TrashSpawner: No trash prefabs assigned or invalid maxTrashType.");
                return;
            }

            Vector3 spawnPos = RandomPosition(spotAdvanced);

            GameObject newTrash = Instantiate(_trashGameObject, spawnPos, Quaternion.identity);
            Debug.Log("Instancia do lixo");

            int trashIndex = UnityEngine.Random.Range(0, Math.Min(maxTrashType * 3, _trashSprite.Count));
            float randomGravity = UnityEngine.Random.Range(gravity.x, gravity.y);

            ApplySettings(newTrash, trashIndex, randomGravity);
            Debug.Log("Applay Settings");
        }

        private void ApplySettings(GameObject obj, int spriteIndex, float gravity)
        {
            // 1. Sprite e Tag
            Sprite selectedSprite = _trashSprite[spriteIndex];
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.sprite = selectedSprite;

            // --- SISTEMA DE RESPONSIVIDADE ---
            float screenHeight = Camera.main.orthographicSize * 2f;
            float screenWidth = screenHeight * Camera.main.aspect;

            float desiredWidth = screenWidth * 0.15f;

            float currentSpriteWidth = sr.sprite.bounds.size.x;

            float scaleFactor = desiredWidth / currentSpriteWidth;
            obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            // ---------------------------------

            // 2. Tag
            if (selectedSprite.name.Contains("-"))
            {
                string tagname = selectedSprite.name.Split('-')[1];
                obj.tag = tagname;
            }

            // 3. Gravidade
            if (obj.GetComponent<Rigidbody2D>() != null)
            {
                obj.GetComponent<Rigidbody2D>().gravityScale = gravity;
            }
        }

        // private Vector3 RandomPosition(bool spotAdvanced)
        // {
        //     if(Application.isMobilePlatform)
        //     {
        //         return RandomPositionMobile(spotAdvanced);
        //     } else return RandomPositionPC();
        // }

        private Vector3 RandomPosition(bool spotAdvanced)
        {
            return RandomPositionMobile(spotAdvanced);
        }

        private Vector3 RandomPositionMobile(bool spotAdvanced)
        {
            if (spotAdvanced)
            {
                Debug.Log("RandomPositionMobile advanced is calede");
                return _spot[UnityEngine.Random.Range(0, 6)].transform.position;
            }
            else
            {
                Debug.Log("RandomPositionMobile not advanced is calede");
                return _spot[UnityEngine.Random.Range(0, 3)].transform.position;
            }
        }



        // Calcula uma posição horizontal (X) aleatória dentro dos limites definidos pelo GameManager
        private Vector3 RandomPositionPC()
        {
            float left = GameManager.Instance.LimitMap.x * _offset;
            float right = GameManager.Instance.LimitMap.y * _offset;

            return new Vector3(
                UnityEngine.Random.Range(left, right),
                _rangeY,
                _rangeZ
            );
        }
    }
}