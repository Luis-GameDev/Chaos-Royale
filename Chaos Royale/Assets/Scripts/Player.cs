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
    }

    Vector3 GetCursorWorldPosition()
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
