using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;
using MySqlX.XDevAPI;
using FishNet.Managing.Scened;


public class LobbyNetworker : NetworkBehaviour
{
    public LobbyManager lobbyManager;
    public readonly SyncDictionary<int, string> playerNames = new SyncDictionary<int, string>();
    public GameObject playerPrefab;

    public override void OnStartClient()
{
    base.OnStartClient();

    lobbyManager = FindObjectOfType<LobbyManager>();
    lobbyManager.lobbyNetworker = this;

    playerNames.OnChange += InstantiatePlayerCard;

    StartCoroutine(WaitForClientId());
    /* bool hasClientId = false;
    while(!hasClientId) {
        if(NetworkManager.ClientManager.Connection.ClientId != -1) {
            hasClientId = true;
            if (NetworkManager.ClientManager.Connection.ClientId != -1)
                {
                if (!playerNames.ContainsKey(NetworkManager.ClientManager.Connection.ClientId))
                {
                    playerNames.Add(NetworkManager.ClientManager.Connection.ClientId, lobbyManager.playerName);
                }
                else
                {
                    Debug.Log("Player already added with ClientId: " + NetworkManager.ClientManager.Connection.ClientId);
                }
            }
            else
            {
                Debug.LogError("Invalid ClientId: " + NetworkManager.ClientManager.Connection.ClientId);
            }
        } 
    }  */
    

    if (lobbyManager == null)
    {
        Debug.LogError("LobbyManager not found in the scene.");
    }
    else
    {
        lobbyManager.lobbyNetworker = this;
    }
}

public void InstantiatePlayerCard(SyncDictionaryOperation op,
    int key, string value, bool asServer){
    Debug.Log("InstantiatePlayerCard() called.");
    foreach (var player in playerNames)
    {
        Debug.Log("Playername: " + player.Value);
    }
}

public IEnumerator WaitForClientId() {
bool hasClientId = false;
    while(!hasClientId) {
        Debug.Log("Waiting for ClientId...");

        if(NetworkManager.ClientManager.Connection.ClientId == -1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }


        Debug.Log("Client ID: " + NetworkManager.ClientManager.Connection.ClientId + " Name: " + lobbyManager.playerName);

        hasClientId = true;
        AddPlayerToSyncDictionary(NetworkManager.ClientManager.Connection.ClientId, lobbyManager.playerName);

        
    }
}


    /*public void SetPlayerName(NetworkConnection conn) {
        playerNames.Add(conn.ClientId, lobbyManager.playerName);
        if(HasAuthority) {
            foreach(var player in playerNames) {
                Debug.Log("Playername: " + player.Value);
            }
        }
    }*/
    
    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerToSyncDictionary(int clientId, string playerName)
    {
        Debug.Log($"AddPlayerToSyncDictionary player with ClientId: {clientId} and name: {playerName}");
        if (!playerNames.ContainsKey(clientId))
        {
            AddPlayer(clientId, playerName);
        }
        else
        {
            Debug.Log("Player already added with ClientId: " + clientId);
        }
    }

    [Server] public void AddPlayer(int client, string name) {
        Debug.Log($"Adding player with ClientId: {client} and name: {name}");
        playerNames.Add(client, name);
        lobbyManager.AddPlayerToSelection(client);
    }

    public void SetReady()
    {        
        if (IsClientInitialized)
        {
            if (InstanceFinder.ClientManager.Connection.ClientId == -1)
            {
                Debug.LogError("Owner is NULL when calling SetPlayerReadyServerRpc!");
                return;
            }

            SetPlayerReadyServerRpc(InstanceFinder.ClientManager.Connection.ClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        if (lobbyManager.playersInLobby.Count < 2)
        {
            Debug.Log("Not enough players to start the game.");
            return;
        }

        foreach (var player in lobbyManager.playersInLobby)
        {
            if (player.GetComponent<LobbyPlayer>().readyStatus == false && player.GetComponent<LobbyPlayer>().clientId != InstanceFinder.ClientManager.Connection.ClientId)
            {
                Debug.Log("Not all players are ready.");
                return;
            }
        }
        StartGame();
    }

    [Server]
    private void StartGame()
    {
        SceneLoadData sld = new SceneLoadData("Game");
        sld.ReplaceScenes = ReplaceOption.All;

        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoaded;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    private void OnSceneLoaded(SceneLoadEndEventArgs args)
    {
        Debug.Log("Scene Loaded");
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc(int clientId)
    {
        Debug.Log("Set ready!!!!!!!!!!!!!!");
        foreach (var player in lobbyManager.playersInLobby)
        {
            Debug.Log("Searching for player with client id..." + clientId);
            var lobbyPlayer = player.GetComponent<LobbyPlayer>();
            if(lobbyPlayer != null && clientId == lobbyPlayer.clientId) {
                Debug.Log("Found player...");
                if(lobbyPlayer.readyStatus == false) {
                    lobbyPlayer.readyStatus = true;
                    Debug.Log($"Player {lobbyPlayer.clientId} is now READY.");
                } else {
                    lobbyPlayer.readyStatus = false;
                    Debug.Log($"Player {lobbyPlayer.clientId} is no longer READY.");
                }     
            }
        }
    }
}
