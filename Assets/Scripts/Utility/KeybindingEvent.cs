using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Raises an event when a keybinding has been pressed
/// </summary>
public class KeybindingEvent : MonoBehaviour
{
    [SerializeField]
    private KeyCode key = default;
    [SerializeField, EnumFlags]
    private InputType inputType = InputType.Down;
    [SerializeField]
    private UnityEvent unityEvent = new UnityEvent();

    private void Update()
    {
        if (Input.GetKeyDown(key) && inputType.HasFlag(InputType.Down))
        {
            Raise();
        }
        if (Input.GetKey(key) && inputType.HasFlag(InputType.Stay))
        {
            Raise();
        }
        if (Input.GetKeyUp(key) && inputType.HasFlag(InputType.Up))
        {
            Raise();
        }
    }
    private void Raise()
    {
        unityEvent.Invoke();
    }

    [System.Flags]
    private enum InputType
    {
        Down = 0b0001,
        Stay = 0b0010,
        Up = 0b0100,
    }
}
