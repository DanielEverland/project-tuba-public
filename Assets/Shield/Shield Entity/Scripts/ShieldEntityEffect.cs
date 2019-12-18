using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect used when adding or removing layers to a <see cref="Shield"/> using <see cref="ShieldEntitySpawner"/>
/// </summary>
public class ShieldEntityEffect : MonoBehaviour
{
    private Task currentTask;
    
    public void EnterOrbit(ShieldEntitySpawner entitySpawner, int shieldLayer)
    {
        EnterOrbitTask orbitTask = new EnterOrbitTask(gameObject, entitySpawner, shieldLayer);
        currentTask = orbitTask;
    }
    public void ExitOrbit(ShieldEntitySpawner entitySpawner, Entity prefab)
    {
        ExitOrbitTask orbitTask = new ExitOrbitTask(gameObject, entitySpawner, prefab);
        currentTask = orbitTask;
    }
    private void Update()
    {
        if (PauseState.IsPaused)
            return;

        if (currentTask == null)
            throw new System.NullReferenceException($"{nameof(ShieldEntityEffect)} doesn't have a task!");

        currentTask.ExecuteTask();

        if (currentTask.ShouldBeDestroyed)
            Destroy(gameObject);
    }

    private abstract class Task
    {
        public Task(GameObject gameObject, ShieldEntitySpawner entitySpawner)
        {
            this.gameObject = gameObject;
            this.entitySpawner = entitySpawner;
        }

        protected readonly GameObject gameObject;
        protected readonly ShieldEntitySpawner entitySpawner;

        protected Shield Shield => entitySpawner.Shield;

        public bool ShouldBeDestroyed { get; protected set; } = false;

        public abstract void ExecuteTask();
    }
    private class ExitOrbitTask : Task
    {
        public ExitOrbitTask(GameObject thisGameObject, ShieldEntitySpawner entitySpawner, Entity prefab) : base(thisGameObject, entitySpawner)
        {
            this.prefab = prefab;
            this.targetPosition = GetTargetPosition();
        }

        private readonly Entity prefab;
        private readonly Vector2 targetPosition;

        private const float MinDistanceForSpawn = 0.01f;
        private const float LerpSpeed = 10;
        private const int SpawnDistanceMin = 2;
        private const int SpawnDistanceMax = 4;

        public override void ExecuteTask()
        {
            if(Vector2.Distance(gameObject.transform.position, targetPosition) > MinDistanceForSpawn)
            {
                MoveTowardsTarget();
            }
            else
            {
                SpawnEntity();

                ShouldBeDestroyed = true;
            }
        }
        private void MoveTowardsTarget()
        {
            gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, targetPosition, LerpSpeed * Time.deltaTime);
        }
        private void SpawnEntity()
        {
            Entity instance = GameObject.Instantiate(prefab);
            instance.transform.position = targetPosition;

            ShieldEntity element = instance.gameObject.AddComponent<ShieldEntity>();
            element.Initialize(entitySpawner, instance);
        }
        private Vector2 GetTargetPosition()
        {
            Vector2 direction = (gameObject.transform.position - entitySpawner.transform.position).normalized;
            int distance = Random.Range(SpawnDistanceMin, SpawnDistanceMax);

            return Utility.GetFarthestWalkablePositionInDirection(entitySpawner.transform.position, direction, distance);
        }
    }
    private class EnterOrbitTask : Task
    {
        public EnterOrbitTask(GameObject thisGameObject, ShieldEntitySpawner entitySpawner, int shieldLayer) : base(thisGameObject, entitySpawner)
        {
            this.shieldLayer = shieldLayer;

            Vector2 direction = (gameObject.transform.position - entitySpawner.transform.position).normalized;
            currentAngle = direction.GetAngle();
        }

        private readonly int shieldLayer;

        private const float LerpSpeed = 10;
        private const float OrbitSpeed = 100;

        private float currentAngle;

        public override void ExecuteTask()
        {
            currentAngle += OrbitSpeed * Time.deltaTime;
            float distanceFromOrbit = GetDistanceFromOrbit();
            Vector2 orbitalPosition = GetOrbitalPosition();

            LerpToPosition(orbitalPosition);
        }
        private float GetDistanceFromOrbit()
        {
            return Vector2.Distance(gameObject.transform.position, GetOrbitalPosition());
        }
        private Vector2 GetOrbitalPosition()
        {
            float targetRadius = Shield.GetShieldRadius(shieldLayer);
            Vector2 currentDirection = currentAngle.GetDirection();
            Vector2 offset = currentDirection * targetRadius;
            offset = Utility.ScaleToOrthographicVector(offset);

            return (Vector2)entitySpawner.transform.position + offset;
        }
        private void LerpToPosition(Vector2 targetPosition)
        {
            gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, targetPosition, LerpSpeed * Time.deltaTime);
        }
    }
}
