using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple wrapper for quitting application
/// </summary>
public class QuitApplication : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
}
