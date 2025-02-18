using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using Unity.VisualScripting;
using FishNet.Object;

public class LobbyManager : NetworkBehaviour
{
    [Header("Player")]
    [SerializeField] private Item[] selectedItems = new Item[3];
    public string selectedCharacter = "Melee";

    [Header("Dependencies")]
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject itemInvContent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject[] itemSlots;
    [SerializeField] private GameObject itemInventory;
    [SerializeField] private GameObject hostScreen;
    [SerializeField] private GameObject clientScreen;
    [SerializeField] private GameObject serverButtons;
    [SerializeField] private GameObject lobbyplayerPrefab;
    [SerializeField] private GameObject lobbyplayerSelection;
    public int isSelectingItemSlotIndex = 0;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI castTimeText;
    [SerializeField] private TextMeshProUGUI cdrText;


    public void SelectItem(int index) {
        // show the item inventory and set the index to the index of the button this function was called from
        itemInventory.SetActive(true);
        isSelectingItemSlotIndex = index;
    }

    public void AddPlayerToSelection(int clientId) {

    }
    
    public void RemovePlayerFromSelection(int clientId) {

    }

    public void AddItemToSlot(Item item) {
        // set the selected item in the selectedItems array
        selectedItems[isSelectingItemSlotIndex] = item;

        // trigger "SetItem" in the ItemSlotItemSelector script to display its icon and occupy the slot
        itemSlots[isSelectingItemSlotIndex].GetComponent<ItemSlotItemSelector>().SetItem(item);

        // hide the item inventory after selecting
        itemInventory.SetActive(false);

        CalculateStats();
    }

    void Start()
    {
        // populate the item inventory with all owned items
        foreach (Item item in items) {
            GameObject itemSlot = Instantiate(itemPrefab, itemInvContent.transform);
            itemSlot.GetComponent<Image>().sprite = item.ItemIcon;
            itemSlot.GetComponent<InvItemItemManager>().item = item;
            itemSlot.GetComponent<InvItemItemManager>().lobbyManager = this;
        }

        CalculateStats();
    }

    /*public void OnClientConnected(int clientId)
    {
        AddPlayerToSelection(clientId);
    }

    public void OnClientDisconnected(int clientId)
    {
        RemovePlayerFromSelection(clientId);
    }*/

    public override void OnStartServer()
    {
        base.OnStartServer();
        UpdateLobby();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateLobby();
    }

    public void UpdateLobby() {
        if (InstanceFinder.IsServerStarted) {
            hostScreen.SetActive(true);
            clientScreen.SetActive(false);
            serverButtons.SetActive(false);
        } else {
            hostScreen.SetActive(false);
            clientScreen.SetActive(true);
            serverButtons.SetActive(false);
        }
    }

    public void CalculateStats() {
        // calculate the stats of the player based on the selected items
        // and update the UI with the new stats
        int totalHealth = 0;
        float totalHealthPercent = 0;
        float totalDamagePercent = 0;
        float totalCDR = 0;
        float totalMovementSpeed = 0;
        float totalCastTimeReduction = 0;

        if(selectedCharacter == "Melee") {
            totalHealth += 700;
        } else if(selectedCharacter == "Ranged") {
            totalHealth += 600;
        } else if(selectedCharacter == "Healer") {
            totalHealth += 600;
        }

        foreach (Item item in selectedItems) {
            if (item != null) {
                totalHealth += item.bonusHealth;
                totalHealthPercent += item.bonusHealthPercent;
                totalDamagePercent += item.bonusDamagePercent;
                totalCDR += item.bonusCooldownReductionPercent;
                totalMovementSpeed += item.bonusMovementSpeedPercentage;
                totalCastTimeReduction += item.bonusCastTimeReductionPercentage;
            }
        }

        healthText.text = "Health: " + totalHealth + " + " + totalHealthPercent + "%";
        damageText.text = "Damage: " + totalDamagePercent + "%";
        speedText.text = "Speed: " + totalMovementSpeed + "%";
        castTimeText.text = "CTR: " + totalCastTimeReduction + "%";
        cdrText.text = "CDR: " + totalCDR + "%";

    }

    void Update()
    {
        
    }
}
