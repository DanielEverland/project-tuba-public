using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Limits the framerate of the application
/// </summary>
public class FPSLimiter : MonoBehaviour
{
    [SerializeField, EnumFlags]
    private CallbackState callback = CallbackState.Awake;
    [SerializeField]
    private IntReference targetFramerate = new IntReference(60);

    private void Awake()
    {
        if (callback == CallbackState.Awake)
            LimitFramerate();
    }
    private void Start()
    {
        if (callback == CallbackState.Start)
            LimitFramerate();
    }
    private void Update()
    {
        if (callback == CallbackState.Update)
            LimitFramerate();
    }
    public void LimitFramerate()
    {
        Application.targetFrameRate = targetFramerate.Value;
    }
}
