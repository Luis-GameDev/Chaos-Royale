using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCam : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    void Update() {
        if(cameraTransform != null) {
            Vector3 cameraPosition = cameraTransform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, cameraPosition, ref velocity, smoothTime);
        }
    }
}
