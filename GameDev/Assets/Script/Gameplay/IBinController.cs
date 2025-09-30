// BinController.cs
/*
Esse script será responsável por controlar o comportamento da lixeira (bin).
Movimentação, a mecânica de boost e a alteração de seu tipo com base no input do jogador.
*/



public interface IBinController
{
    void ChangeBinType(char charType);
    void HandleMovement();
    void HandleBoost();
    void BoostRoutine();
}