using UnityEngine;

[RequireComponent(typeof(SubdividerElement))]
public class SubdividerOnDeath : MonoBehaviour
{
    [SerializeField]
    private Health healthComponent = null;
    [SerializeField]
    private SubdividerElement subdividerElement = null;
    [SerializeField]
    private SubdividerElement prefab = null;
    [SerializeField]
    private IntReference childrenToSpawn = null;
        
    public void DoSubdivide()
    {
        SpawnChildren();

        healthComponent.Die();
    }
    private void SpawnChildren()
    {
        if (subdividerElement.CurrentLevel >= subdividerElement.MaxSubdivides)
            return;

        for (int i = 0; i < childrenToSpawn.Value; i++)
        {
            SpawnChild();
        }
    }
    private void SpawnChild()
    {
        SubdividerElement spawnedChild = Instantiate(prefab);

        spawnedChild.Initialize(subdividerElement.CurrentLevel + 1, subdividerElement.Name);
    }
    protected virtual void OnValidate()
    {
        if (healthComponent == null)
            healthComponent = GetComponent<Health>();

        if (subdividerElement == null)
            subdividerElement = GetComponent<SubdividerElement>();
    }
}