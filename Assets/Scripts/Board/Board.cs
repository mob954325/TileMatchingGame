using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    private List<Block_Normal> blockList;

    public GameObject blockPrefab;

    /// <summary>
    /// 보드 가로칸 개수
    /// </summary>
    private int size_X = 4;
    /// <summary>
    /// 보드 세로칸 개수
    /// </summary>
    private int size_Y = 4;

    /// <summary>
    /// size_X * size_Y 값 ( awake에서 초기화됨 )
    /// </summary>
    private int capacity = -1;

    public void Init()
    {
        capacity = size_X * size_Y;
        blockList = new List<Block_Normal>(capacity);

        for (int i = 0; i < size_X * size_Y; i++)
        {
            // 블록 오브젝트 생성
            GameObject obj = Instantiate(blockPrefab);
            Block_Normal block = obj.GetComponent<Block_Normal>();
            blockList.Add(block);

            int colorRand = UnityEngine.Random.Range(0, Enum.GetValues(typeof(BlockColor)).Length);
            block.SetColor((BlockColor)colorRand);

            // 위치로 이동
            int pos_X = i % size_X;
            int pos_Y = i / size_X;

            obj.name = $"{pos_X}, {pos_Y}";

            block.MoveObject(pos_X, pos_Y);

            block.SlideFunction.OnMove_Right += () => { SwapBlockPosition(block, MoveDirection.Right); CheckTileMatch(); };
            block.SlideFunction.OnMove_Left += () => { SwapBlockPosition(block, MoveDirection.Left); CheckTileMatch(); };
            block.SlideFunction.OnMove_Up += () => { SwapBlockPosition(block, MoveDirection.Up); CheckTileMatch(); };
            block.SlideFunction.OnMove_Down += () => { SwapBlockPosition(block, MoveDirection.Down); CheckTileMatch(); };
        }
    }

    private void CheckTileMatch()
    {
        // 모든 블록 탐색
        // 블록중 가로나 새로로 같은 색깔의 블록이 있으면 해당 블록 제거
        //  어떻게 찾지
        // 제거되면 제거된 만큼 새로운 블록 생성

        List<Block_Normal> removeList = new List<Block_Normal>(capacity);
        Queue<Block_Normal> checkQueue = new Queue<Block_Normal>(capacity);
        List<Vector2> dirList = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        int count = 0; // 루프 방지용 변수

        for(int i = 0; i < capacity; i++)
        {
            checkQueue.Enqueue(blockList[i]);
        }

        //while (checkedList.Count < capacity)
        while (checkQueue.Count > 0)
        {
            if (count > int.MaxValue) // 무한 루프 방지
            {
                Debug.Log("범위 초과");
                break;
            }

            Block_Normal curBlock = checkQueue.Peek();            
            checkQueue.Dequeue();

            if(curBlock != null)
            {
                BlockColor curBlockColor = curBlock.BlockColor; // 비교할 블록 색
                for(int i = 0; i < 4; i++) // 4방향 확인
                {
                    List<Block_Normal> addList = new List<Block_Normal>(capacity);
                    Vector2 nextVec = curBlock.GridPos + dirList[i];
                    int sameCount = 0;

                    if (!IsValidPosition(nextVec)) continue; // 존재하지 않는 위치면 무시
                    else
                    {
                        Block_Normal check = FindBlock((int)nextVec.x, (int)nextVec.y);
                        if (check == null) break; // 잘못된 위치면 무시

                        int remain = 0;

                        if (i < 2) // 가로, 세로 확인인지 확인
                        {
                            remain = size_X - (int)nextVec.x;
                        }
                        else
                        {
                            remain = size_Y - (int)nextVec.y;
                        }

                        // 같은 블록 찾기
                        for(int j = 0; j <= remain; j++)
                        {
                            
                            if (check.BlockColor != curBlockColor) break;
                            else
                            {
                                check = FindBlock((int)nextVec.x, (int)nextVec.y);
                                if (check == null) break; // 잘못된 위치면 무시
                                else
                                {
                                    addList.Add(check);
                                    sameCount++;
                                    nextVec += dirList[i];
                                }
                            }
                        } // for (같은 블록 찾기)

                        if(sameCount > 2)
                        {
                            for(int j = 0; j < addList.Count; j++)
                            {
                                removeList.Add(addList[j]);
                            }
                        }
                    }
                } // for (4방향)
            }
            else
            {
                continue;
            }

            count++;
        } // while ( 모든 블록 체크 )

        // 블록 제거
        for (int i = 0; i < removeList.Count; i++)
        {
            GameObject obj = removeList[i].gameObject;
            Block_Normal block = blockList.Find(x => x.GridPos == removeList[i].GridPos);
            blockList.Remove(block);
            Destroy(obj); 
        }

        // 블록 내리기
        for(int x = 0; x < size_X; x++)
        {
            for(int y = 0; y < size_Y; y++)
            {
                if (FindBlock(x, y) != null) continue;

                for(int i = y + 1; i < size_Y; i++)
                {
                    Block_Normal block = FindBlock(x, i);
                    if(block != null)
                    {
                        block.MoveObject(MoveDirection.Down);
                    }
                }
            }
        }

        // 블록 추가
        for (int x = 0; x < size_X; x++)
        {
            int emptyCount = 0;
            for (int y = 0; y < size_Y; y++)
            {
                Block_Normal block = FindBlock(x, y);
                if (block == null)
                {
                    // 오브젝트 생성
                    GameObject obj = Instantiate(blockPrefab);
                    Block_Normal createdBlock = obj.GetComponent<Block_Normal>();
                    blockList.Add(createdBlock);
                    createdBlock.MoveToCoord(new Vector2Int(x, y));

                    int colorRand = UnityEngine.Random.Range(0, Enum.GetValues(typeof(BlockColor)).Length);
                    createdBlock.SetColor((BlockColor)colorRand);

                    obj.name = $"{x}, {y}";

                    createdBlock.SlideFunction.OnMove_Right += () => { SwapBlockPosition(createdBlock, MoveDirection.Right); CheckTileMatch(); };
                    createdBlock.SlideFunction.OnMove_Left += () => { SwapBlockPosition(createdBlock, MoveDirection.Left); CheckTileMatch(); };
                    createdBlock.SlideFunction.OnMove_Up += () => { SwapBlockPosition(createdBlock, MoveDirection.Up); CheckTileMatch(); };
                    createdBlock.SlideFunction.OnMove_Down += () => { SwapBlockPosition(createdBlock, MoveDirection.Down); CheckTileMatch(); };
                }
            }
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

    public Block_Normal FindBlock(Vector2 grid)
    {
        return FindBlock((int)grid.x, (int)grid.y);
    }

    public Block_Normal FindBlock(int grid_X, int grid_Y)
    {
        if (!IsValidPosition(grid_X, grid_Y)) return null;

        Block_Normal block = blockList.Find(x => x.GridPos == new Vector2(grid_X, grid_Y));

        return block;
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