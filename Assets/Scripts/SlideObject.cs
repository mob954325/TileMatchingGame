using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 startVec = Vector2.zero;
    public Vector2 dirVec = Vector2.zero;

    public Action OnMove_Right;
    public Action OnMove_Left;
    public Action OnMove_Up;
    public Action OnMove_Down;

    private void OnDisable()
    {
        OnMove_Right = null;
        OnMove_Left = null;
        OnMove_Up = null;
        OnMove_Down = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startVec = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dirVec = (eventData.position - startVec).normalized;

        if(dirVec.x > 0.85f) // RIGHT
        {
            OnMove_Right?.Invoke();
        }
        else if(dirVec.x < -0.85f) // LEFT
        {
            OnMove_Left?.Invoke();
        }
        else if(dirVec.y > 0.85f) // UP
        {
            OnMove_Up?.Invoke();
        }
        else if(dirVec.y < -0.85f) // DOWN
        {
            OnMove_Down?.Invoke();
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
