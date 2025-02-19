using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    public int clientId;
    public bool readyStatus;

    public void SetName(string name) {
        var textMeshPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textMeshPro != null) {
            textMeshPro.text = name;
        }
    }
}
