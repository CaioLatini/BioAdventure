using UnityEngine;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Managers;

namespace BioAdventure.Assets.Script.Core
{
    public class UiManager : MonoBehaviour
    {

        //true = en - false = pt
        private bool _lenguage;
        private List<GameObject> _textPT;
        private List<GameObject> _textEN;

        void Start()
        {
            _lenguage = GameManager.Instance.CurrentUser.Lenguage;
            CaptureText();
            SetLenguage();
        }

        private void CaptureText()
        {
            _textPT = new List<GameObject>();
            _textEN = new List<GameObject>();

            Transform[] allTransformObjects = FindObjectsOfType<Transform>(true);
            
            foreach (Transform t in allTransformObjects)
            {
                if (t.name == "pt-br") _textPT.Add(t.gameObject);
                else if (t.name == "en-us") _textEN.Add(t.gameObject);
            }
        }

        private void SetLenguage()
        {
            if(_lenguage)
            {
                for(int i = 0; _textEN.Count > i; i++)
                {
                    _textPT[i].SetActive(false);
                    _textEN[i].SetActive(true);
                }
            } else
            {
                for(int i = 0; _textPT.Count > i; i++)
                {
                    _textEN[i].SetActive(false);
                    _textPT[i].SetActive(true);
                }
            }

            GameManager.Instance.SetLenguage(_lenguage);
        }

        public void OnChangeLenguagePressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            _lenguage = !_lenguage;
            SetLenguage();
        }

    }
}
