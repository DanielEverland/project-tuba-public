using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Vector2 point;

    public Vector2 Point { get => point; set => point = value; }
    public float Radius { get => radius; set => radius = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public float CurrentAngle { get; set; }

    void Update()
    {
        CurrentAngle += rotationSpeed * Time.deltaTime;

        Vector2 newPosition = CurrentAngle.GetDirection() * radius;
        newPosition = point + Utility.ScaleToOrthographicVector(newPosition);

        transform.position = newPosition;
    }
}
