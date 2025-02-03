using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject itemInvContent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject[] itemSlots;
    public bool isSelectingItem = false;
    public int isSelectingItemSlotIndex = 0;


    public void SelectItem(Item item) {
        isSelectingItem = false;
       // itemSlots[isSelectingItemSlotIndex].
    }
    void Start()
    {
        foreach (Item item in items) {
            GameObject itemSlot = Instantiate(itemPrefab, itemInvContent.transform);
            itemSlot.GetComponent<Image>().sprite = item.ItemIcon;
            itemSlot.GetComponent<InvItemItemManager>().item = item;
            itemSlot.GetComponent<InvItemItemManager>().lobby = this.gameObject;
        }
    }

    void Update()
    {
        
    }
}
