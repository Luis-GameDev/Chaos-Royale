using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet;
using FishNet.Transporting;
using FishNet.Connection;
using FishNet.Object;

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
    [SerializeField] private Button readyButton;
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
    public GameObject LobbyNetworkerInstance;

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

    [SerializeField] private GameObject LobbyNetworkerPrefab;
    private bool ready = false;

    public void SelectItem(int index) {
        // show the item inventory and set the index to the index of the button this function was called from
        itemInventory.SetActive(true);
        isSelectingItemSlotIndex = index;
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

    public IEnumerator AddPlayerToSelection(int clientId) {
        Debug.Log("XXXXX ");
        yield return new WaitForSeconds(0.5f);
        GameObject player = Instantiate(lobbyplayerPrefab, lobbyplayerSelection.transform);
        if(lobbyNetworker) {
            foreach(var playername in lobbyNetworker.playerNames) {
                if(playername.Key == clientId) {
                    player.GetComponent<LobbyPlayer>().username = playername.Value;
                }
                if (string.IsNullOrEmpty(player.GetComponent<LobbyPlayer>().username)) {
                    player.GetComponent<LobbyPlayer>().username = "Player";
                }
            }
        }
        player.GetComponent<LobbyPlayer>().clientId = clientId;
        playersInLobby.Add(player);  
    }

    private void OnClientConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args) {
        if (args.ConnectionState == RemoteConnectionState.Started) {
            if (InstanceFinder.ServerManager.Started) {
                Debug.Log($"Client {conn.ClientId} has joined the server!");
                StartCoroutine(AddPlayerToSelection(conn.ClientId));
            }
        }
        if (args.ConnectionState == RemoteConnectionState.Stopped) {
            RemovePlayerFromSelection(conn.ClientId);
        }
    }

    private void OnDestroy()
    {
        if (InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.OnRemoteConnectionState -= OnClientConnectionState;
    }

    void Update() {
        if(!lobbyNetworker) {
            lobbyNetworker = FindObjectOfType<LobbyNetworker>();
        }
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
        ready = !ready;
        if (ready)
        {
            ColorBlock colors = readyButton.colors;
            colors.normalColor = Color.green;
            colors.highlightedColor = Color.green;
            colors.pressedColor = Color.green;
            colors.selectedColor = Color.green;
            readyButton.colors = colors;
        }
        else
        {
            ColorBlock colors = readyButton.colors;
            colors.normalColor = Color.red;
            colors.highlightedColor = Color.red;
            colors.pressedColor = Color.red;
            colors.selectedColor = Color.red;
            readyButton.colors = colors;
        }
    }
    
    public void RemovePlayerFromSelection(int clientId) {
        foreach(var player in playersInLobby) {
            if(player.GetComponent<LobbyPlayer>().clientId == clientId) {
                playersInLobby.Remove(player);
                Destroy(player);
            }
        }
    }

    public void StartHost()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
        Debug.Log("Host gestartet (Server + Client)");

        hostScreen.SetActive(true);
        clientScreen.SetActive(false);
        serverButtons.SetActive(false);     

        StartCoroutine(CheckForServerStart());
    }

    private IEnumerator CheckForServerStart()
    {
        bool Switch = false;

        while(!Switch)
        {
            if(!InstanceFinder.ServerManager.Started)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            Switch = !Switch;
            GameObject go = Instantiate(LobbyNetworkerPrefab);
            LobbyNetworkerInstance = go;
            lobbyNetworker = go.GetComponent<LobbyNetworker>();
            //lobbyNetworker.playerNames.Add(InstanceFinder.NetworkManager.ClientManager.Connection.ClientId, playerName);
            Debug.Log($"Spawning: {LobbyNetworkerInstance.name}");
            InstanceFinder.ServerManager.Spawn(LobbyNetworkerInstance, InstanceFinder.ClientManager.Connection);     
        }
    }


    public void StartClient()
    {
        InstanceFinder.ClientManager.StartConnection();
        Debug.Log("Client gestartet");
        
        hostScreen.SetActive(false);
        clientScreen.SetActive(true);
        serverButtons.SetActive(false);
        
        lobbyNetworker = FindObjectOfType<LobbyNetworker>();
        /*bool clientStarted = false;

         while (!clientStarted)
        {
            if (InstanceFinder.ClientManager.Started)
            {
                clientStarted = true;
                lobbyNetworker.playerNames.Add(InstanceFinder.NetworkManager.ClientManager.Connection.ClientId, playerName);
            }
        } */
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
        selectedItems[isSelectingItemSlotIndex] = item;

        itemSlots[isSelectingItemSlotIndex].GetComponent<ItemSlotItemSelector>().SetItem(item);

        itemInventory.SetActive(false);

        CalculateStats();
    }

    void Start()
    {
        InstanceFinder.ServerManager.OnRemoteConnectionState += OnClientConnectionState;
        databaseManager = transform.GetComponent<DatabaseManager>();

        // populate the item inventory with all owned items
        foreach (Item item in items) {
            GameObject itemSlot = Instantiate(itemPrefab, itemInvContent.transform);
            itemSlot.GetComponent<Image>().sprite = item.ItemIcon;
            itemSlot.GetComponent<InvItemItemManager>().item = item;
            itemSlot.GetComponent<InvItemItemManager>().lobbyManager = this;
        }

        CalculateStats();
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

    public void StartGameButton() {
        lobbyNetworker.StartGameServerRpc();
    }
}
