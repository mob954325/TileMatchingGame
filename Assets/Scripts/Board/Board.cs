using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private List<Block_Normal> blockList;

    public GameObject blockPrefab;

    /// <summary>
    /// 보드 가로칸 개수
    /// </summary>
    private int size_X = 6;
    /// <summary>
    /// 보드 세로칸 개수
    /// </summary>
    private int size_Y = 12;

    /// <summary>
    /// size_X * size_Y 값 ( awake에서 초기화됨 )
    /// </summary>
    private int capacity = -1;

    private void Awake()
    {
        capacity = size_X * size_Y;
        blockList = new List<Block_Normal>(capacity);

        for(int i = 0; i < size_X * size_Y; i++)
        {
            // 블록 오브젝트 생성
            GameObject obj = Instantiate(blockPrefab);
            Block_Normal block = obj.GetComponent<Block_Normal>();
            blockList.Add(block);

            // 위치로 이동
            int pos_X = i % size_X;
            int pos_Y = i / size_X;

            block.MoveObject(pos_X, pos_Y);

            block.SlideFunction.OnMove_Right += () => { SwapBlockPosition(block, MoveDirection.Right); };
            block.SlideFunction.OnMove_Left += () => { SwapBlockPosition(block, MoveDirection.Left); };
            block.SlideFunction.OnMove_Up += () => { SwapBlockPosition(block, MoveDirection.Up); };
            block.SlideFunction.OnMove_Down += () => { SwapBlockPosition(block, MoveDirection.Down); };
        }
    }

    private void SwapBlockPosition(Block_Normal curBlock, MoveDirection dirType)
    {
        int curBlock_x = curBlock.GridPos.x;
        int curBlock_y = curBlock.GridPos.y;

        Block_Normal targetBlock = null;

        if(dirType == MoveDirection.Right) // Right
        {
            targetBlock = FindBlock(curBlock_x + 1, curBlock_y);
            if (targetBlock == null || !IsValidPosition(curBlock_x + 1, curBlock_y)) return; // 유효한 블록이 없거나 보드 밖이면 무시

            curBlock.MoveObject(dirType);
            targetBlock.MoveObject(MoveDirection.Left);
        }
        else if(dirType == MoveDirection.Left) // Left
        {
            targetBlock = FindBlock(curBlock_x - 1, curBlock_y);
            if (targetBlock == null || !IsValidPosition(curBlock_x - 1, curBlock_y)) return;

            curBlock.MoveObject(dirType);
            targetBlock.MoveObject(MoveDirection.Right);
        }
        else if(dirType == MoveDirection.Up) // Up
        {
            targetBlock = FindBlock(curBlock_x, curBlock_y + 1);
            if (targetBlock == null || !IsValidPosition(curBlock_x, curBlock_y + 1)) return;
            
            curBlock.MoveObject(dirType);
            targetBlock.MoveObject(MoveDirection.Down);
        }
        else if(dirType == MoveDirection.Down) // Down
        {
            targetBlock = FindBlock(curBlock_x, curBlock_y - 1);
            if (targetBlock == null || !IsValidPosition(curBlock_x, curBlock_y - 1)) return;

            curBlock.MoveObject(dirType);
            targetBlock.MoveObject(MoveDirection.Up);
        }
    }

    public Block_Normal FindBlock(int grid_X, int grid_Y)
    {
        for(int i = 0; i < capacity; i++)
        {
            Block_Normal curBlock = blockList[i];

            int x = curBlock.GridPos.x;
            int y = curBlock.GridPos.y;

            if(grid_X == x && grid_Y == y)
            {
                return curBlock;
            }
        }

        return null;
    }

    private bool IsValidPosition(Vector2 position)
    {
        return position.x >= 0 && position.x < size_X && position.y >= 0 && position.y < size_Y;
    }

    private bool IsValidPosition(int x, int y)
    {
        return IsValidPosition(new Vector2(x, y));
    }
}