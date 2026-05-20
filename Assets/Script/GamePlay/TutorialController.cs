using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.UI;

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TutorialController : MonoBehaviour
    {

        [SerializeField] private GameUI _gameUI;
        [SerializeField] private BinAssistence _binAssistence;
        [SerializeField] private Animator _cursorAnimator;
        [SerializeField] private List<Animator> _circleBin = new List<Animator>();
        [SerializeField] private List<Animator> _circleTrash = new List<Animator>();

        private int _position;
        private string _material;

        private void OnEnable()
        {
            TrashItem.onTutorial += CaptureAssistence;

        }

        private void OnDisable()
        {
            TrashItem.onTutorial -= CaptureAssistence;

        }

        public void TutorialStep(string Step)
        {
            if (Step.Contains("Circular"))
            {
                Debug.LogWarning("Animação circular...");
                for (int i = 0; i < _circleBin.Count; i++)
                {
                    _circleTrash[i].Play(Step);
                    _circleBin[i].Play(Step);
                }
                CaptureAssistence();
            }
            else
            {
                _cursorAnimator.gameObject.SetActive(true);
                _cursorAnimator.Play(Step);
            }
        }

        public void SetSettings(int trashPosition)
        {
            Debug.Log("A posicao do lixo é: " + trashPosition);
            _position = trashPosition;
        }

        public void SetSettings(string material)
        {
            Debug.Log("O mateiral do tutorial é: " + material);
            _material = material;
        }

        private void CaptureAssistence()
        {
            Time.timeScale = 0f;

            _circleTrash[_position].gameObject.SetActive(true);

            GameObject bin = GameObject.Find(_material);
            Debug.LogWarning("Material: "+_material);
            if (bin == null) return;

            for (int i = 0; i < _binAssistence.Spot.Length; i++)
            {
                if (Vector2.Distance(bin.transform.position, _binAssistence.Spot[i].position) < 0.1f)
                {
                    _circleBin[i].gameObject.SetActive(true);
                }
            }

            StartCoroutine(ShowTutorialRoutine());
        }

        private IEnumerator ShowTutorialRoutine()
        {
            Debug.LogWarning("Chamando tutorial text");
            int index = GameManager.Instance.CurrentUser.Lenguage ? 1 : 0;
            yield return StartCoroutine(_gameUI.ShowTutorial(index));
        }

        public void CheckTutCapture(int positionBin, string name)
        {
            if (positionBin == _position && name.Contains(_material))
            {
                FinalizarTutorial(2);
                Time.timeScale = 1f;
            }
            else return;
        }


        public void FinalizarTutorial(int Tutorial)
        {
            GameManager.Instance.SetTutorialComplete(Tutorial);

            _cursorAnimator.gameObject.SetActive(false);
            if (_circleTrash.Count > 1)
            {
                for (int i = 0; i < _circleBin.Count; i++)
                {
                    _circleTrash[i].gameObject.SetActive(false);
                    _circleBin[i].gameObject.SetActive(false);
                }
            }

            // Inicia a contagem do jogo após o tutorial
            if (Tutorial == 1) GameController.Instance.StartCoroutine(GameController.Instance.StartCountDown());
        }
    }
}