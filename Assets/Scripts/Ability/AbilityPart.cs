using UnityEngine;

public abstract class AbilityPart : ScriptableObject, IObjectDescription
{
    [SerializeField]
    private string description = "NO DESCRIPTION";
    
    public Ability Ability { get; private set; }
    public string Name => name;
    public string Description => description;
    
    public abstract AbilityPartType Type { get; }

    public const string MenuRoot = Utility.MenuItemRoot + "Abilities/";
    public const int MenuOrder = 105;
    
    public virtual void OnStop() { }
    public virtual void OnFired() { }
    public virtual void OnEquipped() { }
    public virtual void OnUneqipped() { }
    public virtual T CreateInstance<T>(Ability ability) where T : AbilityPart
    {
        T instance = (T)Instantiate(this);
        instance.Ability = ability;

        return instance;
    }
}