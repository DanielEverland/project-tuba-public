using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EverlandGames.UpgradeComponents.Modifiers;

namespace EverlandGames.UpgradeComponents.Appliers
{
    /// <summary>
    /// Applies <see cref="DamageMultiplierUpgrade"/> to a given entity
    /// </summary>
    public class TakeDamageMultiplierApplier : ModifierApplier
    {
        [SerializeField]
        private Entity target = default;
        [SerializeField, Tooltip("In the event of multiplier modifiers being registered, how do we decide which to use?")]
        private Upgrades.Comparer comparer = Upgrades.Comparer.Larger;

        private DamageReceivedMultiplierModifier currentModifier;

        protected override void PollModifier()
        {
            RemoveExistingModifier();
            AddNewModifier();
        }
        private void RemoveExistingModifier()
        {
            if (currentModifier == null)
            {
                target.RemoveModifier(currentModifier);
                currentModifier = null;
            }                
        }
        private void AddNewModifier()
        {
            List<DamageMultiplierUpgrade> allUpgrades = Upgrades.GetAllModifiers<DamageMultiplierUpgrade>();

            if (allUpgrades.Count > 0)
            {
                DamageMultiplierUpgrade best = Upgrades.GetBestFloatValue(allUpgrades, x => x.Multiplier, comparer);
                ApplyUpgrade(best);
            }
        }
        private void ApplyUpgrade(DamageMultiplierUpgrade upgrade)
        {
            DamageReceivedMultiplierModifier entityModifier = ScriptableObject.CreateInstance<DamageReceivedMultiplierModifier>();
            entityModifier.Multiplier = upgrade.Multiplier;

            target.AddModifier(entityModifier);
        }
    }
}
