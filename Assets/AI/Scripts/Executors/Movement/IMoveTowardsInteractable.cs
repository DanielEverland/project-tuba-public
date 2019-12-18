using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI interface for moving towards a position
/// </summary>
public interface IMoveTowardsInteractable : IAIMover
{
    void MoveTowards(Interactable interactable);
}
