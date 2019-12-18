using UnityEngine;

public class SubdividerSpeedPropegator : MonoBehaviour
{
    [SerializeField]
    private SubdividerElement subdivider = null;
    [SerializeField]
    private FloatReference baseSpeed = null;
    [SerializeField]
    private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0.3f, 1, 1);
    [SerializeField]
    private AIMovement movement = default;

    private void Start()
    {
        movement.MovementSpeed = baseSpeed.Value * speedCurve.Evaluate((float)subdivider.CurrentLevel / subdivider.MaxSubdivides);
    }
    private void OnValidate()
    {
        if (subdivider == null)
            subdivider = GetComponent<SubdividerElement>();
    }
}