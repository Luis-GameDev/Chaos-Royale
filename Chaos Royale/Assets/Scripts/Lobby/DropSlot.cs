using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            droppedObject.transform.SetParent(transform);
            droppedObject.transform.position = transform.position;
        }
    }
}
