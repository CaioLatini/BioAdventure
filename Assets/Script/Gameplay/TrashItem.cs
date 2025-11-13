using UnityEngine;
using System;
using BioAdventure.Assets.Script.Managers;

// TrashItem.cs
/*
Esse script será responsável pelo comportamento de um item de lixo individual.
Ele controlará sua rotação e, principalmente, a lógica de colisão para determinar
se a coleta foi correta ou incorreta, aplicando dano ou e a devida pontuação ao jogador.
*/

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TrashItem : MonoBehaviour
    {
        [Header("Configuração")]
        [SerializeField] private float _rotationSpeed = 180f;

        public static TrashItem Instance { get; private set; }
        public static event Action<bool, string> OnCollected;
        public static event Action OnMissed;
        private float _currentSpeedRotation;

        private void Awake()
        {
            Instance = this;
            _currentSpeedRotation = _rotationSpeed * Rotation();
        }

        private float Rotation()
        {
            int x = UnityEngine.Random.Range(0, 1);
            float y = UnityEngine.Random.Range(1, 3);
            return x > 0 ? -y : y;
        }

        private void Update()
        {
            transform.Rotate(0f, 0f, Time.deltaTime * _currentSpeedRotation);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            string tag = collision.gameObject.tag;
            Console.WriteLine($"Colidiu com: {tag}");

            if (tag.Equals("Untagged")) return;

            if (tag.Equals("Destroyer"))
            {
                OnMissed?.Invoke();
                Destroy(gameObject);
            }
            
            else
            {
                float volumeScale = 1f;
                switch (gameObject.tag)
                {
                    case "wood": volumeScale = 0.6f; break;
                    case "organic": volumeScale = 2f; break;
                    case "metal": volumeScale = 0.6f; break;
                    case "glass": volumeScale = 1.4f; break;
                }
                SoundManager.Instance.PlayEffect(gameObject.tag, volumeScale);
                bool wasCorrect = gameObject.tag.Equals(tag);
                OnCollected?.Invoke(wasCorrect, tag);
                Destroy(gameObject);
            }
            
        }
    }
}