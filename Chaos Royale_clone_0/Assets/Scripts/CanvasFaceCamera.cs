using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour
{
    [SerializeField] public Camera targetCamera { get; private set; }

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; 
        }
    }


    public void SetCamera(Camera newCamera)
    {
        targetCamera = newCamera;
    }


    void LateUpdate()
    {
        if (targetCamera != null)
        {
            transform.LookAt(transform.position + targetCamera.transform.forward);
        }
    }
}
