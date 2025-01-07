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
    /// ���� ��ġ���� ���µ� �ɸ��� �ð�
    /// </summary>
    private float moveToNextTime = 0.5f;

    public void SetType(BlockType type = BlockType.Normal)
    {
        this.type = type;
    }

    /// <summary>
    /// �׸��� ������ �̵��ϴ� �Լ�
    /// </summary>
    /// <param name="moveVec">�̵���</param>
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
    /// ������Ʈ ����(��Ȱ��ȭ)�� ����Ǵ� �Լ�
    /// </summary>
    public virtual void OnObjectDisable()
    {
        // ������
    }

    private void OnDisable()
    {
        OnObjectDisable();
    }
}