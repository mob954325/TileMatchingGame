using System.Collections;
using UnityEngine;

public class Block_Normal : MonoBehaviour, IMoveable
{
    private BlockType type;
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

    IEnumerator lerpMoveProcess;

    private float moveSpeed = 2;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    /// <summary>
    /// 다음 위치까지 가는데 걸리는 시간
    /// </summary>
    public void SetType(BlockType type = BlockType.Normal)
    {
        this.type = type;
    }

    public void MoveObject(Vector2 moveVec)
    {
        Vector2Int moveGrid = new Vector2Int((int)moveVec.x, (int)moveVec.y);
        GridPos += moveGrid;
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
        float moveToNextTime = 1f / MoveSpeed;
        float timeElapsed = 0f;

        while(timeElapsed < moveToNextTime)
        {
            timeElapsed += Time.deltaTime * MoveSpeed;
            Vector3 lerp = Vector3.Lerp(transform.position, goal, timeElapsed);
            transform.position = lerp;

            yield return null;
        }
    }

    /// <summary>
    /// 오브젝트 제거(비활성화)시 실행되는 함수
    /// </summary>
    public virtual void OnObjectDisable()
    {
        // 사용안함
    }

    private void OnDisable()
    {
        OnObjectDisable();
    }
}