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
using FishNet.Connection;

public class LobbyManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Item[] selectedItems = new Item[3];
    public string selectedCharacter = "Melee";
    public string playerName;

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

    [Header("Host")]
    public List<GameObject> playersInLobby = new List<GameObject>();
    public LobbyNetworker lobbyNetworker;

    [Header("Login")]
    [SerializeField] private GameObject login_nameInputField;
    [SerializeField] private GameObject login_passwordInputfield;
    [SerializeField] private GameObject loggedInScreen;
    [SerializeField] private GameObject notLoggedInScreen;
    [SerializeField] private DatabaseManager databaseManager;

    [Header("Register")]
    [SerializeField] private GameObject register_nameinputField;
    [SerializeField] private GameObject register_passwordinputField;
    [SerializeField] private GameObject register_passwordcheckinputField;

    public void SelectItem(int index) {
        // show the item inventory and set the index to the index of the button this function was called from
        itemInventory.SetActive(true);
        isSelectingItemSlotIndex = index;
    }

    public void TestAddPlayer() {
        AddPlayerToSelection(11);
    }

    public void Login() {
        string username = login_nameInputField.GetComponent<TMP_InputField>().text;
        string password = login_passwordInputfield.GetComponent<TMP_InputField>().text;
        if(!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password) && databaseManager.Login(username, password)) {
            playerName = username;
            loggedInScreen.SetActive(true);
            notLoggedInScreen.SetActive(false);
        }
    }

    public void Register() {
        string username = register_nameinputField.GetComponent<TMP_InputField>().text;
        string password = register_passwordinputField.GetComponent<TMP_InputField>().text;
        string passwordcheck = register_passwordcheckinputField.GetComponent<TMP_InputField>().text;

        if(!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(passwordcheck) && password == passwordcheck) {
            if(databaseManager.Register(username, password, passwordcheck)) {
                playerName = username;
                loggedInScreen.SetActive(true);
                notLoggedInScreen.SetActive(false);
            }
        }
    }

    public void AddPlayerToSelection(int clientId) {
        Debug.Log("XXXXX ");
        GameObject player = Instantiate(lobbyplayerPrefab, lobbyplayerSelection.transform);
        player.GetComponent<LobbyPlayer>().clientId = clientId;
        player.GetComponent<LobbyPlayer>().SetName("Player " + clientId.ToString());
        playersInLobby.Add(player);
    }

    void Update() {
        foreach (var player in playersInLobby) {
            if (player.GetComponent<LobbyPlayer>().readyStatus == true) {
                player.GetComponent<Image>().color = Color.green;
            } else {
                player.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void SetReadyLM() {
        lobbyNetworker.SetReady();
    }
    
    public void RemovePlayerFromSelection(int clientId) {
        foreach(var player in playersInLobby) {
            if(player.GetComponent<LobbyPlayer>().clientId == clientId) {
                playersInLobby.Remove(player);
                Destroy(player);
            }
        }
    }

    /*public void SetReady()
    {
        Debug.Log($"SetReady() called. isClientInitialized: {IsClientInitialized}, Owner: {Owner}");
        
        if (IsClientInitialized)
        {
            if (Owner == null)
            {
                Debug.LogError("Owner is NULL when calling SetPlayerReadyServerRpc!");
                return;
            }

            SetPlayerReadyServerRpc(Owner);
        }
    }*/

    public void StartHost()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
        Debug.Log("Host gestartet (Server + Client)");

        hostScreen.SetActive(true);
        clientScreen.SetActive(false);
        serverButtons.SetActive(false);
    }

    public void StartClient()
    {
        InstanceFinder.ClientManager.StartConnection();
        Debug.Log("Client gestartet");
        
        hostScreen.SetActive(false);
        clientScreen.SetActive(true);
        serverButtons.SetActive(false);
    }

    public void LeaveLobby() {
        InstanceFinder.ClientManager.StopConnection();
        if (InstanceFinder.NetworkManager.IsServerStarted)
        {
            InstanceFinder.ServerManager.StopConnection(true);
            Debug.Log("Network connection stopped.");
        }
        hostScreen.SetActive(false);
        clientScreen.SetActive(false);
        serverButtons.SetActive(true);
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
        databaseManager = transform.GetComponent<DatabaseManager>();
        InstanceFinder.ServerManager.OnRemoteConnectionState += OnClientConnectionState;

        // populate the item inventory with all owned items
        foreach (Item item in items) {
            GameObject itemSlot = Instantiate(itemPrefab, itemInvContent.transform);
            itemSlot.GetComponent<Image>().sprite = item.ItemIcon;
            itemSlot.GetComponent<InvItemItemManager>().item = item;
            itemSlot.GetComponent<InvItemItemManager>().lobbyManager = this;
        }

        CalculateStats();
    }

    private void OnClientConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            Debug.Log($"XXXXXXXXXXXXXXXXXXXXXXXXX Client {conn.ClientId} has joined the server!");
            AddPlayerToSelection(conn.ClientId);
        }
        if(args.ConnectionState == RemoteConnectionState.Stopped) 
        {
            Debug.Log($"XXXXXXXXXXXXXXXXXXXXXXXXX Client {conn.ClientId} has left the lobby!");
            RemovePlayerFromSelection(conn.ClientId); 
        }
    }

    private void OnDestroy()
    {
        if (InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.OnRemoteConnectionState -= OnClientConnectionState;
    }

    /*public void OnClientConnected(int clientId)
    {
        AddPlayerToSelection(clientId);
    }

    public void OnClientDisconnected(int clientId)
    {
        RemovePlayerFromSelection(clientId);
    }*/

    /*public override void OnStartServer()
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
    }*/

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

    /*[ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc(NetworkConnection conn)
    {
        foreach (var player in playersInLobby)
        {
            var lobbyPlayer = player.GetComponent<LobbyPlayer>();
            if (lobbyPlayer != null)
            {
                lobbyPlayer.readyStatus = true;
                Debug.Log($"Player {lobbyPlayer.clientId} is now READY.");
            }
        }
    }*/
}
