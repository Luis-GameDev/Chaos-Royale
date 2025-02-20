using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    public int clientId;
    public bool readyStatus;

    /*void Update()
    {
        if(readyStatus) {
            GetComponent<Image>().color = Color.green;
        } else {
            GetComponent<Image>().color = Color.red;
        }
    }*/

    public void SetName(string name) {
        var textMeshPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textMeshPro != null) {
            textMeshPro.text = name;
        }
    }
}
