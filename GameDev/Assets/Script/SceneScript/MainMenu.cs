using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BioAdventure.Assets.Script.SceneScript
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Dependences")]
        [SerializeField] private SoundTrackManager _soundTrackManager;
        [SerializeField] private BtnController _btnController;

        [SerializeField] private List<Sprite> _winBackGroundCollection;
        [SerializeField] private List<Sprite> _loseBackGroundCollection;
        [SerializeField] private GameObject _backGround;
        [SerializeField] private TMP_Text _bestScore;
        private int _lastLevel = -1;
        private void Update()
        {
            if (Global.CurrentLevel != _lastLevel)
            {
                bool isUnlocked = GlobalServices.IsLevelUnlocked(Global.CurrentLevel);
                _backGround.GetComponent<Image>().sprite = isUnlocked ?
                    _winBackGroundCollection[Global.CurrentLevel] :
                    _loseBackGroundCollection[Global.CurrentLevel];

                _bestScore.text = BestScore();
                _lastLevel = Global.CurrentLevel;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                _btnController.Pause();
        }
        private string BestScore()
        {
            switch (Global.CurrentLevel)
            {
                case 0:
                    return Global.CurrentUser.Level1.ToString();
                case 1:
                    return Global.CurrentUser.Level2.ToString();
                case 2:
                    return Global.CurrentUser.Level3.ToString();
                case 3:
                    return Global.CurrentUser.Level4.ToString();
                case 4:
                    return Global.CurrentUser.Level5.ToString();
                default:
                    return "0";
            }
        }
    }
}