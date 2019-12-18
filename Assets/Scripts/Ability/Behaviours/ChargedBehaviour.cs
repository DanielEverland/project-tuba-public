using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour type that supports charging
/// </summary>
[CreateAssetMenu(menuName = MenuRoot + "Charged Behaviour", order = MenuOrder + 1)]
public class ChargedBehaviour : AbilityBehaviour
{
    public float CurrentCharge { get; private set; }
    public float ChargeTime => chargeTime;

    [SerializeField]
    private float chargeTime = 0;
    [SerializeField]
    private bool singleFire = true;
    [SerializeField]
    private List<AbilityRenderer> chargeEffects = default;
    [SerializeField]
    private Gradient colorOverLifetime = default;

    private Color LifetimeColor
    {
        get
        {
            float percentage = Mathf.Clamp01(CurrentCharge / ChargeTime);
            return colorOverLifetime.Evaluate(percentage);
        }
    }

    private ChargedBehaviourEffects Effects
    {
        get
        {
            if(_effects == null)
            {
                CreateEffects();
            }

            return _effects;
        }
    }
    private ChargedBehaviourEffects _effects;

    private void CreateEffects()
    {
        _effects = new ChargedBehaviourEffects(chargeEffects, Ability.Owner);
    }
    public override bool ShouldFire(AbilityInput input)
    {
        if (singleFire)
        {
            return ShouldFireSingle(input);
        }
        else
        {
            return ShouldFireContinuous(input);
        }
    }
    private bool ShouldFireSingle(AbilityInput input)
    {
        if (IsCooldownReady)
        {
            if (CurrentCharge >= ChargeTime)
            {
                UpdateChargeEffect(input);

                return input.FireButtonReleased;
            }
            else if (input.FireButtonStay)
            {
                UpdateChargeEffect(input);

                Charge(input);
            }
            else
            {
                CurrentCharge = 0;                
            }
        }

        return false;
    }
    private bool ShouldFireContinuous(AbilityInput input)
    {
        if(input.FireButtonStay)
        {
            UpdateChargeEffect(input);

            if (CurrentCharge >= ChargeTime)
            {
                return base.ShouldFire(input);
            }
            else
            {
                Charge(input);
            }
        }
        else
        {
            RemoveCharge();
        }

        return false;
    }
    public void Charge(AbilityInput input)
    {
        if (CurrentCharge == 0)
            Effects.StartChargeEffects();

        CurrentCharge = Mathf.Clamp(CurrentCharge + Time.deltaTime, 0, ChargeTime);
    }
    public override AbilityInput GetInputToFire()
    {
        if(singleFire && CurrentCharge >= chargeTime)
        {
            return new AbilityInput()
            {
                FireButtonReleased = true,
            };
        }
        else
        {
            return new AbilityInput()
            {
                FireButtonStay = true,
            };
        }
    }
    private void UpdateChargeEffect(AbilityInput input)
    {
        Effects.UpdateChargeEffects(input.TargetPosition);
        Effects.UpdateColor(LifetimeColor);
    }
    public override void OnFired()
    {
        base.OnFired();

        if (singleFire)
            SingleOnFired();
        else
            ContinuousOnFired();
    }
    public override void OnStop()
    {
        RemoveCharge();
    }
    private void SingleOnFired()
    {
        RemoveCharge();
    }
    private void ContinuousOnFired()
    {
        if(Ability.CurrentAmmo == 0)
        {
            RemoveCharge();
        }
    }
    private void RemoveCharge()
    {
        CurrentCharge = 0;
        Effects.EndChargeEffect();
    }
}
