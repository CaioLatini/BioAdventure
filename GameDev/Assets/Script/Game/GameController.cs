using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BioAdventure.Assets.Script.Game
{
    public class GameController : MonoBehaviour
    {
        //Responsável por controlar a fase atual, interações e condições.
        [SerializeField] private SoundTrackManager _SoundTrackManager;
        [SerializeField] private TrashSpawner _trashSpwaner;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private BtnController _btnController;
        [SerializeField] private GameObject _bin;
        [SerializeField] private List<Sprite> _binSprite;
        private int _maxTrashTypes;
        private int _requiredCount;
        private float[] _spawnInterval;
        private bool _run = false;
        private char[] _char = new char[] { 'Q', 'W', 'E', 'R', 'A', 'S', 'D', 'F' };

        private void Awake()
        {
            _rules(Global.CurrentLevel);
        }
        private void Update()
        {
            if (_gameManager.Play && !_run)
            {
                StartCoroutine(Spawner());
                _run = true;
            }

            foreach (char letra in _char) // detecta pressionamento da tecla e atualiza o material e tag da Bin 
            {
                if (Input.GetKeyDown(letra.ToString().ToLower()))
                {
                    _bin.GetComponentInChildren<SpriteRenderer>().sprite = _changeBin(letra);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                _btnController.Pause();

            _winCondition();
        }
        private IEnumerator Spawner()
        {
            while (_gameManager.Play)
            {
                _auxSpwaner();
                yield return new WaitForSeconds(Random.Range(_spawnInterval[0], _spawnInterval[1]));
            }
        }


        private void _auxSpwaner()
        {
            _trashSpwaner.Spawn(_maxTrashTypes);
        }
        private void _rules(int index)
        {
            switch (index)
            {
                case 0: _maxTrashTypes = 5; _requiredCount = 10; _spawnInterval = new float[] { 1.8f, 2.2f }; break;
                case 1: _maxTrashTypes = 8; _requiredCount = 20; _spawnInterval = new float[] { 1.7f, 2.1f }; break;
                case 2: _maxTrashTypes = 11; _requiredCount = 30; _spawnInterval = new float[] { 1.6f, 2f }; break;
                case 3: _maxTrashTypes = 14; _requiredCount = 60; _spawnInterval = new float[] { 1.5f, 2f }; break;
                case 4: _maxTrashTypes = 19; _requiredCount = 120; _spawnInterval = new float[] { 1.4f, 2.1f }; break;
            }
        }
        private int Performace(int score)
        {
            if (score * (_gameManager.Lives + 1) == _requiredCount * 2 * 8) return 3;
            else if (score * (_gameManager.Lives + 1) >= _requiredCount * 1.7 * 5) return 2;
            else if (score * (_gameManager.Lives + 1) >= _requiredCount * 1.4 * 2) return 1;
            else return 0;
        }
        private Sprite _changeBin(char letra) // altera o current de acordo com os inputs
        {
            switch (letra)
            {
                //Default
                case 'Q': _bin.tag = "Paper"; return _binSprite[0];
                case 'W': _bin.tag = "Plastic"; return _binSprite[1];
                case 'E': _bin.tag = "Glass"; return _binSprite[2];
                case 'R': _bin.tag = "Metal"; return _binSprite[3];
                //Challenger
                case 'A': _bin.tag = "Organic"; return _binSprite[4];
                case 'S': _bin.tag = "Hospital"; return _binSprite[5];
                case 'D': _bin.tag = "Wood"; return _binSprite[6];
                case 'F': _bin.tag = "Radioative"; return _binSprite[7];
            }
            _SoundTrackManager.PlaySound("Bin");
            _bin.tag = _binSprite[0].name;
            return _binSprite[0];
        }
        private void _winCondition()
        {
            if (_gameManager.CollectedCount >= _requiredCount)
            {
                _SoundTrackManager.PlaySound("winner");
                Debug.Log($"Name: {Global.CurrentUser.UserName} Level: {Global.CurrentLevel} Score: {_gameManager.Score}");
                Global.SetLevelPerformace(Performace(_gameManager.Score));
                Debug.Log(Global.LevelPerformace);
                Global.SetCurrentScore(_gameManager.Score);
                SceneManager.LoadScene("WinScene");
            }
            if (_gameManager.Lives < 0)
            {
                _SoundTrackManager.PlaySound("lose");
                SceneManager.LoadScene("LoseScene");
            }
        }

    }
}