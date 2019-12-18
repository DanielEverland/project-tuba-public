using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField]
    private FloatReference currentHealth = null;
    [SerializeField]
    private FloatReference healthPerSegment = null;
    [SerializeField]
    private IntReference segmentCount = null;
    [SerializeField]
    private FloatReference lerpSpeed = new FloatReference(10);
    [SerializeField]
    private BoolReference lerpIncreases = new BoolReference(true);
    [SerializeField]
    private BoolReference lerpDecreases = new BoolReference(true);
    [SerializeField]
    private Image segmentElement = null;
    [SerializeField]
    private Transform layoutParent = null;
    
    protected Image[] Segments { get; set; }
    protected float LerpValue { get; set; }
    protected bool IsLerping { get; set; }
    protected float PreviousHealth { get; set; }

    public virtual Image this[int index]
    {
        get
        {
            return Segments[index];
        }
    }
    protected virtual void Awake()
    {
        SpawnSegments();
        PollDifferences();
    }
    protected virtual void Update()
    {
        PollDifferences();

        Lerp();
        DrawSegments();

        CacheHealth();
    }
    protected virtual void PollDifferences()
    {
        if (currentHealth.Value != PreviousHealth)
        {
            IsLerping = false;

            if (currentHealth.Value > PreviousHealth && lerpIncreases.Value)
                IsLerping = true;

            if (currentHealth.Value < PreviousHealth && lerpDecreases.Value)
                IsLerping = true;
        }
    }
    protected virtual void Lerp()
    {
        LerpValue = Mathf.Lerp(LerpValue, currentHealth.Value, lerpSpeed.Value * Time.deltaTime);
    }
    protected virtual void CacheHealth()
    {
        PreviousHealth = currentHealth.Value;
    }
    protected virtual void DrawSegments()
    {
        for (int i = 0; i < Segments.Length; i++)
        {
            float healthValue = IsLerping ? LerpValue : currentHealth.Value;

            float minValue = healthPerSegment.Value * i;
            float maxValue = healthPerSegment.Value * (i + 1);
            float percentageValue = Mathf.Clamp01((healthValue - minValue) / (maxValue - minValue));

            this[i].fillAmount = percentageValue;
        }
    }
    protected virtual void SpawnSegments()
    {
        // Remove all existing segments, if any
        for (int i = 0; i < Segments?.Length; i++)
        {
            Destroy(this[i].gameObject);
        }

        Segments = new Image[segmentCount.Value];
        for (int i = 0; i < segmentCount.Value; i++)
        {
            Image image = Instantiate(segmentElement);
            image.transform.SetParent(layoutParent);

            Segments[i] = image;
        }
    }
}