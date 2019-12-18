using UnityEngine;

[CreateAssetMenu(
    fileName = "EntityVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Entity",
    order = 120)]
public class EntityVariable : BaseVariable<Entity>
{
}