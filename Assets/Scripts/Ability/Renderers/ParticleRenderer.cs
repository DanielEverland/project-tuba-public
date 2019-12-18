using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRenderer : AbilityRenderer
{
    [SerializeField]
    private new ParticleSystem particleSystem = default;

    public override void OnStarted()
    {
        base.OnStarted();

        particleSystem.Play();
    }
    public override void OnEnded()
    {
        base.OnEnded();

        particleSystem.Stop();
    }
    public override void UpdateColor(Color color)
    {
        base.UpdateColor(color);

        var main = particleSystem.main;
        main.startColor = color;
    }
    public override void UpdatePosition(Vector3 position)
    {
        base.UpdatePosition(position);

        particleSystem.transform.position = position;
    }
}
