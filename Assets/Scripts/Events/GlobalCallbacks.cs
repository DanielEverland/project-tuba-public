using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles callbacks from monobehaviour messages
/// </summary>
public class GlobalCallbacks : MonoBehaviour
{
    private static GlobalCallbacks Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject gameObject = new GameObject("Global Callbacks");
                instance = gameObject.AddComponent<GlobalCallbacks>();
            }

            return instance;
        }
    }
    private static GlobalCallbacks instance;

    private UnityEvent updateEvents = new UnityEvent();
    private UnityEvent renderObjectEvents = new UnityEvent();

    private void Update()
    {
        updateEvents.Invoke();
    }
    private void OnRenderObject()
    {
        renderObjectEvents.Invoke();
    }
    public static void AddUpdateListener(UnityAction action)
    {
        Instance.updateEvents.AddListener(action);
    }
    public static void RemoveUpdateListener(UnityAction action)
    {
        Instance.updateEvents.RemoveListener(action);
    }
    public static void AddRenderObjectListener(UnityAction action)
    {
        Instance.renderObjectEvents.AddListener(action);
    }
    public static void RemoveRenderObjectListener(UnityAction action)
    {
        Instance.renderObjectEvents.RemoveListener(action);
    }
}
