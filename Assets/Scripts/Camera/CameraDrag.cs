using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera script that lets you drag to move around
/// </summary>
public class CameraDrag : MonoBehaviour
{
    [SerializeField]
    private KeyCode dragKey = KeyCode.Mouse2;

    private Vector2 lastPosition;

    private void LateUpdate()
    {
        PollInput();
    }
    private void PollInput()
    {
        // Reset on start
        if (Input.GetKeyDown(dragKey))
        {
            lastPosition = Utility.MousePositionInWorld;
        }

        if (Input.GetKey(dragKey))
        {
            Move(Utility.MousePositionInWorld - lastPosition);
            lastPosition = Utility.MousePositionInWorld;
        }
    }
    private void Move(Vector3 delta)
    {
        transform.position -= delta;
    }
}
