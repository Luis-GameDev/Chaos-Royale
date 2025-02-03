using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvItemItemManager : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPrefab;
    private GameObject tooltip;
    public GameObject lobby;
    [SerializeField] private bool isHovering = false;
    public Item item;

    void Start()
    {
        
    }
    void Update()
    {
        /*if(isHovering) {
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = 10.0f; 
            tooltip.transform.position = Camera.main.ScreenToWorldPoint(cursorPosition);
        }*/
    }

    public void StartHover() {
        // todo fix: tooltip is instantiated behind lobby-canvas so it's not visible
        Debug.Log("Hovering over item");
        tooltip = Instantiate(tooltipPrefab, lobby.transform);
        tooltip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        isHovering = true;
        tooltip.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.Name;
        tooltip.transform.Find("BonusHealth").GetComponent<TextMeshProUGUI>().text = "Bonus Health: " + item.bonusHealth.ToString() + " + " + item.bonusHealthPercent.ToString() + "%";
        tooltip.transform.Find("BonusDamage").GetComponent<TextMeshProUGUI>().text = "Bonus Damage: " + item.bonusDamagePercent.ToString() + "%";
    }

    public void StopHover() {
        Destroy(tooltip);
        isHovering = false;
    }
}
