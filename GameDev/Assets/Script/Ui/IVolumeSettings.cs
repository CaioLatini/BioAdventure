// VolumeSettings.cs 
/*
Esse script será responsável por controlar os elementos da UI de configurações de volume.
Ele ajustará os valores dos Sliders, salvará as preferências de volume no GameManager
e silenciará/dessilenciará os canais de áudio.
*/



public interface IVolumeSettings
{
    void OnMusicVolumeChanged(float volume);
    void OnEffectsVolumeChanged(float volume);
    void OnMuteMusicButtonPressed();
    void OnMuteEffectsButtonPressed();
    void SyncUI(float musicVolume, float effectsVolume);
}
