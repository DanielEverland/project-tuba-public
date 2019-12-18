using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A monobehaviour that destroys itself after a set amount of time
/// </summary>
public class MonoBehaviourTimed : MonoBehaviour
{
    [SerializeField]
    private float duration = 1;
    
    private float timeSinceStart;

    private void OnEnable()
    {
        timeSinceStart = 0;
    }
    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        if (timeSinceStart >= duration)
            Destroy(this);
    }
} 
