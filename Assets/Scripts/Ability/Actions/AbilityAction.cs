using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuRoot + "Action", order = MenuOrder + 1)]
public class AbilityAction : AbilityPart
{
    [SerializeField]
    private List<Component> components = new List<Component>();
    
    public List<Component> Components => components;
    public override AbilityPartType Type => AbilityPartType.Action;

    public void Tick(AbilityData data)
    {
        if (components.Count == 0)
            Debug.LogWarning($"No components for {name}", this);

        foreach (Component component in components)
        {
            component.HandleEffectType(data);
        }
    }
    
    [System.Serializable]
    public abstract class Component : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private ActionEffectType effectType = ActionEffectType.Instantaneous;
        [SerializeField, HideInInspector]
        private float overTimeDuration = 1;
        [SerializeField, HideInInspector]
        private int totalTicks = 5;

        public int TotalTicks => totalTicks;
        public float EffectOverTimeDuration => overTimeDuration;
        public ActionEffectType EffectType => effectType;
        
        public abstract void Tick(AbilityData data);

        public void HandleEffectType(AbilityData data)
        {
            switch (effectType)
            {
                case ActionEffectType.Instantaneous:
                    HandleInstantenousEffect(data);
                    break;
                case ActionEffectType.OverTime:
                    HandleOverTimeEffect(data);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
        private void HandleInstantenousEffect(AbilityData data)
        {
            Tick(data);
        }
        private void HandleOverTimeEffect(AbilityData data)
        {
            ActionOverTime.Apply(this, data);
        }
    }
}
