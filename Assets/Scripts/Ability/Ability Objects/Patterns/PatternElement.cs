using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an individual object used in creating a <see cref="Pattern"/>
/// </summary>
public class PatternElement : MonoBehaviour
{
    public Vector2 StartingPosition { get; private set; }
    
    [SerializeField]
    private List<Behaviour> damageDealers = default;

    private void Awake()
    {
        if (damageDealers.Count == 0)
            Debug.LogError("No damage dealers assigned to this pattern element", this);
    }
    public void DisableCollision()
    {
        ToggleCollider(false);
    }
    public void EnableCollision()
    {
        ToggleCollider(true);
    }
    private void ToggleCollider(bool enabled)
    {
        for (int i = 0; i < damageDealers.Count; i++)
        {
            damageDealers[i].enabled = enabled;
        }
    }
    public void AssignStartingPosition(Vector2 position)
    {
        StartingPosition = position;
    }
}
