using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityElement : MonoBehaviour
{
    [SerializeField]
    private ObjectSelectionPanel panelPrefab = default;

    protected Ability Ability { get; private set; }

    public void Initialize(Ability ability)
    {
        Ability = ability;
    }
    public void ChangeBehaviour()
    {
        Change(AbilityPartType.Behaviour);
    }
    public void ChangeAction()
    {
        Change(AbilityPartType.Action);
    }
    public void Change(AbilityPartType type)
    {
        ObjectSelectionPanel panelInstance = Instantiate(panelPrefab, transform.root);
        IEnumerable<IObjectDescription> entries = null;

        switch (type)
        {
            case AbilityPartType.Behaviour:
                entries = AbilityLoader.Behaviour;
                break;
            case AbilityPartType.Action:
                entries = AbilityLoader.Action;
                break;
            default:
                throw new System.NotImplementedException(type.ToString());
        }

        panelInstance.Initialize(entries, x =>
        {
            Ability.EquipPart((AbilityPart)x);
        });
    }
}
