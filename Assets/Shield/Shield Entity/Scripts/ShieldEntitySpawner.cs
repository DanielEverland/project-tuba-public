using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns entities in return for a shield layer
/// </summary>
public class ShieldEntitySpawner : MonoBehaviour
{
    public Shield Shield = default;
    
    [SerializeField]
    private ShieldEntityEffect entityEffectPrefab = default;
    [SerializeField]
    private IntReference entitiesPerLayer = new IntReference(4);
    [SerializeField]
    private List<Entity> prefabs = default;

    private List<List<ShieldEntityEffect>> shieldEffectsInOrbit = new List<List<ShieldEntityEffect>>();
    private Queue<ShieldEntityEffect> shieldEffectsWaitingToGetShield = new Queue<ShieldEntityEffect>();
    private List<ShieldEntity> activeShieldEntities = new List<ShieldEntity>();

    private bool isDestroying = false;

    private void Start()
    {
        Shield.Entity.Health.AddOnDeathListener(DestroySpawner);
    }
    public void ShieldUpdated()
    {
        int currentLayers = shieldEffectsInOrbit.Count;

        if (Shield.CurrentLayers != currentLayers)
        {
            UpdateEffects();
        }      
    }
    public void AddShieldEntity(ShieldEntity shieldEntity)
    {
        activeShieldEntities.Add(shieldEntity);
    }
    private void UpdateEffects()
    {
        while (shieldEffectsInOrbit.Count < Shield.CurrentLayers)
        {
            int currentLayer = shieldEffectsInOrbit.Count;

            EnsureEffectsLayerExists(currentLayer);

            List<ShieldEntityEffect> effectsLayer = shieldEffectsInOrbit[currentLayer];
            while (effectsLayer.Count < entitiesPerLayer.Value)
            {
                SpawnEffectInLayer(currentLayer);
            }
        }

        while (shieldEffectsInOrbit.Count > Shield.CurrentLayers)
        {
            DestroyEffectsInLayer(shieldEffectsInOrbit.Count - 1);
        }

        UpdateEntityEffectLayerIndex(shieldEffectsWaitingToGetShield, shieldEffectsInOrbit.Count);
    }
    private void UpdateEntityEffectLayerIndex(IEnumerable<ShieldEntityEffect> collection, int shieldLayer)
    {
        foreach (ShieldEntityEffect entityEffect in collection)
        {
            entityEffect.EnterOrbit(this, shieldLayer);
        }
    }
    private void SpawnEffectInLayer(int layerIndex)
    {
        for (int i = 0; i < entitiesPerLayer.Value; i++)
        {
            ShieldEntityEffect effectInstance = SpawnEntity();
            effectInstance.transform.position = GetOrbitalPositionBasedOnIndex(layerIndex + 1, i);
            effectInstance.EnterOrbit(this, layerIndex);

            AddShieldEffectToLayer(effectInstance, layerIndex);
        }
    }
    private void DestroyEffectsInLayer(int layerIndex)
    {
        List<ShieldEntityEffect> shieldLayerEffects = shieldEffectsInOrbit[layerIndex];
        for (int i = 0; i < shieldLayerEffects.Count; i++)
        {
            ShieldEffectExitOrbit(shieldLayerEffects[i]);
        }

        shieldEffectsInOrbit.RemoveAt(layerIndex);
    }
    private void ShieldEffectExitOrbit(ShieldEntityEffect effect)
    {
        effect.ExitOrbit(this, prefabs.Random());
    }
    public void ElementWasDestroyed(ShieldEntity entity)
    {
        if (isDestroying)
            return;

        activeShieldEntities.Remove(entity);

        ShieldEntityEffect effect = SpawnEntity();
        effect.transform.position = entity.transform.position;
        effect.EnterOrbit(this, (int)Shield.CurrentLayers);

        shieldEffectsWaitingToGetShield.Enqueue(effect);
        
        PollCreateShield();
    }
    private void PollCreateShield()
    {
        while (shieldEffectsWaitingToGetShield.Count >= entitiesPerLayer.Value && Shield.CanAddMoreLayers)
        {
            ConsumeEffectsToCreateShield();
        }
    }
    private void ConsumeEffectsToCreateShield()
    {
        for (int i = 0; i < entitiesPerLayer.Value; i++)
        {
            ShieldEntityEffect effect = shieldEffectsWaitingToGetShield.Dequeue();
            AddShieldEffectToLayer(effect, (int)Shield.CurrentLayers);
        }

        Shield.AddLayer();
    }
    private void AddShieldEffectToLayer(ShieldEntityEffect effect, int layerIndex)
    {
        EnsureEffectsLayerExists(layerIndex);

        List<ShieldEntityEffect> shieldLayerEffects = shieldEffectsInOrbit[layerIndex];
        shieldLayerEffects.Add(effect);
    }
    private void EnsureEffectsLayerExists(int layerIndex)
    {
        while (layerIndex > shieldEffectsInOrbit.Count - 1)
        {
            shieldEffectsInOrbit.Add(new List<ShieldEntityEffect>());
        }
    }
    private ShieldEntityEffect SpawnEntity()
    {
        return HierarchyManager.Instantiate(entityEffectPrefab, HierarchyCategory.Effects);
    }
    private Vector2 GetRandomOrbitalPosition(int shieldLayer)
    {
        float targetRadius = Shield.GetShieldRadius(shieldLayer);
        float angle = Random.Range(0, 360);

        return GetOrbitalPositionFromDirection(angle.GetDirection(), targetRadius);
    }
    private Vector2 GetOrbitalPositionBasedOnIndex(int shieldLayer, int index)
    {
        float targetRadius = Shield.GetShieldRadius(shieldLayer);
        float percentage = (float)index / entitiesPerLayer.Value;
        float angle = 360 * percentage;

        return GetOrbitalPositionFromDirection(angle.GetDirection(), targetRadius);
    }
    private Vector2 GetOrbitalPositionFromDirection(Vector2 direction, float targetRadius)
    {
        direction = Utility.ScaleToOrthographicVector(direction);

        return (Vector2)Shield.transform.position + direction * targetRadius;
    }

    private void DestroySpawner()
    {
        isDestroying = true;

        DestroyAllEffects();

        Destroy(this);
    }
    private void DestroyAllEffects()
    {
        DestroyAllShieldEntities();
        DestroyActiveOrbitEffects();
        DestroyEffectsInQueue();
    }
    private void DestroyAllShieldEntities()
    {
        for (int i = 0; i < activeShieldEntities.Count; i++)
        {
            Destroy(activeShieldEntities[i].gameObject);
        }

        activeShieldEntities.Clear();
    }
    private void DestroyActiveOrbitEffects()
    {
        for (int i = 0; i < shieldEffectsInOrbit.Count; i++)
        {
            for (int j = 0; j < shieldEffectsInOrbit[i].Count; j++)
            {
                Destroy(shieldEffectsInOrbit[i][j].gameObject);
            }
        }

        shieldEffectsInOrbit.Clear();
    }
    private void DestroyEffectsInQueue()
    {
        while (shieldEffectsWaitingToGetShield.Count > 0)
        {
            Destroy(shieldEffectsWaitingToGetShield.Dequeue().gameObject);
        }
    }
}
