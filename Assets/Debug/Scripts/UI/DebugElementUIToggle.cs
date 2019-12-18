using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UI element for <see cref="DebugElementToggle"/>
/// Will update the UI element to reflect the state of the debug element
/// </summary>
[RequireComponent(typeof(Toggle))]
public class DebugElementUIToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private DebugElementToggle debugElement = default;
    [SerializeField]
    private Toggle toggleElement = default;

    public void OnPointerClick(PointerEventData eventData)
    {
        debugElement.Toggle(!debugElement.IsOn);

        SetState();
    }
    protected void OnEnable()
    {
        SetState();
    }
    private void SetState()
    {
        toggleElement.isOn = debugElement.IsOn;
    }
    private void OnValidate()
    {
        if (toggleElement)
            toggleElement = GetComponent<Toggle>();
    }
}
