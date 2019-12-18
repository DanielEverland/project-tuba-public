using UnityEngine;

[CreateAssetMenu(
    fileName = "HealthVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Health",
    order = 120)]
public class HealthVariable : BaseVariable<Health>
{
}