using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = Utility.MenuItemRoot + "Upgrade", order = 107)]
public class UpgradeObject : ScriptableObject, IObjectDescription
{
    [SerializeField]
    private List<UpgradeModifier> modifiers = new List<UpgradeModifier>();
    [SerializeField]
    private string description = "NO DESCRIPTION";

    public string Name => name;
    public string Description => description;

    public List<UpgradeModifier> Modifiers => modifiers;
}
