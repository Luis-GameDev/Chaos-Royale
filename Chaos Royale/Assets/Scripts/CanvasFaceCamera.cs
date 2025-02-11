using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour
{
    [SerializeField] public Camera targetCamera; 

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; 
        }
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            transform.LookAt(transform.position + targetCamera.transform.forward);
        }
    }
}
