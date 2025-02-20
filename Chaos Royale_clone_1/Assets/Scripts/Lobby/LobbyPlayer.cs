using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    public int clientId;
    public bool readyStatus;
    public string username;

    void Start()
    {
        var textMeshPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textMeshPro != null) {
            textMeshPro.text = username;
        }
    }
}
