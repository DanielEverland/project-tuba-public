using UnityEngine;

/// <summary>
/// Ensures ability objects are setup correctly and ready for use
/// </summary>
public class AbilitySetup : MonoBehaviour
{
    [SerializeField]
    private Entity owner = default;
    [SerializeField]
    private AbilityVariable selectedAbility = null;
    [SerializeField]
    private AbilityCollection defaultInventory = null;
    [SerializeField]
    private AbilityCollection abilityInventory = null;
    [SerializeField]
    private IntReference abilitySlots = null;

    private void Awake()
    {
        abilityInventory.List.Clear();

        for (int i = 0; i < abilitySlots.Value; i++)
        {
            Ability ability = null;

            if(i < defaultInventory.Count)
            {
                ability = defaultInventory[i];
            }
            else
            {
                ability = Ability.CreateInstance<Ability>();
                ability.AssignRandomParts();
            }
            
            abilityInventory.Add(Ability.Instantiate(ability, owner));
        }

        selectedAbility.Value = abilityInventory[0];
    }
    private void OnValidate()
    {
        owner = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }
}