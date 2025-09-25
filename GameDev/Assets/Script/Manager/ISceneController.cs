// SceneController.cs
/*
Esse script será responsável pelo gerenciamento e navegação de cenas.
Ele conterá métodos públicos de navegação que serão chamados pelos botões da UI. 
*/



public interface IScaneController
{
    void GoToMainMenu();
    void GoToGame();
    void GoToSettings();
    void GoToNextLevel();
    void GoToAuthMenu();
    void QuitGame();
}