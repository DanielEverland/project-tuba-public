using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes the creation of a level
/// </summary>
public class LevelInitializer : MonoBehaviour
{
    [SerializeField]
    private BoolReference shouldUseSeed = default;
    [SerializeField]
    private StringVariable seedValue = default;
    [SerializeField]
    private ThemeCollection allThemes = default;
    [SerializeField]
    private GameObject player = default;

    private void Start()
    {
        if (shouldUseSeed.Value)
        {
            Debug.Log($"Initializing game with seed {seedValue.Value}");

            Random.InitState(seedValue.Value.GetHashCode());
        }
        
        LevelBuildData data = LevelDataGenerator.CreateNewLevel(allThemes[0]);
        Level.Current = LevelBuilder.Build(data);

        player.transform.position = data.SpawnPosition;

        shouldUseSeed.Value = false;
    }
}
