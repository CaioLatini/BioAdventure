using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BioAdventure.Assets.Script
{
    public class SoundTrackManager : MonoBehaviour
    {
        public List<AudioClip> audioLibrary;

        public AudioSource AudioSourceEffects;
        public AudioSource AudioSourceMusic;

        private void Awake()
        {
            AudioSourceEffects = gameObject.AddComponent<AudioSource>();
            AudioSourceMusic = gameObject.AddComponent<AudioSource>();

            if (SceneManager.GetActiveScene().name.Contains("Menu") || SceneManager.GetActiveScene().name.Contains("Auth"))
                PlaySound("backGround", true);
            if (SceneManager.GetActiveScene().name.Contains("Game"))
                PlaySound("Game", true);
        }

        public void PlaySound(string name)
        {
            foreach (var clip in audioLibrary)
            {
                if (clip != null && clip.name.ToLower().Contains(name.ToLower()))
                {
                    AudioSourceEffects.loop = false;
                    AudioSourceEffects.clip = clip;
                    AudioSourceEffects.Play();
                    return;
                }
            }
            Debug.LogWarning($"Som com nome '{name}' não encontrado.");
        }
        public void PlaySound(string name, bool loop)
        {
            foreach (var clip in audioLibrary)
            {
                if (clip != null && clip.name.ToLower().Contains(name.ToLower()))
                {
                    AudioSourceMusic.loop = loop;
                    AudioSourceMusic.clip = clip;
                    AudioSourceMusic.Play();
                    return;
                }
            }
            Debug.LogWarning($"Som com nome '{name}' não encontrado.");
        }
        public void MuteSound(bool tipe, bool status) // true effects false music - true mute false desmute 
        {
            if (tipe)
                AudioSourceEffects.mute = status;
            else AudioSourceMusic.mute = status;
        }
    }
}
