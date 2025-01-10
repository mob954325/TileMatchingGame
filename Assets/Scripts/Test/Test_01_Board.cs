using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_Board : TestBase
{
    public Board board;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        board.Init();
    }
}