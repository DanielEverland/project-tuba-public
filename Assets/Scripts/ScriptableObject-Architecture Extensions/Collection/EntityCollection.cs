using UnityEngine;

[CreateAssetMenu(
    fileName = "EntityCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Entity",
    order = 120)]
public class EntityCollection : Collection<Entity>
{
}