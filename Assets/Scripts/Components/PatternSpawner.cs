using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will spawn a <see cref="Pattern"/> into the scene
/// </summary>
public class PatternSpawner : CallbackMonobehaviour
{
    [SerializeField]
    private AbilityAction patternAction = default;
    [SerializeField]
    private Pattern pattern = default;

    public override void OnRaised()
    {
        Ability ability = ScriptableObject.CreateInstance<Ability>();
        ability.EquipPart(patternAction);

        PatternObject obj = pattern.Spawn();

        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
    }
}
