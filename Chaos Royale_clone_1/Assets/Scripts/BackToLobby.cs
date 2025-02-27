using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Connection;

public class BackToLobby : MonoBehaviour
{
    [SerializeField] public GameObject winScreen;
    [SerializeField] public GameObject loseScreen;
   public void BackToLobbyButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);
        InstanceFinder.ClientManager.StopConnection();
    }
}
