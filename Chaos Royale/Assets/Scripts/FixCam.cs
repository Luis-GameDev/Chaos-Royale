using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCam : MonoBehaviour
{
    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = transform.GetComponent<Transform>();
    }

    void Update() {
        cameraTransform.rotation = Quaternion.Euler(80, 0, 0);
    }
}
