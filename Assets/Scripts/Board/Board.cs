using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 블록 저장 배열 [y, x]
    /// </summary>
    private List<Block_Normal> blockList;

    public Action<float> onGetScore;

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

    public float SingleBlockScore = 50;

    /// <summary>
    /// 초기화 확인 여부
    /// </summary>
    private bool isInit = false;

    private void Awake()
    {
        isInit = false;
    }

    public void Init()
    {
        capacity = size_X * size_Y;
        blockList = new List<Block_Normal>();

        for (int i = 0; i < size_X * size_Y; i++)
        {
            // 블록 오브젝트 생성

            int pos_X = i % size_X;
            int pos_Y = i / size_X;

            SpawnBlock(new Vector2 (pos_X, pos_Y));
        }

        isInit = true;
    }

    private IEnumerator SpawnDelayProcess()
    {
        float timeElapsed = 0f;

        while(timeElapsed < 0.7f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (blockList.Count == size_X * size_Y) CheckTileMatch();
    }
    private void CheckTileMatch()
    {
        // 모든 블록 탐색
        // 블록중 가로나 새로로 같은 색깔의 블록이 있으면 해당 블록 제거
        // 제거되면 제거된 만큼 새로운 블록 생성

        List<Block_Normal> removeList = new List<Block_Normal>(capacity);
        List<Vector2> dirList = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) }; // 이동방향


        for(int y = 0; y < size_Y; y++)
        {
            for(int x = 0; x < size_X; x++)
            {
                Block_Normal block = FindBlock(x, y);
                if (removeList.Contains(block)) continue; // 이미 제거 목록에 있다.

                List<Block_Normal> result = FloodFill(new Vector2Int(x, y), block.BlockColor);

                if (result.Count < 3) continue;         // 3개 미만이다
                for(int i = 0; i < result.Count; i++)   // 제거 리스트 추가
                {
                    if (removeList.Contains(result[i])) continue; // 중복 제거

                    removeList.Add(result[i]);
                }
            }
        }

        AddScoreAtList(removeList);
        AllBlockPullDown();
        FillBoard();
    }

    // CheckTileMatch 관련 함수 ====================================================================

    private List<Block_Normal> FloodFill(Vector2Int startCoord,BlockColor targetColor)
    {
        // Flood Fill 알고리즘 사용
        // DFS
        // 4방향 체크, targetColor 값을 가진 블록을 찾고 3개 이상 연속되면 반환 값에 넣기

        List<Block_Normal> result = new List<Block_Normal>(capacity);
        List<Block_Normal> vertialResult = new List<Block_Normal>(size_Y);
        List<Block_Normal> horizonResult = new List<Block_Normal>(size_X);
        Stack<Block_Normal> s = new Stack<Block_Normal>(capacity);
        Block_Normal startBlock = FindBlock(startCoord);
        
        bool[] visited = new bool[capacity];
        int[] dir_X = {1, -1, 0, 0};
        int[] dir_Y = {0, 0, 1, -1};
        int count = 0; // 루프 방지 카운트

        // 초기화
        for(int i = 0; i < capacity; i++)
        {
            visited[i] = false;
        }
        s.Clear();

        s.Push(startBlock);             // 스택 데이터 추가
        vertialResult.Add(startBlock);  // 가로 데이터 추가
        horizonResult.Add(startBlock);  // 세로 데이터 추가

        while(s.Count > 0)
        {
            // 무한 루프 방지
            count++;
            if (count > capacity + 1)
            {
                Debug.Log("FloodFill Overflow");
                break;
            }

            Block_Normal cur = s.Peek();
            s.Pop();

            visited[cur.GridPos.y * size_X + cur.GridPos.x] = true;

            for (int i = 0; i < 4; i++)
            {
                Vector2Int next = new Vector2Int(cur.GridPos.x + dir_X[i], cur.GridPos.y + dir_Y[i]);
                Block_Normal nextBlock = FindBlock(next);
                if (!IsValidPosition(next)) continue;
                if (!visited[next.y * size_X + next.x] && nextBlock.BlockColor == targetColor)
                {
                    if (i < 2 && startCoord.y == nextBlock.GridPos.y) // X
                    {
                        vertialResult.Add(nextBlock); 
                    }
                    else if (i >= 2 && startCoord.x == nextBlock.GridPos.x) // Y
                    {
                        horizonResult.Add(nextBlock); 
                    }

                    s.Push(nextBlock);                    
                }
            }
        }

        // 조건에 맞으면 결과값에 추가
        if(vertialResult.Count >= 3) // 가로가 3칸이상 동일하면 추가
        {
            for(int i = 0; i <  vertialResult.Count; i++)
            {
                result.Add(vertialResult[i]);
            }
        }
        else if(horizonResult.Count >= 3) // 세로가 3칸이상 동일하면 추가
        {
            for (int i = 0; i < horizonResult.Count; i++)
            {
                result.Add(horizonResult[i]);
            }
        }

        return result;
    }

    /// <summary>
    /// 리스트에 있는 블록을 제거하고 점수를 얻는 함수
    /// </summary>
    /// <param name="removeList">제거할블록 리스트</param>
    private void AddScoreAtList(List<Block_Normal> removeList)
    {
        if (removeList.Count == 0) return;

        // 점수 콤보를 위한 색깔 정렬
        removeList.Sort((a, b) =>
        {
            if (a.BlockColor == b.BlockColor) return 0;
            else return a.BlockColor > b.BlockColor ? 1 : -1;
        });

        BlockColor color = removeList[0].BlockColor;
        int combo = 1;

        // 블록 제거
        for (int i = 0; i < removeList.Count; i++)
        {
            Block_Normal block = removeList[i];

            // 점수 추가
            if (color == block.BlockColor)
            {
                combo++;
            }
            else // 색 바뀌면 이전에 처리했던 점수 추가
            {
                onGetScore?.Invoke(SingleBlockScore * combo);
                // 다음 색깔
                color = block.BlockColor;
                combo = 1;
            }

            onGetScore?.Invoke(SingleBlockScore * combo); // 마지막 색깔 점수 추가
            RemoveBlock(block);
        }
    }

    /// <summary>
    /// 보드 빈칸 채우는 함수
    /// </summary>
    private void FillBoard()
    {
        // 블록 추가
        for (int x = 0; x < size_X; x++)
        {
            for (int y = 0; y < size_Y; y++)
            {
                Block_Normal block = FindBlock(x, y);
                if (block == null)
                {
                    // 오브젝트 생성
                    SpawnBlock(new Vector2(x, y));
                }
            }
        }
    }

    /// <summary>
    /// 모든 블록 밑으로 내리기 (밑에 있는 빈칸 채우기)
    /// </summary>
    private void AllBlockPullDown()
    {
        for (int x = 0; x < size_X; x++)
        {
            for (int y = 0; y < size_Y; y++)
            {
                if (FindBlock(x, y) != null) continue;

                for (int i = y + 1; i < size_Y; i++)
                {
                    Block_Normal block = FindBlock(x, i);
                    if (block == null) continue; // 유요하지 않는 좌표
                    else
                    {
                        block.MoveObject(MoveDirection.Down);
                    }
                }
            }
        }
    }

    // 기능 함수 ====================================================================

    private Block_Normal SpawnBlock(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        // 블록 생성
        GameObject obj = Instantiate(blockPrefab);
        obj.transform.position = new Vector2(x * ConstValues.BlockSize, (size_Y + 1) * ConstValues.BlockSize);

        Block_Normal createdBlock = obj.GetComponent<Block_Normal>();
        blockList.Add(createdBlock);
        createdBlock.MoveToCoord(new Vector2Int(x, y));

        // 색갈 랜덤 지정
        int colorRand = UnityEngine.Random.Range(0, Enum.GetValues(typeof(BlockColor)).Length);
        createdBlock.SetColor((BlockColor)colorRand);

        // 슬라이드 액션
        createdBlock.SlideFunction.OnMove_Right += () => { SwapBlockPosition(createdBlock, MoveDirection.Right); CheckTileMatch(); };
        createdBlock.SlideFunction.OnMove_Left += () => { SwapBlockPosition(createdBlock, MoveDirection.Left); CheckTileMatch(); };
        createdBlock.SlideFunction.OnMove_Up += () => { SwapBlockPosition(createdBlock, MoveDirection.Up); CheckTileMatch(); };
        createdBlock.SlideFunction.OnMove_Down += () => { SwapBlockPosition(createdBlock, MoveDirection.Down); CheckTileMatch(); };

        obj.name = $"{createdBlock.BlockColor.ToString()}";

        StartCoroutine(SpawnDelayProcess());       

        return createdBlock;
    }

    private void RemoveBlock(Block_Normal block)
    {
        if (block == null) return;

        blockList.Remove(block);
        Destroy(block.gameObject);
    }

    private void RemoveBlock(Vector2 pos)
    {
        Block_Normal block = FindBlock(pos);
        RemoveBlock(block);
    }

    private void RemoveBlock(int x, int y)
    {
        RemoveBlock(new Vector2(x, y));
    }

    public Block_Normal FindBlock(Vector2 grid)
    {
        if (!IsValidPosition((int)grid.x, (int)grid.y)) return null;

        Block_Normal block = blockList.Find(x => x.GridPos == grid);

        return block;
    }

    public Block_Normal FindBlock(int grid_X, int grid_Y)
    {
        return FindBlock(new Vector2(grid_X, grid_Y));
    }

    private bool IsValidPosition(Vector2 position)
    {
        return position.x >= 0 && position.x < size_X && position.y >= 0 && position.y < size_Y;
    }

    private bool IsValidPosition(int x, int y)
    {
        return IsValidPosition(new Vector2(x, y));
    }

    // 블록 기능 함수 =====================================================================

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
}