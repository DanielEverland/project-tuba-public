using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Type = System.Type;

/// <summary>
/// Handles everything related to <see cref="EntityModifier"/>
/// </summary>
public partial class Entity : MonoBehaviour
{
    private Dictionary<Type, ModifierList> modifiers = new Dictionary<Type, ModifierList>();
    private HashSet<Type> dirtyModifiers = new HashSet<Type>();

    /// <summary>
    /// Events are raised whenever the ContainsModifier state switches
    /// </summary>
    private Dictionary<Type, UnityEvent> events = new Dictionary<Type, UnityEvent>();

    private void CallEvent(Type modifier)
    {
        if (events.ContainsKey(modifier))
            events[modifier].Invoke();
    }
    public void AddModifierListener(UnityAction call, EntityModifier modifier) => AddModifierListener(call, modifier.GetType());
    public void AddModifierListener(UnityAction call, Type modifier)
    {
        if(!events.ContainsKey(modifier))
        {
            events.Add(modifier, new UnityEvent());
        }

        events[modifier].AddListener(call);
    }
    public void RemoveModifierListener(UnityAction call, EntityModifier modifier) => RemoveModifierListener(call, modifier.GetType());
    public void RemoveModifierListener(UnityAction call, Type modifier)
    {
        if (events.ContainsKey(modifier))
        {
            events[modifier].RemoveListener(call);
        }
    }

    public void SetModifierAsDirty(EntityModifier modifier) => SetModifierAsDirty(modifier.GetType());
    public void SetModifierAsDirty(Type type)
    {
        if (!dirtyModifiers.Contains(type))
            dirtyModifiers.Add(type);
    }
    public T GetModifier<T>(int index) where T : EntityModifier
    {
        return (T)modifiers[typeof(T)][index];
    }
    public int GetModifierCount(EntityModifier modifier) => GetModifierCount(modifier.GetType());
    public int GetModifierCount(Type type)
    {
        return modifiers[type].Count;
    }
    public bool ContainsModifier(EntityModifier modifier)
    {
        Type type = modifier.GetType();

        if (modifiers.ContainsKey(type))
        {
            for (int i = 0; i < modifiers[type].Count; i++)
            {
                if (modifiers[type][i] == modifier)
                    return true;
            }
        }

        return false;
    }
    public bool ContainsModifier(Type modifier)
    {
        if (modifiers.ContainsKey(modifier))
        {
            return modifiers[modifier].Count > 0;
        }

        return false;
    }
    public void AddModifier(Type type, float duration = Mathf.Infinity)
    {
        EntityModifier modifier = (EntityModifier)ScriptableObject.CreateInstance(type);

        AddModifier(modifier, duration);
    }
    public void AddModifier(EntityModifier modifier, float duration = Mathf.Infinity)
    {
        Type type = modifier.GetType();

        if (!modifiers.ContainsKey(type))
        {
            ModifierList list = new ModifierList(type);
            modifiers.Add(type, list);
        }

        modifiers[type].Add(modifier, duration);
        CallEvent(type);
    }
    public void RemoveModifier(EntityModifier modifier)
    {
        modifiers[modifier.GetType()].Remove(modifier);
    }
    protected virtual void Update()
    {
        List<Type> allTypes = new List<Type>(modifiers.Keys);

        foreach (Type type in allTypes)
        {
            if (modifiers[type].Tick() || dirtyModifiers.Contains(type))
            {
                CallEvent(type);
            }

            if (modifiers[type].Count <= 0)
                modifiers.Remove(type);
        }

        dirtyModifiers.Clear();
    }
    
    private class ModifierList
    {
        public ModifierList(EntityModifier modifier)
        {
            modifierType = modifier.GetType();
        }
        public ModifierList(Type modifierType)
        {
            this.modifierType = modifierType;
        }

        public EntityModifier this[int index] => modifiers[index].Modifier;

        private readonly Type modifierType = default;
        private List<ModifierContainer> modifiers = new List<ModifierContainer>();
            
        public int Count => modifiers.Count;
        
        public void Add(EntityModifier modifier, float duration)
        {
            modifiers.Add(new ModifierContainer(modifier, duration));
        }
        public void Remove(EntityModifier modifier)
        {
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if(modifiers[i].Modifier == modifier)
                    modifiers.RemoveAt(i);
            }
        }
        /// <returns>Whether the state was altered</returns>
        public bool Tick()
        {
            bool stateAltered = false;
            
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if(modifiers[i].Tick())
                {
                    modifiers.RemoveAt(i);
                    stateAltered = true;
                }
            }

            return stateAltered;
        }
        
        public static implicit operator Type(ModifierList list)
        {
            return list.modifierType;
        }
    }
    private class ModifierContainer
    {
        public ModifierContainer(EntityModifier modifier, float duration)
        {
            Modifier = modifier;
            timeLeft = duration;
        }

        public readonly EntityModifier Modifier;
        public bool IsFinished => timeLeft <= 0;

        private float timeLeft;

        public bool Tick()
        {
            timeLeft -= Time.deltaTime;

            return IsFinished;
        }
    }
}
