using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys gameobject after a set amount of time
/// </summary>
public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float delay = 1;

    private void Awake()
    {
        StartCoroutine(DestroySelf());
    }
    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
