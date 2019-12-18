public interface IPersistentAbilityObject : IAbilityObject
{
    bool ShouldBeDestroyed();
    void Destroy();
}
