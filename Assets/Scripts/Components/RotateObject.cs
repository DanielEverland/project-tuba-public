using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will rotate an object in world space
/// </summary>
public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotation = Vector3.zero;

    private void Update()
    {
        transform.eulerAngles += rotation * Time.deltaTime;
    }
}
