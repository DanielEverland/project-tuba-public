[System.Serializable]
public sealed class AbilityReference : BaseReference<Ability, AbilityVariable>
{
    public AbilityReference() : base() { }
    public AbilityReference(Ability value) : base(value) { }
}