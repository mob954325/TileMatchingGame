using System.Collections;
using UnityEngine;

public enum BlockType
{
    Normal,
    Long,
    Bomb
}

public class Block_Normal : MonoBehaviour
{
    private BlockType type;
    private Vector2Int gridPos;
    public Vector2Int GridPos
    {
        get => gridPos;
        set
        {
            gridPos = value;
            StartCoroutine(LerpMove((Vector2)gridPos * ConstValues.BlockSize));
        }
    }

    /// <summary>
    /// 다음 위치까지 가는데 걸리는 시간
    /// </summary>
    private float moveToNextTime = 0.5f;

    public void SetType(BlockType type = BlockType.Normal)
    {
        this.type = type;
    }

    /// <summary>
    /// 그리드 값으로 이동하는 함수
    /// </summary>
    /// <param name="moveVec">이동값</param>
    public void MoveObject(Vector2Int moveVec)
    {
        GridPos += moveVec;
    }

    private IEnumerator LerpMove(Vector3 goal)
    {
        float timeElapsed = 0f;;

        while(timeElapsed < moveToNextTime)
        {
            timeElapsed += Time.deltaTime / moveToNextTime;
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