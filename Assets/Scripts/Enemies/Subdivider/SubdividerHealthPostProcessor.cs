using UnityEngine;

public class SubdividerHealthPostProcessor : HealthPostProcessor
{
    [SerializeField]
    private SubdividerElement subdividerElement;
    [SerializeField]
    private AnimationCurve healthMultiplier = AnimationCurve.Linear(0, 1, 1, 0.2f);
    
    protected int MaxLevel => subdividerElement.MaxSubdivides;
    protected float CurrentLevelPercentage => (float)subdividerElement.CurrentLevel / MaxLevel;

    public override float ProcessMaxHealth(float maxHealth)
    {
        return healthMultiplier.Evaluate(CurrentLevelPercentage) * maxHealth;
    }
    protected virtual void OnValidate()
    {
        if (subdividerElement == null)
            subdividerElement = GetComponent<SubdividerElement>();
    }
}