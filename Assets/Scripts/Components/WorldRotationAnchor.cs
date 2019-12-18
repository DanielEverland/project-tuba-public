using UnityEngine;

public class WorldRotationAnchor : MonoBehaviour
{
    [SerializeField]
    private FloatReference angle = new FloatReference(0);

    private void Update() => transform.eulerAngles = Vector3.forward * angle.Value;
}