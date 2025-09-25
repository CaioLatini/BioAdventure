// PauseMenu.cs 
/*
Esse script será responsável por gerenciar o menu de pausa.
Ele controlará a visibilidade do painel de pausa e pausará/despausará o jogo
*/




public interface IPauseMenu
{
    void ShowPauseMenu();
    void HidePauseMenu();
    void MainMenuButton();
    void GitHubButton();
    void LogoutButton();
    void RestartButton();
    void ControlButton();
    void HelpButton();
}