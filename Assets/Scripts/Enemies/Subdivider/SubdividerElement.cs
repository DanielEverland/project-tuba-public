using UnityEngine;

public class SubdividerElement : MonoBehaviour
{
    [SerializeField]
    private IntReference maxSubdivides = null;
    [SerializeField]
    private AnimationCurve sizeProgression = AnimationCurve.Linear(0, 1, 1, 0.3f);
    [SerializeField]
    private Transform sizeTarget;
    
    public int MaxSubdivides => maxSubdivides.Value;
    public int CurrentLevel { get; private set; }
    public string Name { get; private set; }

    protected float CurrentLevelPercentage => CurrentLevel / maxSubdivides.Value;
    protected AnimationCurve SizeProgressionCurve => sizeProgression;

    protected virtual void Awake()
    {
        Name = name;
    }
    public virtual void Initialize(int subDivideLevel, string name)
    {
        CurrentLevel = subDivideLevel;
        Name = name;

        SetSize();
        SetName(subDivideLevel);
    }
    protected virtual void SetName(int level)
    {
        gameObject.name = $"{Name} ({level})";
    }
    protected virtual void SetSize()
    {
        float multiplier = SizeProgressionCurve.Evaluate(CurrentLevelPercentage);
        sizeTarget.localScale *= multiplier;
    }
    private void OnValidate()
    {
        if (sizeTarget == null)
            sizeTarget = GetComponent<Transform>();
    }
}