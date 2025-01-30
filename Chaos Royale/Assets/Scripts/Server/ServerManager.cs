using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.ComponentModel;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Instance;
    public readonly SyncVar<float> matchTimeLeft = new SyncVar<float>(600.0f);
    public float matchTime = 600.0f;
    public readonly SyncVar<bool> gameStarted = new SyncVar<bool>(false);
    public readonly List<Character> players = new List<Character>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        if (!InstanceFinder.IsServerStarted) // Check if server is running
        {
            InstanceFinder.ServerManager.StartConnection(); // Start server
        }
    }

    void Update()
    {
        matchTimeLeft.Value -= Time.deltaTime;
        matchTime = matchTimeLeft.Value;
    }
}
