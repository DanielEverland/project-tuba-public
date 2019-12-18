using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityObject
{
    [SerializeField]
    private Type type;
    [SerializeField]
    private ProjectileBase projectile;
    [SerializeField]
    private BeamBase beam;
    [SerializeField]
    private HitScanObject hitScanObject;
    [SerializeField]
    private ExplosionObject explosionObject;
    [SerializeField]
    private DamageOnTouch damageOnTouch;
    
    public IAbilityObjectSpawner Value
    {
        get
        {
            switch (type)
            {
                case Type.Projectile:
                    return projectile;
                case Type.Beam:
                    return beam;
                case Type.HitScan:
                    return hitScanObject;
                case Type.Explosion:
                    return explosionObject;
                case Type.DamageOnTouch:
                    return damageOnTouch;
                default:
                    throw new System.NotImplementedException();
            }
        }
        set
        {
            switch (value)
            {
                case ProjectileBase projectile:
                    this.projectile = projectile;
                    type = Type.Projectile;
                    break;
                case BeamBase beam:
                    this.beam = beam;
                    type = Type.Beam;
                    break;
                case HitScanObject hitScanObject:
                    this.hitScanObject = hitScanObject;
                    type = Type.HitScan;
                    break;
                case ExplosionObject explosionObject:
                    this.explosionObject = explosionObject;
                    type = Type.Explosion;
                    break;
                case DamageOnTouch damageOnTouch:
                    this.damageOnTouch = damageOnTouch;
                    type = Type.DamageOnTouch;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
    public IAbilityObject Spawn(Ability ability, float multiplier)
    {
        IAbilityObject abilityObject = Value.Spawn(ability, multiplier);
        HierarchyManager.Add(abilityObject.GameObject, HierarchyCategory.AbilityObjects);

        return abilityObject;
    }

    [System.Serializable]
    public enum Type
    {
        Projectile,
        Beam,
        HitScan,
        Explosion,
        DamageOnTouch,
    }
}
