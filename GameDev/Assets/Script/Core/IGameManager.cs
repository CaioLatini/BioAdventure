using BioAdventure.Assets.Script;

/*
Esta interface define o "contrato" para qualquer classe que gerencie o estado do jogo.
Ela garante que qualquer GameManager terá as propriedades e métodos essenciais para
o funcionamento do jogo, permitindo que outros scripts dependam da interface (abstração)
em vez da implementação concreta (GameManager). Isso facilita a criação de testes e aumenta o desacoplamento.
*/



public interface IGameManager
{
    // --- PROPRIEDADES ---
    UserSaveData CurrentUser { get; }
    int CurrentLevel { get; }
    string CurrentScore { get; }
    float VolumeEffects { get; }
    float VolumeMusic { get; }

    // --- MÉTODOS ---
    void StartSession(UserSaveData user);
    void ChangeLevel(bool operacao);
    bool UnlockedLevel(int levelIndex);
    void SetCurrentScore(int score);
    void SaveProgress();
    void SetVolumeEffects(float effects);
    void SetVolumeMusic(float music);
}