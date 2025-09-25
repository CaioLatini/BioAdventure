// AuthUI.cs
/*
Esse script será responsável por gerenciar a interface da cena de autenticação.
Ele capturará os dados dos InputFields, exibirá mensagens de status ao usuário
e chamará os sistemas de autenticação e salvamento quando o botão de login for pressionado.
*/



public interface IAuthMenu
{
    void ShowStatusMessage(string message);
    void ClearFields();
    void OnLoginButtonPressed();
    void OnRegisterButtonPressed();
}


//Existe a ideia de remover o register e manter apenas name, associando o progresso local ao ususario
