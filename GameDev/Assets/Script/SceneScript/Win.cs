using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BioAdventure.Assets.Script.SceneScript
{
    public class Win : MonoBehaviour
    {
        [Header("Dependences")]
        [SerializeField] private SoundTrackManager _soundTrackManager;
        [SerializeField] private List<GameObject> _stars;
        [SerializeField] private List<Sprite> _backGroundCollection;
        [SerializeField] private GameObject _backGround;
        [SerializeField] private GameObject _score;
        private void Awake()
        {
            GlobalServices.SaveProgress();
            _backGround.GetComponent<Image>().sprite = _backGroundCollection[Global.CurrentLevel];
        }
        private void Start()
        {
            _score.GetComponent<TMP_Text>().text = Global.CurrentScore;
            StartCoroutine(_performace(Global.LevelPerformace));
        }
        private IEnumerator _performace(int result)
        {
            for (int x = 0; x < result && x < 3; x++)
            {
                _stars[x].GetComponent<Image>().color = Color.green;
                _soundTrackManager.PlaySound("scorePoint");
                yield return new WaitForSeconds(0.5f);
            }
            if (result == 3)
            {
                _soundTrackManager.PlaySound("perfect");
                yield return new WaitForSeconds(1.5f);
                foreach (GameObject star in _stars)
                {
                    star.GetComponent<Image>().color = Color.yellow;
                }
            }
        }

    }
}