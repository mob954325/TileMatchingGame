using UnityEngine;

public interface IMoveable
{
    public float MoveSpeed { get; set; }

    public void MoveObject(Vector2 moveVec);

    public void MoveObject(MoveDirection dir);
}