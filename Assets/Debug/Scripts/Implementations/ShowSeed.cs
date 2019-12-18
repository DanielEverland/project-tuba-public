using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Will display the seed used in level generation
/// </summary>
[CreateAssetMenu(fileName = "ShowSeed.asset", menuName = MenuRoot + "Show Seed", order = MenuOrder)]
public class ShowSeed : DebugElementToggle
{
    [SerializeField]
    private TMP_Text textElementPrefab = default;
    [SerializeField]
    private BoolVariable useSeedVariable = default;
    [SerializeField]
    private StringVariable seedValue = default;

    private TMP_Text instancedElement;

    public override void OnDebugElementEnable()
    {
        instancedElement = GameEvent.Instantiate(textElementPrefab);
        instancedElement.transform.SetParent(DebugInitializer.Canvas2D.transform, false);
        
        if (useSeedVariable.Value)
        {
            instancedElement.text = $"Seed: {seedValue.Value}";
        }
        else
        {
            if(Application.isEditor)
            {
                instancedElement.text = "Seed: N/A";
            }
            else
            {
                instancedElement.text = "Seed: N/A\nThis should not happen. Contact developer";
            }            
        }
    }
    public override void OnDebugElementDisable()
    {
        if (instancedElement != null)
            Destroy(instancedElement.gameObject);
    }
}
