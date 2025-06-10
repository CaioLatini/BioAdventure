using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

namespace BioAdventure.Assets.Script.Game
{
    public class GameManager : MonoBehaviour
    {
        //Responsável por gerenciar estado geral do jogo (vidas, score, objetos).
        [SerializeField] private GameController _gameController;
        [SerializeField] private BtnController _btnController;
        [SerializeField] private SoundTrackManager _soundTrackManager;
        [SerializeField] private GameObject _regresive;
        [SerializeField] private GameObject _scoreDisplay;
        [SerializeField] private List<GameObject> _livesDisplay;
        private int _currentLives = 7;
        private int _collectedCount = 0;
        private int _score = 0;
        private bool _play = false;
        private TextMeshProUGUI _counterText;

        private void Awake()
        {
            _counterText = _regresive.GetComponentInChildren<TextMeshProUGUI>();
        }
        private void Start()
        {
            if (Global.CurrentUser.Level1 == 0)
            {
                _btnController.InfoGeneral();
            }
                
            StartCoroutine(ContagemRegressiva());
        }
        private IEnumerator ContagemRegressiva()
        {
            for (int x = 3; x >= 0; x--)
            {
                _counterText.text = x.ToString();
                if(x != 0)
                    _soundTrackManager.PlaySound("timer");
                else _soundTrackManager.PlaySound("finish");
                yield return new WaitForSeconds(1f);
            }
            _regresive.SetActive(false);
            _play = true;
        }
        public void ApplyDamage()
        {
            _livesDisplay[_currentLives].SetActive(false);
            _currentLives--;
        }
        public void AddScore(bool correct)
        {
            _collectedCount++;
            _score += correct ? 2 : 1;
            _scoreDisplay.GetComponent<TextMesh>().text = _score.ToString();
        }

        public int Score => _score;
        public int CollectedCount => _collectedCount;
        public int Lives => _currentLives;
        public bool Play => _play;
    }
}