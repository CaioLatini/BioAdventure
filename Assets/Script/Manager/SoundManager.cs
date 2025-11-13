using UnityEngine;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using System;

// SoundManager.cs
/*
Esse script será responsável por gerenciar e tocar todos os clipes de áudio do jogo,
separando músicas de fundo de efeitos sonoros. Contendo métodos para tocar, parar,
e ajustar o volume dos sons, sendo uma fonte central para todo o áudio do projeto.
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }

            Debug.Log("SoundManager Initialized");
        }

        [Header("Biblioteca de Áudio")]
        [SerializeField] private List<AudioClip> audioClipList;

        private Dictionary<string, AudioClip> _audioLibrary;

        private AudioSource _musicSource;
        private AudioSource _effectsSource;

        private AudioClip _currentMusic;


        public float _musicVolume { get; private set; } = 0.7f;
        public float _effectsVolume { get; private set; } = 0.7f;

        private void Start()
        {
            _audioLibrary = new Dictionary<string, AudioClip>();
            foreach (var clip in audioClipList)
            {
                if (clip != null && !_audioLibrary.ContainsKey(clip.name))
                {
                    _audioLibrary.Add(clip.name, clip);
                }
            }

            SetMusicVolume(0.7f);
            SetEffectsVolume(0.7f);
        }

        private void InitializeAudioSources()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _effectsSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlayEffect(string soundName, float volumeScale = 1f)
        {
            if (_audioLibrary.TryGetValue(soundName, out AudioClip clip))
            {
                _effectsSource.PlayOneShot(clip, volumeScale);
            }
            else
            {
                Debug.LogWarning($"Efeito sonoro '{soundName}' não encontrado na biblioteca.");
            }
        }

        public void PlayMusic(string musicName, bool loop = true)
        {
            if (_audioLibrary.TryGetValue(musicName, out AudioClip clip))
            {
                if (_currentMusic == clip) return;
                else
                {
                    _currentMusic = clip;
                    _musicSource.clip = clip;
                    _musicSource.loop = loop;
                    _musicSource.Play();
                }
            }
        }
        public void RestartEffects()
        {
            _effectsSource.Stop();
            _effectsSource.Play();
        }
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = _musicVolume;
        }
        public void SetEffectsVolume(float volume)
        {
            _effectsVolume = Mathf.Clamp01(volume);
            _effectsSource.volume = _effectsVolume;
        }
    }
}