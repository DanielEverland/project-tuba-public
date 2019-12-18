using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all data for a given ability
/// </summary>[
[Serializable, CreateAssetMenu(menuName = Utility.MenuItemRoot + "Ability", order = 299)]
public partial class Ability : ScriptableObject
{
    public Entity Owner { get; private set; }
    public AbilityBehaviour Behaviour => behaviour;
    public AbilityAction Action => action;

    public int CurrentAmmo
    {
        get
        {
            if (Behaviour.InfiniteAmmo)
                return int.MaxValue;

            return currentAmmo;
        }
        set
        {
            currentAmmo = value;
        }
    }
    private int currentAmmo;

    [SerializeField]
    private AbilityBehaviour behaviour;
    [SerializeField]
    private AbilityAction action;

    public AbilityReloader Reloader => Components.Reloader;
    public AbilityActivator Activator => Components.Activator;

    private ComponentContainer Components
    {
        get
        {
            if (components == null)
                throw new NullReferenceException("Components haven't been initialized");

            return components;
        }
    }
    private ComponentContainer components;
    
    public AbilityStateDelta UpdateState(AbilityInput input)
    {
        AbilityStateDelta delta = AbilityStateDelta.None;
        delta |= Reloader.PollInput(input);
        
        if (Reloader.IsReloading)
        {
            delta |= Reloader.DoReload();
        }
        else
        {
            delta |= Activator.PollInput(input);
        }

        return delta;
    }
    public void OnFired()
    {
        Behaviour.OnFired();
        Action.OnFired();
    }
    public void OnStop()
    {
        Behaviour.OnStop();
        Action.OnStop();

        Activator.DestroyPersistentObjects();
    }
    public void EquipPart(AbilityPart part)
    {
        if (IsPartEquipped(part))
            return;

        Unequip(part.Type);

        switch (part)
        {
            case AbilityBehaviour behaviour:
                ChangeBehaviour(behaviour);
                break;
            case AbilityAction action:
                ChangeAction(action);
                break;
            default:
                throw new NotImplementedException();
        }

        part.OnEquipped();
    }
    public void ChangeBehaviour(AbilityBehaviour behaviour)
    {
        this.behaviour = behaviour;
        CurrentAmmo = behaviour.AmmoCapacity;
    }
    public void ChangeAction(AbilityAction action)
    {
        this.action = action;
    }
    private bool IsPartEquipped(AbilityPart part)
    {
        return GetEquippedPartOfType(part.Type) == part;
    }
    private void Unequip(AbilityPartType partType)
    {
        if (HasPartOfType(partType))
            GetEquippedPartOfType(partType).OnUneqipped();
    }
    private bool HasPartOfType(AbilityPartType type)
    {
        return GetEquippedPartOfType(type) != null;
    }
    public AbilityPart GetEquippedPartOfType(AbilityPartType partType)
    {
        switch (partType)
        {
            case AbilityPartType.Behaviour:
                return Behaviour;
            case AbilityPartType.Action:
                return Action;
            default:
                throw new NotImplementedException();
        }
    }
    public void AssignRandomParts()
    {
        EquipPart(AbilityLoader.Behaviour.Random());
        EquipPart(AbilityLoader.Action.Random());
    }
    private void Initialize()
    {
        CurrentAmmo = Behaviour.AmmoCapacity;
    }
    public static Ability Instantiate(Ability prefab, Entity owner)
    {
        Ability newAbility = Instantiate(prefab);
        newAbility.Owner = owner;

        newAbility.behaviour = prefab.Behaviour.CreateInstance<AbilityBehaviour>(newAbility);
        newAbility.action = prefab.Action.CreateInstance<AbilityAction>(newAbility);
        newAbility.components = new ComponentContainer(newAbility);

        newAbility.Initialize();

        return newAbility;
    }
    
    private class ComponentContainer
    {
        public ComponentContainer(Ability ability)
        {
            Reloader = new AbilityReloader(ability);
            Activator = new AbilityActivator(ability);
        }

        public AbilityReloader Reloader { get; private set; }
        public AbilityActivator Activator { get; private set; }
    }

    [Serializable]
    public class Muzzle : IEquatable<Muzzle>
    {
        public AbilityObject Spawner => _spawner;
        public Space Space => _space;
        public float Angle => _angle;
        public float IntervalStart => _intervalStart;
        public float IntervalEnd => _intervalEnd;
        public float Multiplier => _multiplier;

        [SerializeField, Range(0.05f, 10)]
        private float _multiplier = 1;
        [SerializeField]
        private AbilityObject _spawner = default;
        [SerializeField]
        private float _angle = default;
        [SerializeField]
        private Space _space = Space.Self;
        [SerializeField]
        private float _intervalStart = default;
        [SerializeField]
        private float _intervalEnd = default;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Muzzle)
            {
                return Equals(obj as Muzzle);
            }

            return false;
        }
        public bool Equals(Muzzle other)
        {
            if (other == null)
                return false;

            return
                other.Angle == Angle &&
                other.Space == Space &&
                other.IntervalStart == IntervalStart &&
                other.IntervalEnd == IntervalEnd;
        }
        public override int GetHashCode()
        {
            int i = 17;

            i += Angle.GetHashCode() * 13;
            i += Space.GetHashCode() * 13;
            i += IntervalStart.GetHashCode() * 13;
            i += IntervalEnd.GetHashCode() * 13;

            return i;
        }
        public override string ToString() => $"{_angle} ({IntervalStart}-{IntervalEnd})\n{_space}";
    }
}