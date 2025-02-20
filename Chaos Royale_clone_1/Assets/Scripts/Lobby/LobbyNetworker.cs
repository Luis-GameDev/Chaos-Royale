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

public class LobbyNetworker : NetworkBehaviour
{
    [SerializeField] public LobbyManager lobbyManager;

    public void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
        if (lobbyManager == null)
        {
            Debug.LogError("LobbyManager not found in the scene.");
        } 
        else
        {
            lobbyManager.lobbyNetworker = this;
        }
    }
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
}
