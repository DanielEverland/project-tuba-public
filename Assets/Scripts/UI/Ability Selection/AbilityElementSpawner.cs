using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityElementSpawner : MonoBehaviour
{
    [SerializeField]
    private AbilityElement elementPrefab = default;
    [SerializeField]
    private AbilityCollection abilityCollection = default;
    [SerializeField]
    private Transform parent = default;

    private void Start()
    {
        foreach (Ability ability in abilityCollection)
        {
            AbilityElement element = Instantiate(elementPrefab);
            element.transform.SetParent(parent);

            element.Initialize(ability);
        }
    }
}
