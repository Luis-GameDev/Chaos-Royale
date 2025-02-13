using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour
{
    [SerializeField] public Camera targetCamera { get; private set; }

    void Start()
    {
        //targetCamera = Camera.main; 
        //targetCamera = Camera.current;
    }

    void LateUpdate()
    {
        targetCamera = Camera.current;
        if (targetCamera != null)
        {
            transform.LookAt(transform.position + targetCamera.transform.forward);
        }
    }
}
