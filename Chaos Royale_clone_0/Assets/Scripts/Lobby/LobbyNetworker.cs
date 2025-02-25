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
    Debug.Log("Client ID: " + NetworkManager.ClientManager.Connection.ClientId + " Name: " + lobbyManager.playerName);

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

public IEnumerator WaitForClientId() {
bool hasClientId = false;
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
        yield return new WaitForEndOfFrame();
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

    public void SetReady()
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
    }

    [ServerRpc(RequireOwnership = true)]
    public void StartGameServerRpc()
    {
        /* if (lobbyManager.playersInLobby.Count <= 2)
        {
            Debug.Log("Not enough players to start the game.");
            return;
        }

        foreach (var player in lobbyManager.playersInLobby)
        {
            if (player.GetComponent<LobbyPlayer>().readyStatus == false)
            {
                Debug.Log("Not all players are ready.");
                return;
            }
        } */

        StartGame();
    }

    [ObserversRpc]
    private void StartGame()
    {
        lobbyManager.StartGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
        var playerInstance = Instantiate(playerPrefab);
        Spawn(playerInstance);
    }

    /*public void SetName()
    {
        Debug.Log($"SetName() called. isClientInitialized: {IsClientInitialized}, Owner: {Owner}");
        
        if (IsClientInitialized)
        {
            if (Owner == null)
            {
                Debug.LogError("Owner is NULL when calling SetPlayerUsernameServerRpc!");
                return;
            }

            SetPlayerUsernameServerRpc(Owner);
        }
    }*/

    /*public void OnClientConnected(int clientId)
    {
        lobbyManager.AddPlayerToSelection(clientId);
    }

    public void OnClientDisconnected(int clientId)
    {
        lobbyManager.RemovePlayerFromSelection(clientId);
    }*/

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc(NetworkConnection conn)
    {
        foreach (var player in lobbyManager.playersInLobby)
        {
            var lobbyPlayer = player.GetComponent<LobbyPlayer>();
            if(lobbyPlayer != null && conn.ClientId == lobbyPlayer.clientId) {

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

    /*[ServerRpc(RequireOwnership = false)]
    public void SetPlayerUsernameServerRpc(NetworkConnection conn)
    {
        foreach (var player in lobbyManager.playersInLobby)
        {
            var lobbyPlayer = player.GetComponent<LobbyPlayer>();
            if(lobbyPlayer != null && conn.ClientId == lobbyPlayer.clientId) {
                lobbyPlayer.username = lobbyManager.playerName;
            }
        }
    }*/
}
