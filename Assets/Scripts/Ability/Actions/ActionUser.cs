using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Uses an action
/// </summary>
public class ActionUser : CallbackMonobehaviour
{
    [SerializeField]
    private AbilityAction action = default;
    [SerializeField]
    private EntityReference owner = default;    
    [SerializeField]
    private EntityReference target = default;
    [SerializeField]
    private UnityEvent onRaised = default;

    public void OnRaisedWithCollision(Collider2D collider)
    {
        if(collider.gameObject.TryGetEntity(out Entity entity))
        {
            if (entity == target.Value)
                OnRaised();
        }
    }
    public override void OnRaised()
    {
        AbilityData data = new AbilityData()
        {
            Owner = owner.Value,
            Target = target.Value,
            Multiplier = 1,
        };

        action.Tick(data);
        
        onRaised.Invoke();
    }
}
