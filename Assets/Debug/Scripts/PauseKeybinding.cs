using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pauses the editor when using a keybinding
/// </summary>
public class PauseKeybinding : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F5))
            UnityEditor.EditorApplication.isPaused = true;
#endif
    }
}
