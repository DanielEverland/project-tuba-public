using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.ActionComponents
{
    public class Damage : AbilityAction.Component
    {
        [SerializeField]
        protected float damage = 10;
        
        public override void Tick(AbilityData data)
        {
            data.Target.Health.TakeDamage(damage * data.Multiplier);
        }
    }
}