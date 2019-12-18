using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeElement : MonoBehaviour
{
    [SerializeField]
    private ObjectSelectionPanel panelPrefab = default;
    [SerializeField]
    private UpgradeReference selectedUpgrade = default;
    
    public void Change()
    {
        ObjectSelectionPanel panelInstance = Instantiate(panelPrefab, transform.root);
        panelInstance.Initialize(UpgradeLoader.Upgrades, x => SetNewUpgrade(x as UpgradeObject), true);
    }
    private void SetNewUpgrade(UpgradeObject newUpgrade)
    {
        if (newUpgrade == selectedUpgrade.Value)
            return;

        selectedUpgrade.Value = newUpgrade;
        if (newUpgrade == null)
        {
            Upgrades.UnequipUpgrade();
            return;
        }

        Upgrades.EquipUpgrade(newUpgrade);
    }
}
