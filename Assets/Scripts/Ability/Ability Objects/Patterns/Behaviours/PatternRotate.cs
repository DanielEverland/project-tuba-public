using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates a pattern around its center axis
/// </summary>
public class PatternRotate : PatternBehaviour
{
    [SerializeField]
    private float speed = 5;

    private float currentAngle = 0;

    public override void Update()
    {
        currentAngle += speed * Time.deltaTime;
        Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, currentAngle));

        Evaluate(x =>
        {
            Pattern.SetElementPosition(x, matrix.MultiplyPoint(x.StartingPosition));
        });
    }
}
