using UnityEngine;

public class PlayerHighlight : MonoBehaviour
{
    public Camera playerCamera;  // Reference to the camera (can be the main camera)
    public LayerMask playerLayer;  // The layer that your players are on
    public Material outlineMaterial;  // The material with outline shader to highlight players
    public bool Enabled = false;  // Boolean to toggle the outline on/off

    private GameObject currentTarget;  // The player that is currently targeted

    void Update()
    {
        HandleHover();
        HandleLeftClick();
    }

    void HandleHover()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            GameObject hoveredObject = hit.collider.gameObject;

            if (hoveredObject.CompareTag("Player") && Enabled)
            {
                if (hoveredObject != currentTarget)
                {
                    if (currentTarget != null)
                    {
                        RestoreOriginalMaterial(currentTarget);
                    }

                    ApplyOutline(hoveredObject);
                    currentTarget = hoveredObject; 
                }
            }
        }
        else
        {
            if (currentTarget != null)
            {
                RestoreOriginalMaterial(currentTarget);
                currentTarget = null;  
            }
        }
    }

    void HandleLeftClick()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag("Player") && Enabled)
                {
                    ApplyOutline(clickedObject);
                    currentTarget = clickedObject; 
                }
            }
        }
    }

    void ApplyOutline(GameObject target)
    {
        Renderer targetRenderer = target.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            Material[] materials = targetRenderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].shader == outlineMaterial.shader)
                {
                    materials[i].SetInt("_Enabled", 1); 
                    targetRenderer.materials = materials;  
                    break;
                }
            }
        }
    }

    void RestoreOriginalMaterial(GameObject target)
    {
        Renderer targetRenderer = target.GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            Material[] materials = targetRenderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].shader == outlineMaterial.shader)
                {
                    materials[i].SetInt("_Enabled", 0); 
                    targetRenderer.materials = materials;  
                    break;
                }
            }
        }
    }
}
