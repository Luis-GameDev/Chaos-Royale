using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private List<string> characters = new List<string>();
    [SerializeField] private Image classImage;
    [SerializeField] private Sprite MeleeImage;
    [SerializeField] private Sprite RangedImage;
    [SerializeField] private Sprite HealerImage;
    [SerializeField] private TextMeshProUGUI classNameText;
    private string selectedCharacter = "Melee";

    private int currentIndex = 0;

    public void Left() {
        currentIndex--;
        if (currentIndex < 0) {
            currentIndex = characters.Count - 1;
        }
        SelectCharacter();
    }

    public void Right() {
        currentIndex++;
        if (currentIndex >= characters.Count) {
            currentIndex = 0;
        }
        SelectCharacter();
    }

    private void SelectCharacter() {
        selectedCharacter = characters[currentIndex];
        // Update the lobby manager or UI with the selected character
        lobbyManager.selectedCharacter = selectedCharacter;

        // Update the class image
        classImage.sprite = selectedCharacter switch {
            "Melee" => MeleeImage,
            "Ranged" => RangedImage,
            "Healer" => HealerImage,
            _ => classImage.sprite
        };

        // Update the class name text
        classNameText.text = selectedCharacter;
        lobbyManager.CalculateStats();
    }
}
