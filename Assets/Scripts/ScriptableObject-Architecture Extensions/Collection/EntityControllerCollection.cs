using UnityEngine;

[CreateAssetMenu(
    fileName = "EntityControllerCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "EntityController",
    order = 120)]
public class EntityControllerCollection : Collection<EntityController>
{
}