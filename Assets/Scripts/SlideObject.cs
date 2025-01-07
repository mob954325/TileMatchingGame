using UnityEngine;
using UnityEngine.EventSystems;

public class SlideObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 startVec = Vector2.zero;
    public Vector2 dirVec = Vector2.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log($"start Drag : {eventData.position}");
        startVec = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragged");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log($"end Drag : {eventData.position}");
        dirVec = (eventData.position - startVec).normalized;
        Debug.Log($"{dirVec}");

        if(dirVec.x > 0.85f)
        {
            Debug.Log($"right");
        }
        else if(dirVec.x < -0.85f)
        {
            Debug.Log($"left");
        }
        else if(dirVec.y > 0.85f)
        {
            Debug.Log($"up");
        }
        else if(dirVec.y < -0.85f)
        {
            Debug.Log($"down");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ASDFASDFSAD");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("+++");
    }
}
