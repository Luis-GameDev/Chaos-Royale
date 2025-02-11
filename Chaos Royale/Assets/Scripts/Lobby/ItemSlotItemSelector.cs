using UnityEngine;
using UnityEngine.UI;

public class ItemSlotItemSelector : MonoBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private int slotIndex;

    public Item selectedItem;
    public void Select() {
        lobbyManager.SelectItem(slotIndex);
    }

    public void SetItem(Item item) {
        selectedItem = item;
        gameObject.GetComponent<Image>().sprite = item.ItemIcon;
    }
}
