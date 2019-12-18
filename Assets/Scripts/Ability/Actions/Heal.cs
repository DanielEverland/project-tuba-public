using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.ActionComponents
{
    public class Heal : AbilityAction.Component
    {
        [SerializeField]
        private FloatReference healAmount = new FloatReference(10);

        public override void Tick(AbilityData data)
        {
            data.Target.Health.Heal(healAmount.Value * data.Multiplier);
        }
    }
}