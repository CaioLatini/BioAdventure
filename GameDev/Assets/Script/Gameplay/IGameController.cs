// GameController.cs
/*
Esse script será responsável por orquestrar a lógica principal da cena de gameplay.
Ele utilizará um dos LevelConfig para definir as regras da fase,
iniciará o spawner de lixo e monitorará as condições de vitória e derrota.
*/



public interface IGameController
{
    void InitializeLevel(LevelConfig config);
    void StartGame();
    void PauseGame();
    void ResumeGame();

}