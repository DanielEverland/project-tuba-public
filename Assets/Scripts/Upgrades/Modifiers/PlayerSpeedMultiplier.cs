using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.UpgradeComponents.Modifiers
{
    /// <summary>
    /// Increases the speed of the player
    /// </summary>
    [System.Serializable]
    public class PlayerSpeedMultiplier : UpgradeModifier
    {
        [SerializeField]
        private float multiplier = 1;

        public float Multiplier => multiplier;
    }
}