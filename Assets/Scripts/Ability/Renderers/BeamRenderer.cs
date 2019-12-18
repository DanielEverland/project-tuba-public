using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamRenderer : AbilityRenderer
{
    [SerializeField]
    private new LineRenderer renderer = null;
    [SerializeField]
    private bool useRaycast = false;
    [SerializeField]
    private float maxDistance = 100;
    [SerializeField]
    private LayerMask layerMask = (int)Layer.AllEnvironment;
    
    private void Awake()
    {
        renderer.positionCount = 2;
        renderer.enabled = false;
    }
    private void Update()
    {
        UpdateBeam();
    }
    private void UpdateBeam()
    {
        if (useRaycast)
        {
            Vector3 direction = (TargetPosition - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

            if (hit)
            {
                SetLineRendererPositions(hit.point);
            }
            else
            {
                SetLineRendererPositions(transform.position + direction * maxDistance);
            }
        }
        else
        {
            SetLineRendererPositions(TargetPosition);
        }
    }
    public override void OnStarted()
    {
        renderer.enabled = true;
    }    
    public override void UpdatePosition(Vector3 position)
    {
        base.UpdatePosition(position);

        UpdateBeam();
    }
    public override void OnEnded()
    {
        renderer.enabled = false;
    }
    public override void UpdateColor(Color color)
    {
        renderer.startColor = color;
        renderer.endColor = color;
    }
    private void SetLineRendererPositions(Vector2 endPosition)
    {
        renderer.SetPosition(0, transform.position);
        renderer.SetPosition(1, endPosition);
    }
}
