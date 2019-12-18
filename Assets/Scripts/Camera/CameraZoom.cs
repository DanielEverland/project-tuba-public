using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the camera to change orthographic size using the scroll wheel
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private Camera cameraTarget;
    [SerializeField]
    private float zoomSpeed = 20;
    [SerializeField]
    private float minSize = 1;
    [SerializeField]
    private float maxSize = 10;

    private void Update()
    {
        float delta = -Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        float newSize = Mathf.Clamp(cameraTarget.orthographicSize + delta, minSize, maxSize);

        cameraTarget.orthographicSize = newSize;
    }
    private void OnValidate()
    {
        if (cameraTarget == null)
            cameraTarget = GetComponent<Camera>();
    }
}
