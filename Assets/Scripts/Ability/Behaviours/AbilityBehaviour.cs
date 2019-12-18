using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muzzle = Ability.Muzzle;

[CreateAssetMenu(menuName = MenuRoot + "Behaviour", order = MenuOrder + 0)]
public class AbilityBehaviour : AbilityPart
{
    public float CurrentCooldown => Mathf.Clamp(Time.time - lastFireTime, 0, CooldownDuration);
    public override AbilityPartType Type => AbilityPartType.Behaviour;

    public IEnumerable<Muzzle> Muzzles => muzzles;
    public int AmmoCapacity => ammoCapacity;
    public int ObjectsToFire => objectsToFire;
    public bool InfiniteAmmo => infiniteAmmo;
    public bool RandomMuzzle => randomMuzzle;
    public float ReloadTime => reloadTime;
    public float CooldownDuration => cooldown;
    public bool UseOrthographicScaling => useOrthographicScaling;

    protected bool IsCooldownReady => CurrentCooldown >= CooldownDuration;
        
    [SerializeField]
    private float cooldown = 0.3f;
    [SerializeField]
    private bool infiniteAmmo = false;
    [SerializeField]
    private int ammoCapacity = 16;
    [SerializeField]
    private float reloadTime = 0.1f;
    [SerializeField]
    private List<Muzzle> muzzles;
    [SerializeField]
    private bool randomMuzzle = false;    
    [SerializeField]
    private int objectsToFire = 1;
    [SerializeField]
    private bool useOrthographicScaling = false;

    private float lastFireTime = float.MinValue;
    
    /// <summary>
    /// Returns the input necessary to fire the ability
    /// </summary>
    public virtual AbilityInput GetInputToFire()
    {
        return new AbilityInput()
        {
            FireButtonStay = true,
        };
    }
    /// <summary>
    /// Is called when the ability handler wants to fire, but needs permission from the behaviour
    /// </summary>
    public virtual bool ShouldFire(AbilityInput input)
    {
        return IsCooldownReady && input.FireButtonStay;
    }
    public override void OnFired()
    {
        lastFireTime = Time.time;
    }
    private void Reset()
    {
        if (muzzles == null)
            muzzles = new List<Muzzle>();

        if(muzzles.Count == 0)
        {
            muzzles.Add(new Muzzle());
        }
    }
}
