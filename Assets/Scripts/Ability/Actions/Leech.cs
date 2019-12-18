using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.ActionComponents
{
    public class Leech : Damage
    {
        public override void Tick(AbilityData data)
        {
            base.Tick(data);

            data.Owner.Health.Heal(damage * data.Multiplier);
        }
    }
}