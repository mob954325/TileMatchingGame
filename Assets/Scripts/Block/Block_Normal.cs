using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SlideObject))]
public class Block_Normal : MonoBehaviour, IMoveable
{
    public Sprite[] sprite;
    private SpriteRenderer spriteRenderere;

    private BlockType blockType;
    public BlockType BlocType { get => blockType; }
    private BlockColor blockColor;
    public BlockColor BlockColor { get => blockColor; }

    private SlideObject slideFunction;
    public SlideObject SlideFunction { get => slideFunction; private set => slideFunction = value; }

    private Vector2Int gridPos;
    public Vector2Int GridPos
    {
        get => gridPos;
        set
        {
            gridPos = value;
            StopCoroutine(LerpMove((Vector2)gridPos * ConstValues.BlockSize));
            StartCoroutine(LerpMove((Vector2)gridPos * ConstValues.BlockSize));
        }
    }

    private float moveSpeed = 2;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private void Awake()
    {
        slideFunction = GetComponent<SlideObject>();
        spriteRenderere = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void SetColor(BlockColor color = BlockColor.Blue)
    {
        blockColor = color;
        spriteRenderere.sprite = sprite[(int)blockColor];
    }

    /// <summary>
    /// 다음 위치까지 가는데 걸리는 시간
    /// </summary>
    public void SetType(BlockType type = BlockType.Normal)
    {
        blockType = type;
    }

    public void MoveToCoord(Vector2 goal)
    {
        GridPos = new Vector2Int((int)goal.x, (int)goal.y);
    }

    public void MoveObject(Vector2 moveVec)
    {
        Vector2Int moveGrid = new Vector2Int((int)moveVec.x, (int)moveVec.y);
        GridPos += moveGrid;
    }

    /// <summary>
    /// 위치값 만큼 현재 위치에서 이동
    /// </summary>
    /// <param name="x">현재 위치에서 이동할 x값</param>
    /// <param name="y">현재 위치에서 이동할 y값</param>
    public void MoveObject(int x, int y)
    {
        MoveObject(new Vector2(x, y));
    }

    public void MoveObject(MoveDirection dir)
    {
        switch(dir)
        {
            case MoveDirection.Right:
                MoveObject(Vector2.right);
                break;
            case MoveDirection.Left:
                MoveObject(Vector2.left);
                break;
            case MoveDirection.Up:
                MoveObject(Vector2.up);
                break;
            case MoveDirection.Down:
                MoveObject(Vector2.down);
                break;
        }
    }

    private IEnumerator LerpMove(Vector3 goal)
    {
        float timeElapsed = 0f;

        while(timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * MoveSpeed;
            Vector3 lerp = Vector3.Lerp(transform.position, goal, timeElapsed);
            transform.position = lerp;

            yield return null;
        }

        transform.position = new Vector2(gridPos.x * ConstValues.BlockSize, gridPos.y * ConstValues.BlockSize); // 이동후 정확한 위치로 이동
    }

    /// <summary>
    /// 오브젝트 제거(비활성화)시 실행되는 함수
    /// </summary>
    public virtual void OnObjectDisable()
    {
    }

    private void OnDisable()
    {
        OnObjectDisable();
    }
}