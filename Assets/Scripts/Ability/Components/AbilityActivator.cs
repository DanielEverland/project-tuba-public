using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Ability
{
    public class AbilityActivator : AbilityComponent
    {
        public AbilityActivator(Ability ability) : base(ability)
        {
            LastFireTime = -Ability.Behaviour.CooldownDuration;
        }

        public bool HasPersistentObjects => PersistentObjects.Count > 0;

        private float InputStartTime { get; set; }
        private float LastFireTime { get; set; }
        private Vector2 TargetPosition { get; set; }
        
        private bool HasAmmo => Ability.CurrentAmmo > 0;
        private List<IPersistentAbilityObject> PersistentObjects { get; set; } = new List<IPersistentAbilityObject>();
        
        public override AbilityStateDelta PollInput(AbilityInput input)
        {
            AbilityStateDelta delta = AbilityStateDelta.None;
            CacheInput(input);

            if (HasPersistentObjects)
            {
                UpdatePersistentObjects(input);
            }
            else if(Ability.currentAmmo > 0)
            {
                if (Ability.Behaviour.ShouldFire(input))
                {
                    Fire(input);

                    delta |= AbilityStateDelta.Fired;
                }
            }

            if (HasPersistentObjects)
                delta |= AbilityStateDelta.HasPersistentObject;

            return delta;
        }
        private void UpdatePersistentObjects(AbilityInput input)
        {
            for (int i = PersistentObjects.Count - 1; i >= 0; i--)
            {
                Vector2 direction = TargetPosition - (Vector2)Ability.Owner.transform.position;

                if (PersistentObjects[i] is ISetTargetDirection directionSetter)
                    directionSetter.SetTargetDirection(direction);

                if (PersistentObjects[i] is ISetTargetPosition positionSetter)
                    positionSetter.SetTargetPosition(Ability.Owner.transform.position);
                
                if (PersistentObjects[i].ShouldBeDestroyed())
                {
                    PersistentObjects.RemoveAt(i);
                }
            }
        }
        private void Fire(AbilityInput input)
        {
            LastFireTime = Time.time;
            Ability.CurrentAmmo--;

            Vector2 delta = (TargetPosition - (Vector2)Ability.Owner.transform.position).normalized;
            float angleToTarget = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

            CreateProjectiles(angleToTarget, input);
            Ability.OnFired();
        }
        private void CreateProjectiles(float angleToTarget, AbilityInput input)
        {
            foreach (Muzzle muzzle in Ability.Behaviour.Muzzles)
            {
                IAbilityObject abilityObject = muzzle.Spawner.Spawn(Ability, muzzle.Multiplier);
                abilityObject.GameObject.transform.position = Ability.Owner.transform.position;

                float angle = GetAngle(muzzle, angleToTarget);
                Vector2 direction = angle.GetDirection();

                if(Ability.Behaviour.UseOrthographicScaling)
                    direction = Utility.ScaleToOrthographicVector(direction);


                if (abilityObject is ISetTargetDirection directionSetter)
                    directionSetter.SetTargetDirection(direction);

                if (abilityObject is ISetTargetPosition positionSetter)
                    positionSetter.SetTargetPosition(input.TargetPosition);
                                
                if(abilityObject is IPersistentAbilityObject persistentObject)
                    PersistentObjects.Add(persistentObject);
            }
        }
        public void DestroyPersistentObjects()
        {
            for (int i = 0; i < PersistentObjects.Count; i++)
                PersistentObjects[i].Destroy();

            PersistentObjects.Clear();
        }
        private float GetAngle(Muzzle muzzle, float angleToTarget)
        {
            float rootAngle = GetRootAngle(muzzle, angleToTarget);
            return ApplyInterval(muzzle, rootAngle);
        }
        private float GetRootAngle(Muzzle muzzle, float angleToTarget)
        {
            if (muzzle.Space == Space.Self)
            {
                return angleToTarget + muzzle.Angle;
            }
            else
            {
                return muzzle.Angle;
            }
        }
        private float ApplyInterval(Muzzle muzzle, float rawAngle)
        {
            float randomValue = Random.Range(muzzle.IntervalStart, muzzle.IntervalEnd);

            return rawAngle + randomValue;
        }
        private void CacheInput(AbilityInput input)
        {
            TargetPosition = input.TargetPosition;
        }
    }
}