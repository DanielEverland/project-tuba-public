using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles the assigning of seed values to a variable
/// </summary>
public class SeedInputFieldHandler : MonoBehaviour
{
    [SerializeField]
    private StringVariable seedVariable = default;
    [SerializeField]
    private TMP_InputField inputField = default;

    private void Awake()
    {
        seedVariable.Value = Random.Range(0, int.MaxValue).ToString();
        inputField.text = seedVariable.Value;
    }
    public void UpdateVariable()
    {
        seedVariable.Value = inputField.text;
    }
}
