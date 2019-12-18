using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private EntityReference target = null;
    [SerializeField]
    private FloatReference zDistance = new FloatReference(10);
    [SerializeField]
    private FloatReference lerpSpeed = new FloatReference(10);
    
    private void Awake()
    {
        transform.eulerAngles = new Vector3(-Utility.OrthogonalCameraAngle, 0, 0);
    }
    protected virtual void LateUpdate()
    {
        Vector3 targetPosition = target.Value.transform.position;
        targetPosition += transform.TransformDirection(Vector3.back) * zDistance.Value;

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed.Value * Time.deltaTime);
    }
}