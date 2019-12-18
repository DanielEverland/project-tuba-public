using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.UpgradeComponents.Modifiers
{
    [System.Serializable]
    public class DamageMultiplierUpgrade : UpgradeModifier
    {
        [SerializeField]
        private float multiplier = 1;

        public float Multiplier => multiplier;
    }
}
