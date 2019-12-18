using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.2f;
    [SerializeField]
    private float shakeIntensity = 1;

    protected bool IsShaking => Time.time - currentEntryStartTime < duration;

    private float currentEntryStartTime;

    public void AddForce()
    {
        currentEntryStartTime = Time.time;
    }
    private void LateUpdate()
    {
        if (IsShaking)
        {
            transform.position += Random.insideUnitSphere * shakeIntensity * Time.deltaTime;
        }
    }
}