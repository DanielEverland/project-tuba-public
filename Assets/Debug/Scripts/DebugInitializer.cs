using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes all debug systems
/// </summary>
public class DebugInitializer : MonoBehaviour
{
    [SerializeField]
    private Canvas UIRoot = default;
    [SerializeField]
    private Canvas canvas3DPrefab = default;
    [SerializeField]
    private Canvas canvas2DPrefab = default;
    [SerializeField]
    private DefaultDebugLoadout defaultLoadout = default;

    public static Canvas Canvas2D { get; private set; }
    public static Canvas Canvas3D { get; private set; }
    
    private void Awake()
    {
        CreateUI();
        LoadDefaultLoadout();
    }
    private void LoadDefaultLoadout()
    {
        if (defaultLoadout != null)
            defaultLoadout.Load();
    }
    private void CreateUI()
    {
        Canvas2D = Object.Instantiate(canvas2DPrefab);
        Canvas2D.transform.SetParent(UIRoot.transform);

        Canvas3D = Object.Instantiate(canvas3DPrefab);
    }
}
