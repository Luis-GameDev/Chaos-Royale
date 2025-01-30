using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCooldowns : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private GameObject Q;
    [SerializeField] private GameObject W;
    [SerializeField] private GameObject E;
    [SerializeField] private GameObject R;

    void Start() {
        Q.GetComponent<Image>().sprite = character.Abilities[0].AbilityIcon;
        W.GetComponent<Image>().sprite = character.Abilities[1].AbilityIcon;
        E.GetComponent<Image>().sprite = character.Abilities[2].AbilityIcon;
        R.GetComponent<Image>().sprite = character.Abilities[3].AbilityIcon;
    }

    void Update() {
        Q.GetComponentInChildren<TextMeshProUGUI>().text = character.ability0Cooldown > 0 ? character.ability0Cooldown.ToString("F1") : "";
        W.GetComponentInChildren<TextMeshProUGUI>().text = character.ability1Cooldown > 0 ? character.ability1Cooldown.ToString("F1") : "";
        E.GetComponentInChildren<TextMeshProUGUI>().text = character.ability2Cooldown > 0 ? character.ability2Cooldown.ToString("F1") : "";
        R.GetComponentInChildren<TextMeshProUGUI>().text = character.ability3Cooldown > 0 ? character.ability3Cooldown.ToString("F1") : "";
    }
}
