using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjusts components in the pool to fit a certain radius
/// </summary>
public class Pool : MonoBehaviour
{
    public float Radius
    {
        get
        {
            return radius;
        }
        set
        {
            radius = value;

            UpdateComponents();
        }
    }
    private float radius;

    [SerializeField]
    private FloatReference duration = new FloatReference(1);
    [SerializeField]
    private FloatReference destructionAnimationSpeed = new FloatReference(1);
    [SerializeField]
    private List<Projector> projectors = new List<Projector>();
    [SerializeField]
    private List<CircleCollider2D> circleColliders = new List<CircleCollider2D>();

    private float aliveTime = 0;

    private void Update()
    {
        aliveTime += Time.deltaTime;

        if (aliveTime >= duration.Value)
        {
            Radius = Mathf.Lerp(Radius, 0, destructionAnimationSpeed.Value * Time.deltaTime);

            if (Radius < 0.01f)
                Destroy(gameObject);
        }
    }
    private void UpdateComponents()
    {
        SetProjectorsRadius();
        SetCircleCollidersRadius();
    }
    private void SetProjectorsRadius()
    {
        foreach (Projector projector in projectors)
        {
            projector.orthographicSize = radius;
        }
    }
    private void SetCircleCollidersRadius()
    {
        foreach (CircleCollider2D circleCollider in circleColliders)
        {
            circleCollider.radius = radius;
        }
    }
}
