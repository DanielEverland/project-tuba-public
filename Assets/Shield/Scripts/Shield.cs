using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Shields spawns a protective bubble around an entity
/// </summary>
public abstract class Shield : MonoBehaviour
{
    [SerializeField]
    protected Entity entity = default;
    [SerializeField]
    private ShieldPingType pingType = ShieldPingType.All;
    [SerializeField]
    private Color pingEffectColor = Color.white;
    [SerializeField]
    private FloatReference pingEffectDelayPerLayer = new FloatReference(0.05f);
    [SerializeField]
    protected UIntReference maxLayers = new UIntReference(3);
    [SerializeField]
    private UIntReference startLayers = new UIntReference(3);
    [SerializeField]
    private ShieldEffectElement layerPrefab = default;
    [SerializeField]
    private FloatReference minRadius = new FloatReference(1);
    [SerializeField]
    private FloatReference shieldRadiusInterval = new FloatReference(1);
    [SerializeField]
    private UnityEvent shieldsUpdated = new UnityEvent();

    public Entity Entity => entity;
    public UnityEvent ShieldsUpdates => shieldsUpdated;
    public uint CurrentLayers { get; set; }
    public bool CanAddMoreLayers => CurrentLayers < maxLayers.Value;
    
    private List<ShieldEffectElement> activeLayers = new List<ShieldEffectElement>();

    protected virtual void Awake()
    {
        CurrentLayers = startLayers.Value;
    }
    protected virtual void Start()
    {
        UpdateShield();

        entity.Health.AddOnDeathListener(OwnerDied);
        entity.Health.AddOnHitListener(OnHit);
    }
    
    
    [ContextMenu("Add Layer")]
    public void AddLayer()
    {
        CurrentLayers = (uint)Mathf.Min(maxLayers.Value, CurrentLayers + 1);

        UpdateShield();
    }
    [ContextMenu("Remove Layer")]
    public void RemoveLayer()
    {
        CurrentLayers = (uint)Mathf.Max(0, CurrentLayers - 1);

        UpdateShield();
    }
    public float GetShieldRadius(int index)
    {
        return minRadius.Value + shieldRadiusInterval.Value * index;
    }
    private void UpdateShield()
    {
        UpdateModifiers();
        UpdateShieldIndicators();

        shieldsUpdated.Invoke();
    }
    protected abstract void UpdateModifiers();
    protected virtual void UpdateShieldIndicators()
    {
        while (activeLayers.Count < CurrentLayers)
        {
            SpawnLayer(activeLayers.Count);
        }

        while (activeLayers.Count > CurrentLayers)
        {
            DestroyLayer();
        }
    }
    protected virtual void SpawnLayer(int layerIndex)
    {
        ShieldEffectElement layer = ShieldEffectElement.Instantiate(layerPrefab);
        layer.Initialize(GetShieldRadius(layerIndex));
        layer.transform.SetParent(transform);

        activeLayers.Add(layer);
    }
    protected virtual void DestroyLayer()
    {
        int layerToDestroy = activeLayers.Count - 1;

        Destroy(activeLayers[layerToDestroy].gameObject);
        activeLayers.RemoveAt(layerToDestroy);
    }
    protected void OnHit()
    {
        PingShield();
    }
    protected void PingShield()
    {
        switch (pingType)
        {
            case ShieldPingType.None:
                break;
            case ShieldPingType.All:
                StartCoroutine(PingAllLayers());
                break;
            case ShieldPingType.OuterMost:
                PingOutermostLayer();
                break;
            case ShieldPingType.InnerMost:
                PingInnermostLayer();
                break;
            default:
                throw new System.NotImplementedException();
        }        
    }
    protected IEnumerator PingAllLayers()
    {
        for (int i = 0; i < activeLayers.Count; i++)
        {
            activeLayers[i].PingDamageEffect(pingEffectColor);

            yield return new WaitForSeconds(pingEffectDelayPerLayer.Value);
        }
    }
    protected void PingOutermostLayer()
    {
        GetOutermostElement().PingDamageEffect(pingEffectColor);
    }
    protected void PingInnermostLayer()
    {
        GetInnerMostElement().PingDamageEffect(pingEffectColor);
    }
    protected ShieldEffectElement GetInnerMostElement()
    {
        return activeLayers[0];
    }
    protected ShieldEffectElement GetOutermostElement()
    {
        return activeLayers[activeLayers.Count - 1];
    }
    protected void OwnerDied()
    {
        DestroyShield();
    }
    protected void DestroyShield()
    {
        DestroyAllEffects();
        Destroy(this);
    }
    protected void DestroyAllEffects()
    {
        for (int i = 0; i < activeLayers.Count; i++)
        {
            Destroy(activeLayers[i].gameObject);
        }

        activeLayers.Clear();
    }
    private void OnValidate()
    {
        if (entity == null)
        {
            if(gameObject.TryGetEntity(out Entity entity))
                this.entity = entity;
        }            
    }

    private enum ShieldPingType
    {
        None = 0,

        All = 1,
        OuterMost = 2,
        InnerMost = 3,
    }
}
