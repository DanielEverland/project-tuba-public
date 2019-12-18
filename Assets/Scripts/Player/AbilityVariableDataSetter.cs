using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVariableDataSetter : MonoBehaviour
{
    [SerializeField]
    private AbilityReference currentAbility = default;
    [SerializeField]
    private IntVariable currentAmmo = default;
    [SerializeField]
    private FloatVariable currentCharge = default;
    [SerializeField]
    private FloatVariable currentCooldown = default;

    private void Update()
    {
        currentAmmo.Value = currentAbility.Value.CurrentAmmo;
        currentCooldown.Value = currentAbility.Value.Behaviour.CurrentCooldown;

        if(currentAbility.Value.Behaviour is ChargedBehaviour chargeBehaviour)
        {
            currentCharge.Value = chargeBehaviour.CurrentCharge;
        }
    }
}
