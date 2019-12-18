using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttacker : AbilityAttacker
{
    protected override AbilityStateDelta UseAbility()
    {
        AbilityInput input = GetDefaultInput(TargetPosition);
        return Ability.UpdateState(input);
    }
}
