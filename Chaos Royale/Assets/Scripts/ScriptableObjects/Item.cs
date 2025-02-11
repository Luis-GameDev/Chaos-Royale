using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Rarity ItemRarity;
    public Sprite ItemIcon;
    public int bonusHealth;
    public float bonusHealthPercent;
    public float bonusDamagePercent;
    public float bonusCooldownReductionPercent;
    public float bonusMovementSpeedPercentage;
    public float bonusCastTimeReductionPercentage;
}

public enum Rarity { Common, Rare, Epic, Legendary }
