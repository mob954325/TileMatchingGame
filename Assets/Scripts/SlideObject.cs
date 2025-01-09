using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    IMoveable moveable;

    private Vector2 startVec = Vector2.zero;
    public Vector2 dirVec = Vector2.zero;

    private void Awake()
    {
        moveable = GetComponent<IMoveable>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startVec = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log($"end Drag : {eventData.position}");
        dirVec = (eventData.position - startVec).normalized;

        if(dirVec.x > 0.85f) // RIGHT
        {
            moveable.MoveObject(MoveDirection.Right);
        }
        else if(dirVec.x < -0.85f) // LEFT
        {
            moveable.MoveObject(MoveDirection.Left);
        }
        else if(dirVec.y > 0.85f) // UP
        {
            moveable.MoveObject(MoveDirection.Up);
        }
        else if(dirVec.y < -0.85f) // DOWN
        {
            moveable.MoveObject(MoveDirection.Down);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 사용안함
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 사용안함
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 사용안함
    }
}
