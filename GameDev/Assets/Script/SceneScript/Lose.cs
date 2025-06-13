using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioAdventure.Assets.Script.SceneScript
{
    public class Lose : MonoBehaviour
    {
        [Header("Dependences")]
        [SerializeField] private SoundTrackManager _soundTrackManager;
        [SerializeField] private List<Sprite> _backGroundCollection;
        [SerializeField] private GameObject _backGround;
        private void Awake()
        {
            try
            { _backGround.GetComponent<Image>().sprite = _backGroundCollection[Global.CurrentLevel]; }
            catch
            { _backGround.GetComponent<SpriteRenderer>().sprite = _backGroundCollection[Global.CurrentLevel]; }
        }
    }
}