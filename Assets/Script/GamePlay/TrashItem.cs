using UnityEngine;
using System;
using BioAdventure.Assets.Script.Managers;

// TrashItem.cs
/*
Esse script será responsável pelo comportamento de um item de lixo individual.
Ele controlará sua rotação e a lógica de colisão para determinar
se a coleta foi correta ou incorreta, aplicando a devida pontuação ao jogador.
*/

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TrashItem : MonoBehaviour
    {
        [Header("Configuração")]
        [Tooltip("Velocidade base de rotação do lixo enquanto cai.")]
        [SerializeField] private float _rotationSpeed = 180f;

        public static TrashItem Instance { get; private set; }
        
        // Eventos disparados ao coletar ou perder o lixo
        public static event Action<bool, string> OnCollected;
        public static event Action OnMissed;
        public static event Action onTutorial;
        
        private float _currentSpeedRotation;

        // Configura a instância e sorteia a direção da rotação
        private void Awake()
        {
            Instance = this;
            _currentSpeedRotation = _rotationSpeed * Rotation();
        }

        // Define se a rotação inicial será para a esquerda ou direita
        private float Rotation()
        {
            int x = UnityEngine.Random.Range(0, 2);
            float y = UnityEngine.Random.Range(1, 3);
            return x > 0 ? -y : y;
        }

        // Aplica a rotação contínua ao objeto a cada frame
        private void Update()
        {
            transform.Rotate(0f, 0f, Time.deltaTime * _currentSpeedRotation);
        }

        // Avalia a colisão do lixo com o chão (Destroyer) ou com a Lixeira (Bin)
        private void OnCollisionEnter2D(Collision2D collision)
        {
            string tag = collision.gameObject.tag;

            if (tag.Equals("Untagged")) return;

            if(tag.Equals("Tutorial"))
            {
                onTutorial?.Invoke();
                Destroy(collision.gameObject);
                return;
            }

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
                
                if(SoundManager.Instance != null) SoundManager.Instance.PlayEffect(gameObject.tag, volumeScale);
                         
                bool wasCorrect = gameObject.tag.Equals(tag);
                OnCollected?.Invoke(wasCorrect, tag);
                Destroy(gameObject);
            }
        }
    }
}