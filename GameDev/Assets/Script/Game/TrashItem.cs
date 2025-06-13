using UnityEngine;

namespace BioAdventure.Assets.Script.Game
{
    public class TrashItem : MonoBehaviour
    {
        //Controla rotação do lixo e lógica de colisão para pontuar ou causar dano
        private SoundTrackManager _soundTrackManager;
        private GameManager _gameManager;
        private float _rotationSpeed = 360f;
        private void Awake()
        {
            _soundTrackManager = FindAnyObjectByType<SoundTrackManager>();
            _gameManager = FindAnyObjectByType<GameManager>();
        }
        private void Update()
        {
            transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime * Random.Range(-2f, 2f));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Bin"))
            {
                bool correct = collision.gameObject.tag.Contains(gameObject.tag);
                string name = collision.gameObject.tag;
                
                _soundTrackManager.PlaySound(name);
                _gameManager.AddScore(correct);
                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("destroyer"))
            {
                _soundTrackManager.PlaySound("wrong");
                _gameManager.ApplyDamage();
                Destroy(gameObject);
            }
        }
    }
}