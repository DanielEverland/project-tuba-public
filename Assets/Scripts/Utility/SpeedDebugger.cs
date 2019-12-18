using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Outputs a positional delta every frame  
/// </summary>
public class SpeedDebugger : MonoBehaviour
{
    private Vector3 previousPosition;

    private void Awake()
    {
        previousPosition = transform.position;
    }
    private void LateUpdate()
    {
        Debug.Log(transform.position - previousPosition);

        previousPosition = transform.position;
    }
}
