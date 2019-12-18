using UnityEngine;

public abstract class HealthPostProcessor : MonoBehaviour
{
    public abstract float ProcessMaxHealth(float maxHealth);
}