using UnityEngine;

public class AddExplosiveForce : MonoBehaviour
{
    [SerializeField]
    private FloatReference radius = new FloatReference(10);
    [SerializeField]
    private FloatReference force = new FloatReference(50);
    [SerializeField]
    private ForceMode2D forceMode = ForceMode2D.Impulse;
    [SerializeField]
    private ForceFalloffMode falloffMode = ForceFalloffMode.Squared;
    [SerializeField]
    private LayerMask ignoreLayer = default;
    
    public void AddForce() => Utility.AddForceAll(transform.position, radius.Value, force.Value, forceMode, falloffMode, ~ignoreLayer);
}