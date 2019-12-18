using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum = System.Enum;

/// <summary>
/// Debugs the current input
/// </summary>
public class InputDebugger : MonoBehaviour
{
    [SerializeField]
    private bool shouldOutputDeltaLog = false;
    [SerializeField, Readonly]
    private List<KeyCode> currentKeys = null;

    private void Update()
    {
        if(Application.isEditor)
            UpdateReadonlyField();

        if (shouldOutputDeltaLog)
            OutputDeltaLog();
    }
    private void OutputDeltaLog()
    {
        Utility.GetAllDownKeys().ForEach(x => Debug.Log(x));
    }
    private void UpdateReadonlyField()
    {
        currentKeys = Utility.GetAllStayKeys();
    }
}
