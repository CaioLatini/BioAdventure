using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BioAdventure.Assets.Script.SceneScript;
using System.Collections.Generic;

namespace BioAdventure.Assets.Script
{
    public class BtnController : MonoBehaviour
    {
        // Handles menu navigation, pause, and scene transitions

        [Header("Dependencies")]
        [SerializeField] private AuthenticationService authService;
        [SerializeField] private SoundTrackManager soundTrackManager;

        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private GameObject infoBin;
        [SerializeField] private GameObject infoBinPage2;
        [SerializeField] private GameObject infoGeneral;
        [SerializeField] private List<GameObject> infoPages;
        [SerializeField] private GameObject mutedBarEffects;
        [SerializeField] private GameObject mutedBarMusic;
        [SerializeField] private Slider effectsVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;

        private GameObject currentUI;
        private bool isPaused = false;
        private bool isMutedEffects = false;
        private bool isMutedMusic = false;
        private int infoPageIndex = 0;

        private void Awake()
        {
            if (Global.CurrentUser != null)
            {
                Global.SetVolumeEffects(Global.CurrentUser.Effects);
                Global.SetVolumeMusic(Global.CurrentUser.Music);
            }
        }

        private void Start()
        {
            effectsVolumeSlider.value = soundTrackManager.AudioSourceEffects.volume;
            musicVolumeSlider.value = soundTrackManager.AudioSourceMusic.volume;

            effectsVolumeSlider.onValueChanged.AddListener(SetVolumeEffects);
            musicVolumeSlider.onValueChanged.AddListener(SetVolumeMusic);

            if (Global.CurrentUser == null)
            {
                Global.SetVolumeEffects(0.5f);
                Global.SetVolumeMusic(0.5f);
            }

            soundTrackManager.AudioSourceEffects.volume = Global.VolumeEffects;
            soundTrackManager.AudioSourceMusic.volume = Global.VolumeMusic;

            effectsVolumeSlider.value = Global.VolumeEffects;
            musicVolumeSlider.value = Global.VolumeMusic;
        }

        private void Update()
        {
            if (!infoGeneral.activeSelf) return;

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                ShowNextInfoPage();
            }
        }

        // Volume
        private void SetVolumeEffects(float volume)
        {
            soundTrackManager.AudioSourceEffects.volume = volume;
            Global.SetVolumeEffects(volume);
            if (Global.CurrentUser != null)
                Global.CurrentUser.Effects = volume;
        }

        private void SetVolumeMusic(float volume)
        {
            soundTrackManager.AudioSourceMusic.volume = volume;
            Global.SetVolumeMusic(volume);
            if (Global.CurrentUser != null)
                Global.CurrentUser.Music = volume;
        }

        // Pause and UI
        private void TogglePause(GameObject targetUI)
        {
            if (currentUI != null)
                currentUI.SetActive(false);

            isPaused = (currentUI != targetUI) ? true : !isPaused;
            currentUI = isPaused ? targetUI : null;

            if (currentUI != null)
                currentUI.SetActive(true);

            Time.timeScale = isPaused ? 0f : 1f;
        }

        // Mute
        private void ToggleMute(bool isEffect)
        {
            if (isEffect)
            {
                isMutedEffects = !isMutedEffects;
                mutedBarEffects.SetActive(isMutedEffects);
                soundTrackManager.MuteSound(true, isMutedEffects);
            }
            else
            {
                isMutedMusic = !isMutedMusic;
                mutedBarMusic.SetActive(isMutedMusic);
                soundTrackManager.MuteSound(false, isMutedMusic);
            }
        }

        // Sound
        private void PlayClick(string soundName = "click")
        {
            Debug.Log("Playing " + soundName);
            soundTrackManager.PlaySound(soundName);
        }

        // Info Pages
        private void ShowNextInfoPage()
        {
            if (infoPageIndex > 0 && infoPageIndex <= infoPages.Count)
                infoPages[infoPageIndex - 1].SetActive(false);

            if (infoPageIndex < infoPages.Count)
            {
                infoPages[infoPageIndex].SetActive(true);
                infoPageIndex++;
            }
            else
            {
                infoPages[infoPages.Count - 1].SetActive(false);
                TogglePause(pauseCanvas);
                if (SceneManager.GetActiveScene().name.Contains("Game"))
                    TogglePause(pauseCanvas);
                infoPageIndex = 0;
            }
        }

        // Public UI Methods
        public void Quit() => Application.Quit();
        public void Pause() { PlayClick(); TogglePause(pauseCanvas); }
        public void InfoBin() { PlayClick(); TogglePause(infoBin); }
        public void InfoBinPage2() { PlayClick(); TogglePause(infoBinPage2); }
        public void MuteMusic() { PlayClick(); ToggleMute(false); }
        public void MuteEffects() { PlayClick(); ToggleMute(true); }
        public void GoHome() { PlayClick(); SceneManager.LoadScene("MainMenu"); }
        public void StartGame() { Time.timeScale = 1f; PlayClick(); SceneManager.LoadScene("GameScene"); }
        public void NextLevel() { PlayClick(); Global.ChangeLevel(true); SceneManager.LoadScene("GameScene"); }
        public void NextLevelMenu() { PlayClick("switchGo"); Global.ChangeLevel(true); }
        public void BackLevelMenu() { PlayClick("switchBack"); Global.ChangeLevel(false); }
        public void OnGitHubLink() { PlayClick(); Application.OpenURL("https://github.com/CaioLatini"); }
        public void Logout() { PlayClick(); Global.StartSession(null); SceneManager.LoadScene("Auth"); }
        public void InfoGeneral()
        {
            PlayClick();
            infoPageIndex = 0;
            foreach (var page in infoPages)
                page.SetActive(false);
            TogglePause(infoGeneral);
            infoPages[infoPageIndex].SetActive(true);
            infoPageIndex++;
        }
        public void Auth()
        {
            PlayClick();
            var (ok, user) = authService.Authenticate();
            if (ok)
            {
                Debug.Log($"{user.UserName} - {user.Password} - {user.Level1} - {user.Level2} - {user.Level3}");
                Global.StartSession(user);
                Global.SetVolumeEffects(user.Effects);
                Global.SetVolumeMusic(user.Music);
                SceneManager.LoadScene("MainMenu");
            }
        }
        public void Save()
        {
            AuthenticationService.SaveUser(Global.CurrentUser);
        }

        public bool IsPaused => isPaused;
    }
}