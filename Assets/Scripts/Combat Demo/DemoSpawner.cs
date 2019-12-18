using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObjectCollection allDemos = default;
    [SerializeField]
    private BaseCollection allEnemies = default;
    [SerializeField]
    private GameObject helpText = default;
    [SerializeField]
    private GameObject player = default;

    private bool IsReady => allEnemies.Count <= 0;

    private void Update()
    {
        PollHelpText();
        PollSpawn();
    }
    private void PollSpawn()
    {
        if (IsReady && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
        {
            Instantiate(allDemos.Random());
            player.transform.position = Vector3.zero;
        }
    }
    private void PollHelpText()
    {
        helpText.SetActive(IsReady);
    }    
}
