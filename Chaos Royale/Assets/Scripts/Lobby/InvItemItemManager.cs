using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvItemItemManager : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPrefab;
    private GameObject tooltipHolder;
    private GameObject tooltip;
    public Item item;

    void Start()
    {
        tooltipHolder = GameObject.Find("TooltipHolder");
    }

    public void StartHover()
    {
        tooltip = Instantiate(tooltipPrefab, tooltipHolder.transform);
        
        // Tooltip korrekt im TooltipHolder positionieren
        tooltip.transform.SetParent(tooltipHolder.transform, false);

        // Initiale Mausposition setzen
        RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
        Canvas canvas = tooltip.GetComponentInParent<Canvas>();

        if (canvas == null) return;

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), 
            Input.mousePosition, 
            canvas.worldCamera, 
            out anchoredPos
        );

        tooltipRect.anchoredPosition = anchoredPos;
        
        UpdateTooltipPosition();

        // Farbe basierend auf Seltenheit setzen
        Color rarityColor;
        switch (item.ItemRarity)
        {
            case Rarity.Common:
                rarityColor = Color.green;
                break;
            case Rarity.Rare:
                rarityColor = new Color(0, 0.5f, 1f);
                break;
            case Rarity.Epic:
                rarityColor = Color.magenta;
                break;
            case Rarity.Legendary:
                rarityColor = Color.yellow;
                break;
            default:
                rarityColor = Color.white;
                break;
        }

        tooltip.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.Name;
        tooltip.transform.Find("Name").GetComponent<TextMeshProUGUI>().color = rarityColor;
        tooltip.transform.Find("BonusHealth").GetComponent<TextMeshProUGUI>().text = "Health: " + item.bonusHealth.ToString() + " + " + item.bonusHealthPercent.ToString() + "%";
        tooltip.transform.Find("BonusDamage").GetComponent<TextMeshProUGUI>().text = "Damage: " + item.bonusDamagePercent.ToString() + "%";
        tooltip.transform.Find("BonusCDR").GetComponent<TextMeshProUGUI>().text = "Cooldown Rdc: " + item.bonusCooldownReductionPercent.ToString() + "%";
        tooltip.transform.Find("BonusMovement").GetComponent<TextMeshProUGUI>().text = "Move Speed: " + item.bonusMovementSpeedPercentage.ToString() + "%";
        tooltip.transform.Find("BonusCTR").GetComponent<TextMeshProUGUI>().text = "Cast Time Rdc: " + item.bonusCastTimeReductionPercentage.ToString() + "%";
    }

    public void StopHover()
    {
        Destroy(tooltip);
    }

    private void UpdateTooltipPosition()
    {
        if (tooltip == null) return;

        RectTransform canvasRectTransform = tooltipHolder.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform tooltipRectTransform = tooltip.GetComponent<RectTransform>();

        Vector3 tooltipPosition = Input.mousePosition;
        float canvasWidth = canvasRectTransform.rect.width * canvasRectTransform.lossyScale.x;
        float canvasHeight = canvasRectTransform.rect.height * canvasRectTransform.lossyScale.y;
        float tooltipWidth = tooltipRectTransform.rect.width * tooltipRectTransform.lossyScale.x;
        float tooltipHeight = tooltipRectTransform.rect.height * tooltipRectTransform.lossyScale.y;

        // Begrenzungen prüfen und korrigieren
        if (tooltipPosition.x + tooltipWidth > Screen.width)
        {
            tooltipPosition.x = Screen.width - tooltipWidth;
        }

        if (tooltipPosition.x < 0)
        {
            tooltipPosition.x = 0;
        }

        if (tooltipPosition.y + tooltipHeight > Screen.height)
        {
            tooltipPosition.y = Screen.height - tooltipHeight;
        }

        if (tooltipPosition.y < tooltipHeight)
        {
            tooltipPosition.y = tooltipHeight;
        }

        tooltip.transform.position = tooltipPosition;
    }
}
