using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BioAdventure.Assets.Script.Core;
using UnityEngine;

// TrashSpawner.cs
/*
Esse script será responsável por instanciar os prefabs de lixo na cena.
Ele receberá as regras do LevelController para gerar os itens de forma procedural durante a partida.
*/

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TrashSpawner : MonoBehaviour
    {
        [Header("Trash Spawner Spot")]
        [SerializeField] private GameObject[] _spot = new GameObject[7];


        [Header("Trash Spawner Settings")]
        [SerializeField] private List<Sprite> _trashSprite;
        [SerializeField] private GameObject _trashGameObject;

        [Header("Scripts")]
        [SerializeField] private TutorialController _tutorialCOntroller;


        public void SpawTrash(float gravity, bool spotAdvanced = false, bool dobleTrash = false, int OddDouble = 0)
        {
            if (_trashSprite == null || _trashSprite.Count == 0)
            {
                Debug.LogWarning("TrashSpawner: No trash prefabs assigned or invalid maxTrashType.");
                return;
            }

            GameObject newTrash1 = null;
            GameObject newTrash2 = null;
            int num1 = -1;
            int num2 = -1;

            var ReturnPostion = RandomPosition(spotAdvanced, DobleTrash(dobleTrash, OddDouble));
            if (ReturnPostion is Vector2[] posV2)
            {
                Vector3 spawPos1 = posV2[0];
                Vector3 spawPos2 = posV2[1];

                newTrash1 = Instantiate(_trashGameObject, spawPos1, Quaternion.identity);
                newTrash2 = Instantiate(_trashGameObject, spawPos2, Quaternion.identity);

                num1 = UnityEngine.Random.Range(0, 3);
                do{num2 = UnityEngine.Random.Range(0, 3);}
                while(num1 == num2);

                Debug.LogWarning("Num1 é:"+num1+ "num2 é: "+num2);
            }
            else if (ReturnPostion is Vector3 posV3)
            {
                Vector3 spawPos = posV3;
                newTrash1 = Instantiate(_trashGameObject, spawPos, Quaternion.identity);
                num1 = UnityEngine.Random.Range(0, _trashSprite.Count);
            }
            else
            {
                Debug.LogError("Falha na geração do lixo");
                return;
            }

            
            if (newTrash2 != null)
            {
                ApplySettings(newTrash1, num1*3, gravity);
                ApplySettings(newTrash2, num2*3, gravity);
            } else ApplySettings(newTrash1, num1, gravity);

            if(!GameManager.Instance.CurrentUser.TutCaptureComplete)
            {
                _tutorialCOntroller.SetSettings(newTrash1.tag);
            }

            return;
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

        private object RandomPosition(bool spotAdvanced, bool dobleTrash)
        {
            if (dobleTrash)
            {
                Debug.Log("RandomPosition doble trash is calede");
                int num1 = UnityEngine.Random.Range(0, 3);
                int num2;
                do { num2 = UnityEngine.Random.Range(0, 3); }
                while (num1 == num2);

                return new Vector2[] { _spot[num1].transform.position, _spot[num2].transform.position };
            }
            else if (spotAdvanced)
            {
                Debug.Log("RandomPositionMobile advanced is calede");
                return _spot[UnityEngine.Random.Range(0, 6)].transform.position;
            }
            else
            {
                Debug.Log("RandomPositionMobile not advanced is calede");
                int number = UnityEngine.Random.Range(0, 3);
                if(!GameManager.Instance.CurrentUser.TutCaptureComplete)
                {
                _tutorialCOntroller.SetSettings(number);
                }
                return _spot[number].transform.position;
            }
        }
           

        private bool DobleTrash(bool DobleTrash, int OddDoble)
        {
            if (!DobleTrash)
            {
                Debug.Log("Level sem doble Trash");
                return false;
            }

            int num = UnityEngine.Random.Range(1, 100);
            Debug.Log("Numero sorteado da ODD do doble trash é ... " + num);
            if (num < OddDoble)
            {
                Debug.Log("Lá vem o doble trash");
                return true;
            }
            else
            {
                Debug.Log("Apenas um lixo agora");
                return false;
            }
        }
    }
}