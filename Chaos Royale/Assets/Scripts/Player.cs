using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Character character;
    [SerializeField] private Camera cam;
    void Update()
    {
        if (Input.GetMouseButton(1)) // 1 ist die rechte Maustaste
        {
            Vector3 cursorWorldPosition = GetCursorWorldPosition();

            if(cursorWorldPosition != Vector3.zero) {
                character.Move(cursorWorldPosition);
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && character.globalCooldownLeft <= 0) {
            character.UseAbility(0);
            character.combatTimeLeft = character.combatTime;
        }
        if(Input.GetKeyDown(KeyCode.W) && character.globalCooldownLeft <= 0) {
            character.UseAbility(1);
            character.combatTimeLeft = character.combatTime;
        }
        if(Input.GetKeyDown(KeyCode.E) && character.globalCooldownLeft <= 0) {
            character.UseAbility(2);
            character.combatTimeLeft = character.combatTime;
        }
        if(Input.GetKeyDown(KeyCode.R) && character.globalCooldownLeft <= 0) {
            character.UseAbility(3);
            character.combatTimeLeft = character.combatTime;
        }
    }

    public Vector3 GetCursorWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) 
        {
            return hit.point; 
        }
        return Vector3.zero; 
    }
}
