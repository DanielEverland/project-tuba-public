using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRendererHandler : MonoBehaviour
{
    [SerializeField]
    private List<AbilityRenderer> renderers = default;

    public virtual void UpdatePosition(Vector2 position)   => QueryRenderers(x => x.UpdatePosition(position));
    public virtual void Tick()     => QueryRenderers(x => x.OnTick());
    public virtual void Start()    => QueryRenderers(x => x.OnStarted());
    public virtual void End()      => QueryRenderers(x => x.OnEnded());

    protected virtual void QueryRenderers(System.Action<AbilityRenderer> action)
    {
        for (int i = 0; i < renderers.Count; i++)
            action(renderers[i]);
    }
}
