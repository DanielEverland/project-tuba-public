using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Teleports the player to the boss scene 
/// </summary>
public class BossFightTeleporter : ChangeScene
{
    [SerializeField]
    private EntityVariable playerEntity = default;

    [Space()]

    [SerializeField]
    private EntityCollection allEnemies = default;
    [SerializeField]
    private GameObject unavailableGameObject = default;
    [SerializeField]
    private GameObject availableGameObject = default;
    [SerializeField]
    private UnityEvent onBecameAvailable = new UnityEvent();

    private int startCount;
    private bool isAvailable = false;

    private const float MinRatio = 0.1f;

    private void Start()
    {
        startCount = allEnemies.Count;

        if (startCount <= 0)
            BecomeAvailable();
    }
    private void Update()
    {
        EvaluateIfAvailable();
    }
    private void EvaluateIfAvailable()
    {
        if (isAvailable)
            return;

        float aliveEnemyRatio = (float)allEnemies.Count / startCount;

        if (aliveEnemyRatio <= MinRatio)
            BecomeAvailable();
    }
    private void BecomeAvailable()
    {
        isAvailable = true;

        unavailableGameObject.SetActive(false);
        availableGameObject.SetActive(true);

        onBecameAvailable.Invoke();
    }
    public void EvaluateEntityCollision(Collider2D collider)
    {
        if(collider.gameObject.TryGetEntity(out Entity entity))
        {
            if(entity == playerEntity.Value)
            {
                LoadScene();
            }
        }
    }
}
