using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; 
        canvasGroup.alpha = 1f; 

        DropSlot dropSlot = FindDropSlotUnderMouse();
        if (dropSlot != null)
        {
            transform.SetParent(dropSlot.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }

    private DropSlot FindDropSlotUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            DropSlot dropSlot = result.gameObject.GetComponent<DropSlot>();
            if (dropSlot != null)
            {
                Debug.Log("Found DropSlot: " + dropSlot.name);
                return dropSlot; 
            }
        }

        return null; 
    }
}
