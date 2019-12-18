using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField]
    private AbilityVariable currentAbility = default;
    
    private void Update()
    {
        if (InputSuppressor.IsSuppressed || PauseState.IsPaused)
            return;

        AbilityInput input = GetInput();

        DebugInput(input);
        currentAbility.Value.UpdateState(input);
    }
    private AbilityInput GetInput()
    {
        Vector3 delta = (Utility.MousePositionInWorld - (Vector2)transform.position).normalized;
        
        return new AbilityInput()
        {
            TargetPosition = transform.position + delta,
            FireButtonStay = Input.GetKey(KeyCode.Mouse0),
            ShouldReload = Input.GetKeyDown(KeyCode.R),
            FireButtonReleased = Input.GetKeyUp(KeyCode.Mouse0),
        };
    }
    private void DebugInput(AbilityInput input)
    {
        Debug.DrawLine(transform.position, input.TargetPosition, Color.cyan);
    }
}
