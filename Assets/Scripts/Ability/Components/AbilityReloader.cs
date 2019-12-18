using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Ability
{
    public class AbilityReloader : AbilityComponent
    {
        public AbilityReloader(Ability ability) : base(ability)
        {
        }

        public bool IsReloading { get; private set; }

        private float DeltaTime { get; set; }
        private float ReloadTimePerAmmo => Ability.Behaviour.ReloadTime / Ability.Behaviour.AmmoCapacity;

        public override AbilityStateDelta PollInput(AbilityInput input)
        {
            if (input.ShouldReload && !IsReloading)
            {
                StartReload();
                return AbilityStateDelta.Reloading;
            }

            return AbilityStateDelta.None;
        }
        public void StartReload()
        {
            IsReloading = true;
            DeltaTime = 0;
        }
        public AbilityStateDelta DoReload()
        {
            DeltaTime += Time.deltaTime;

            while (DeltaTime >= ReloadTimePerAmmo)
            {
                if(Ability.CurrentAmmo >= Ability.Behaviour.AmmoCapacity)
                {
                    IsReloading = false;
                    break;
                }

                DeltaTime -= ReloadTimePerAmmo;
                Ability.CurrentAmmo++;
            }

            return AbilityStateDelta.Reloading;
        }
    }
}
