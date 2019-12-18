using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will display a UI panel when the player dies
/// </summary>
public class OnDeathPanel : MonoBehaviour
{
    [SerializeField]
    private FloatVariable playerHealth = default;
    [SerializeField]
    private GameObject panel = default;

    private void Awake()
    {
        panel.SetActive(false);
    }
    public void Poll()
    {
        if(playerHealth.Value <= 0)
        {
            panel.SetActive(true);
        }
    }
}
