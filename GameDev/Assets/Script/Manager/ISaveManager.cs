// SaveManager.cs
/*
Esse script será responsável por toda a lógica de manipulação de arquivos.
Sua única função é salvar e carregar os dados do jogo em formato JSON. 
Ele abstrai as operações de Input/Output do resto do sistema.
*/

public interface ISaveManager
{
    List<UserSaveData> LoadAllUsers();
    void SaveUser(UserSaveData userToSave);
}