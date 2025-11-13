using UnityEngine;
using UnityEngine.UI;
using BioAdventure.Assets.Script.Managers;

// VolumeSettings.cs 
/*
Esse script será responsável por controlar os elementos da UI de configurações de volume.
Ele ajustará os valores dos Sliders, salvará as preferências de volume no GameManager
e silenciará/dessilenciará os canais de áudio.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class VolumeUI : MonoBehaviour
    {
        [Header("Referências da UI")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider effectsVolumeSlider;
        [SerializeField] private GameObject musicMutedIcon;
        [SerializeField] private GameObject effectsMutedIcon;

        private bool _isMusicMuted = false;
        private bool _isEffectsMuted = false;

        private void Start()
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);

            SyncUI();
            
        }

        public void OnMusicVolumeChanged(float volume)
        {
            if (_isMusicMuted)
            {
                _isMusicMuted = false;
                UpdateMuteIcons();
            }
            if(volume == 0f)
            {
                _isMusicMuted = true;
                UpdateMuteIcons();
            }
            SoundManager.Instance.SetMusicVolume(volume);
        }

        public void OnEffectsVolumeChanged(float volume)
        {
            if (_isEffectsMuted)
            {
                _isEffectsMuted = false;
                UpdateMuteIcons();
            }
            if(volume == 0f)
            {
                _isEffectsMuted = true;
                UpdateMuteIcons();
            }
            SoundManager.Instance.SetEffectsVolume(volume);
        }

        public void OnMuteMusicButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            _isMusicMuted = !_isMusicMuted;
            float targetVolume = _isMusicMuted ? 0f : musicVolumeSlider.value;
            SoundManager.Instance.SetMusicVolume(targetVolume);

            UpdateMuteIcons();
        }

        public void OnMuteEffectsButtonPressed()
        {
            SoundManager.Instance.PlayEffect("click");

            _isEffectsMuted = !_isEffectsMuted;
            float targetVolume = _isEffectsMuted ? 0f : effectsVolumeSlider.value;
            SoundManager.Instance.SetEffectsVolume(targetVolume);

            UpdateMuteIcons();
        }

        public void SyncUI()
        {
            musicVolumeSlider.value = SoundManager.Instance._musicVolume;
            effectsVolumeSlider.value = SoundManager.Instance._effectsVolume;
        }

        private void UpdateMuteIcons()
        {
            if (musicMutedIcon != null)
            {
                musicMutedIcon.SetActive(_isMusicMuted);
            }
            if (effectsMutedIcon != null)
            {
                effectsMutedIcon.SetActive(_isEffectsMuted);
            }
        }
    }
}