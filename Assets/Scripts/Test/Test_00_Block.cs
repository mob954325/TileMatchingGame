using UnityEngine;
using UnityEngine.InputSystem;

public class Test_00_Block : TestBase
{
    public Block_Normal block;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        block.MoveObject(Vector2Int.left);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        block.MoveObject(Vector2Int.right);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        block.MoveObject(Vector2Int.up);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        block.MoveObject(Vector2Int.down);
    }
}
