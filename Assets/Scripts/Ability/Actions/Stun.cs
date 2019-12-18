using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.ActionComponents
{
    /// <summary>
    /// Stuns the target for a certain duration
    /// </summary> 
    public class Stun : AbilityAction.Component
    {
        [SerializeField]
        private float stunTime = 1;

        public override void Tick(AbilityData data)
        {
            data.Target.AddModifier(typeof(StunnedModifier), stunTime * data.Multiplier);
        }
    }
}