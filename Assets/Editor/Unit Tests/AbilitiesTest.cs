using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AbilitiesTest
    {
        private static AbilityInput ReloadInput = new AbilityInput() { ShouldReload = true, };
        private static AbilityInput FireInput = new AbilityInput() { ShouldReload = false, FireButtonStay = true, };

        [UnityTest]
        public IEnumerator CannotHaveNegativeAmmo()
        {
            int startAmmo = 0;

            Ability ability = GetAbility();
            // Ensure ability isn't charged
            AbilityPart nonChargedBehaviour = AbilityLoader.Behaviour.Where(x => !(x is ChargedBehaviour)).Random();
            ability.EquipPart(nonChargedBehaviour);

            // We do this after equipping since changing behaviour will reset ammo count to max
            ability.CurrentAmmo = startAmmo;
            
            GlobalCallbacks.AddUpdateListener(() => ability.UpdateState(FireInput));

            // First we wait for the cooldown to expire
            yield return new WaitForSeconds(ability.Behaviour.CooldownDuration);

            // Then we wait a single frame for the actual shooting to occur
            yield return 0;
            
            Assert.AreEqual(startAmmo, ability.CurrentAmmo);
        }
        [UnityTest]
        public IEnumerator CannotReloadPastMaxAmmoLimit()
        {
            Ability ability = GetAbility();
            GlobalCallbacks.AddUpdateListener(() => ability.UpdateState(ReloadInput));

            yield return new WaitForSeconds(1);

            Assert.AreEqual(ability.Behaviour.AmmoCapacity, ability.CurrentAmmo);
        }
        [UnityTest]
        public IEnumerator ShouldFullyReloadWithinTimeframe()
        {
            Ability ability = GetAbility();
            ability.CurrentAmmo = 0;

            GlobalCallbacks.AddUpdateListener(() => ability.UpdateState(ReloadInput));
            
            yield return new WaitForSeconds(ability.Behaviour.ReloadTime);

            Assert.AreEqual(ability.Behaviour.AmmoCapacity, ability.CurrentAmmo);
        }
        [Test]
        public void ShouldEquipNewBehaviour()
        {
            ShouldEquipPart<AbilityBehaviour>();
        }
        [Test]
        public void ShouldEquipNewAction()
        {
            ShouldEquipPart<AbilityAction>();
        }
        private void ShouldEquipPart<T>() where T : AbilityPart
        {
            Ability testAbility = GetAbility();
            T part = ScriptableObject.CreateInstance<T>();

            testAbility.EquipPart(part);

            Assert.AreEqual(testAbility.GetEquippedPartOfType(part.Type), part);
        }
        private Ability GetAbility()
        {
            Entity owner = UnitTestUtil.InstantiateEntity();
            Ability ability = ScriptableObject.CreateInstance<Ability>();
            ability.AssignRandomParts();

            ability = Ability.Instantiate(ability, owner);

            return ability;
        }
    }
}
