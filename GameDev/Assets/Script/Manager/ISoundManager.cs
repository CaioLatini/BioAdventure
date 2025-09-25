// SoundManager.cs
/*
Esse script será responsável por gerenciar e tocar todos os clipes de áudio do jogo,
separando músicas de fundo de efeitos sonoros. Contendo métodos para tocar, parar,
e ajustar o volume dos sons, sendo uma fonte central para todo o áudio do projeto.
*/



public interface ISoundManager
{
    void PlayEffect(string effectName);
    void PlayMusic(string musicName);
    void StopMusic();
    void SetMusicVolume(float volume);
    void SetEffectsVolume(float volume);
}