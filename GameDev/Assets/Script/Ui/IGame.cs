// GameUI.cs
/*
Esse script será responsável por atualizar a interface do usuário (HUD) durante o gameplay.
Sua função é exibir informações como a pontuação atual e o número de vidas restantes.
*/



public interface IGame
{
    void UpdateScore(int score);
    void UpdateLives(int lives);
    void ShowContDown();
    void HideContDown();
}