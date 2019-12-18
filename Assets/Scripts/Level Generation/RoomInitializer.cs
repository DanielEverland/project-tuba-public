using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will initialize the level using a simple room
/// </summary>
public class RoomInitializer : MonoBehaviour
{
    [SerializeField]
    private RoomData roomData = default;

    private void Awake()
    {
        Level.Current = LevelBuilder.Build(roomData);
    }
}
