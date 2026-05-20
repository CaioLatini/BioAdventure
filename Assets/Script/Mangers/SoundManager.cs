using UnityEngine;
using System.Collections.Generic;

// SoundManager.cs
/*
Gerencia e reproduz todos os clipes de áudio do jogo.
Separa músicas de fundo (BGM) de efeitos sonoros (SFX).
Atua como um Singleton central para controle de volume e reprodução em qualquer cena.
*/

namespace BioAdventure.Assets.Script.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Biblioteca de Áudio")]
        [Tooltip("Arraste todos os arquivos de áudio do jogo para cá. O nome do arquivo será usado para tocá-lo via script.")]
        [SerializeField] private List<AudioClip> audioClipList;

        // Dicionário para busca rápida (O(1)) de clipes pelo nome
        private Dictionary<string, AudioClip> _audioLibrary;

        // Canais de áudio separados
        private AudioSource _musicSource;
        private AudioSource _effectsSource;

        private AudioClip _currentMusic;

        [Tooltip("Volume atual da música (0 a 1).")]
        public float _musicVolume { get; private set; } = 0.7f;
        
        [Tooltip("Volume atual dos efeitos sonoros (0 a 1).")]
        public float _effectsVolume { get; private set; } = 0.7f;

        // Configura o Singleton e cria os canais de áudio
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
        }

        // Inicializa a biblioteca convertendo a lista para dicionário e aplica volumes base
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

        // Adiciona dinamicamente os componentes AudioSource ao GameObject do Manager
        private void InitializeAudioSources()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _effectsSource = gameObject.AddComponent<AudioSource>();
        }

        // Toca um efeito sonoro uma única vez (permite sobreposição de vários sons juntos)
        public void PlayEffect(string soundName, float volumeScale = 1f)
        {
            if (_audioLibrary.TryGetValue(soundName, out AudioClip clip))
            {
                _effectsSource.PlayOneShot(clip, volumeScale);
            }
            else
            {
                Debug.LogWarning($"[SoundManager] Efeito sonoro '{soundName}' não encontrado na biblioteca.");
            }
        }

        // Toca uma música de fundo em loop. Substitui a música atual se for diferente.
        public void PlayMusic(string musicName, bool loop = true)
        {
            if (_audioLibrary.TryGetValue(musicName, out AudioClip clip))
            {
                if (_currentMusic == clip) return; // Evita reiniciar a música se já estiver tocando
                
                _currentMusic = clip;
                _musicSource.clip = clip;
                _musicSource.loop = loop;
                _musicSource.Play();
            }
            else
            {
                Debug.LogWarning($"[SoundManager] Música '{musicName}' não encontrada na biblioteca.");
            }
        }

        // Interrompe os efeitos sonoros sendo tocados no canal principal de SFX
        public void RestartEffects()
        {
            _effectsSource.Stop();
            _effectsSource.Play(); 
        }

        // Define o volume da música e aplica ao componente
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            if (_musicSource != null) _musicSource.volume = _musicVolume;
        }

        // Define o volume dos efeitos sonoros e aplica ao componente
        public void SetEffectsVolume(float volume)
        {
            _effectsVolume = Mathf.Clamp01(volume);
            if (_effectsSource != null) _effectsSource.volume = _effectsVolume;
        }
    }
}