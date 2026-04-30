using UnityEngine;
using UnityEngine.UI;
using BioAdventure.Assets.Script.Managers;

// VolumeUI.cs
/*
Controla os sliders e botões de Mute na interface de configurações.
Sincroniza os valores visuais, atualiza o volume em tempo real e agora
gerencia a troca de ícones (Sprites) quando o som é mutado manualmente ou pelo slider.
*/

namespace BioAdventure.Assets.Script.UI
{
    public class VolumeUI : MonoBehaviour
    {
        [Header("Referências da UI - Sliders")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider effectsSlider;

        [Header("Referências da UI - Ícones de Mute")]
        [Tooltip("O gameobject que fica dentro do botão de Mute da Música.")]
        [SerializeField] private GameObject musicMuteIcon;
        [Tooltip("O gameobject que fica dentro do botão de Mute dos Efeitos.")]
        [SerializeField] private GameObject effectsMuteIcon;


        private float _lastMusicVolume = 0.7f;
        private float _lastEffectsVolume = 0.7f;
        
        private bool _isMusicMuted = false;
        private bool _isEffectsMuted = false;

        private void Start()
        {
            if (SoundManager.Instance == null) return;

            if (musicSlider != null)
            {
                musicSlider.value = SoundManager.Instance._musicVolume;
                _isMusicMuted = (musicSlider.value <= 0f);
                musicSlider.onValueChanged.AddListener(SetMusicVolume);
            }

            if (effectsSlider != null)
            {
                effectsSlider.value = SoundManager.Instance._effectsVolume;
                _isEffectsMuted = (effectsSlider.value <= 0f);
                effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
            }

            UpdateMusicIcon();
            UpdateEffectsIcon();
        }

        private void OnDestroy()
        {
            if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            if (effectsSlider != null) effectsSlider.onValueChanged.RemoveListener(SetEffectsVolume);
        }

        #region Controle por Sliders

        public void SetMusicVolume(float volume)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetMusicVolume(volume);
                
                // Se chegou a zero, muta. Se for maior que zero, desmuta.
                _isMusicMuted = (volume <= 0f);
                UpdateMusicIcon();
            }
        }

        public void SetEffectsVolume(float volume)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetEffectsVolume(volume);
                
                _isEffectsMuted = (volume <= 0f);
                UpdateEffectsIcon();
            }
        }

        #endregion

        #region Controle por Botões (Mute)

        public void OnMuteMusicButtonPressed()
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");

            _isMusicMuted = !_isMusicMuted;

            if (_isMusicMuted)
            {
                _lastMusicVolume = musicSlider != null && musicSlider.value > 0f ? musicSlider.value : SoundManager.Instance._musicVolume;
                
                if (musicSlider != null) musicSlider.value = 0f;
                else SoundManager.Instance.SetMusicVolume(0f);
            }
            else
            {
                float volumeToRestore = _lastMusicVolume > 0f ? _lastMusicVolume : 0.7f;
                if (musicSlider != null) musicSlider.value = volumeToRestore;
                else SoundManager.Instance.SetMusicVolume(volumeToRestore);
            }

            UpdateMusicIcon();
        }

        public void OnMuteEffectsButtonPressed()
        {
            if (SoundManager.Instance != null && !_isEffectsMuted) SoundManager.Instance.PlayEffect("click");

            _isEffectsMuted = !_isEffectsMuted;

            if (_isEffectsMuted)
            {
                _lastEffectsVolume = effectsSlider != null && effectsSlider.value > 0f ? effectsSlider.value : SoundManager.Instance._effectsVolume;
                
                if (effectsSlider != null) effectsSlider.value = 0f;
                else SoundManager.Instance.SetEffectsVolume(0f);
            }
            else
            {
                float volumeToRestore = _lastEffectsVolume > 0f ? _lastEffectsVolume : 0.7f;
                if (effectsSlider != null) effectsSlider.value = volumeToRestore;
                else SoundManager.Instance.SetEffectsVolume(volumeToRestore);
                
                if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("click");
            }

            UpdateEffectsIcon();
        }

        #endregion

        #region Atualização Visual

        // Troca a imagem do botão de Música dependendo do status
        private void UpdateMusicIcon()
        {
            if (musicMuteIcon != null)
            {
                musicMuteIcon.SetActive(_isMusicMuted);
            }
        }

        // Troca a imagem do botão de Efeitos dependendo do status
        private void UpdateEffectsIcon()
        {
            if (effectsMuteIcon != null)
            {
                effectsMuteIcon.SetActive(_isEffectsMuted);
            }
        }

        #endregion
    }
}