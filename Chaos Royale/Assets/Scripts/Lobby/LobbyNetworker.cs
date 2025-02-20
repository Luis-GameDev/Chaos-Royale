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
using FishNet.Object.Synchronizing;

public class LobbyNetworker : NetworkBehaviour
{
    [SerializeField] public LobbyManager lobbyManager;
    public readonly SyncDictionary<int, string> playerNames = new SyncDictionary<int, string>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        lobbyManager = FindObjectOfType<LobbyManager>();
        lobbyManager.lobbyNetworker = this;
        if (lobbyManager == null)
        {
            Debug.LogError("LobbyManager not found in the scene.");
        } 
        else
        {
            lobbyManager.lobbyNetworker = this;
        }

        //SetPlayerName(Owner);
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
