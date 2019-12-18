using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.UpgradeComponents.Modifiers
{
    /// <summary>
    /// Spawns an object when player is damaged
    /// </summary>
    [System.Serializable]
    public class SpawnOnDamage : UpgradeModifier
    {
        [SerializeField]
        private GameObject prefab = default;

        public GameObject Prefab => prefab;
    }
}