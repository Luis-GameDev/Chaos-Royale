using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.ComponentModel;
using FishNet.Connection;
using MySql.Data.MySqlClient;
using FishNet.Managing.Scened;

public class ServerManager : NetworkBehaviour
{
    [Header("Server")]
    public static ServerManager Instance;
    public readonly SyncVar<float> matchTimeLeft = new SyncVar<float>(600.0f);
    public float matchTime = 600.0f;
    public readonly SyncVar<bool> gameStarted = new SyncVar<bool>(false);
    public readonly SyncDictionary<int, Character> players = new SyncDictionary<int, Character>();
    public GameObject playerPrefab;
    public int totalPlayers = 0;
    public int matchId;

    [Header("Database")]
    private string connectionString = "Server=localhost;Database=db_chaosroyale;User ID=root;Password=;Pooling=false";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        matchId = CreateMatchEntry();
        Debug.Log("Match ID: " + matchId);
        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            Debug.Log("Player: "+ conn.ClientId);
            SpawnPlayer(conn);
            totalPlayers++;
        }
    }

    /* [Server]
    public void StartGame()
    {
        
    } */

    [ServerRpc(RequireOwnership = false)]
    public void PlayerDeath(int clientId)
    {
        foreach (var player in players)
        {
            if (player.Key == clientId)
            {
                // add player to the database when he dies and remove him from the list of active players
                CreatePlayerEntry(clientId, player.Value);
                players.Remove(player);
                break;
            }
        }
        if(players.Count <= 1)
        {
            foreach (var player in players)
            {
                CreatePlayerEntry(player.Key, player.Value);
                player.Value.WinMatch();
                StartCoroutine(EndGame());
            }
        }
    }

    [Server]
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        InstanceFinder.ServerManager.StopConnection(true);
    }

    private void SpawnPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        Character charComponent = player.GetComponent<Character>();
        players.Add(conn.ClientId, charComponent);

        NetworkObject netObj = player.GetComponent<NetworkObject>();

        if (netObj != null)
        {
            ServerManager.Spawn(player, conn);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    void FixedUpdate()
    {
        matchTimeLeft.Value -= Time.deltaTime;
        matchTime = matchTimeLeft.Value;
    }

    [Server]
    private int CalculatePoints(int clientId) {
        int points = 0;
        points += 800 - (int)matchTime;

        // calculating the points multiplier based on the player's placement
        int placement = totalPlayers - players.Count + 1;
        float multiplier = 1.0f;

        if (placement <= totalPlayers / 2)
        {
            multiplier = 1.0f + (totalPlayers / 2 - placement) * 0.1f;
        }
        else
        {
            multiplier = 1.0f - (placement - totalPlayers / 2) * 0.1f;
        }

        points = (int)(points * multiplier);

        return points;
    }

    [Server]
    private int GetPlayerIdFromUsername(string username) {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT ID FROM user WHERE username = @nickname LIMIT 1;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nickname", username);
                    object result = cmd.ExecuteScalar();
                    
                    if (result != null) {
                        return Convert.ToInt32(result);
                    } else {
                        return -1;
                    }   
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("MySQL Error: " + ex.Message);
                return -1;
            }
        }
    }

    [Server]
    private void CreatePlayerEntry(int clientId, Character player) {
        string playerClass = player.Name;
        int points = CalculatePoints(clientId);
        int kills = 0;
        int playerId;
        int items = 0;

        // find the players id in the database based on his name 
        LobbyNetworker lobbyNetworker = FindAnyObjectByType<LobbyNetworker>();
        string playerName = "";
        foreach(var LNplayer in lobbyNetworker.playerNames) {
            if (LNplayer.Key == clientId) {
                playerName = LNplayer.Value;
                break;
            }
        }
        playerId = GetPlayerIdFromUsername(playerName);

        int placement = totalPlayers - players.Count + 1;

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO match_players (player_id, match_id, kills, points, placement, items, class) " +
                               "VALUES (@player_id, @match_id, @kills, @points, @placement, @items, @class);";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@player_id", playerId);
                    cmd.Parameters.AddWithValue("@match_id", matchId);
                    cmd.Parameters.AddWithValue("@kills", kills);
                    cmd.Parameters.AddWithValue("@points", points);
                    cmd.Parameters.AddWithValue("@placement", placement);
                    cmd.Parameters.AddWithValue("@items", items);
                    cmd.Parameters.AddWithValue("@class", playerClass);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("MySQL Error: " + ex.Message);
            }
        }
    }

    [Server]
    private int CreateMatchEntry() {
        int matchId = -1; // Default value if insert fails

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO matches () VALUES (); SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    matchId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("MySQL Error: " + ex.Message);
            }
        }

        return matchId;
    }
}
