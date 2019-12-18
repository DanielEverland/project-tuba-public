using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the camera of the canvas to whatever the main camera in the scene is
/// </summary>
public class Canvas3DSetCamera : MonoBehaviour
{
    [SerializeField]
    private Canvas targetCanvas = default;

    private void OnEnable()
    {
        targetCanvas.worldCamera = Camera.main;
    }
}
