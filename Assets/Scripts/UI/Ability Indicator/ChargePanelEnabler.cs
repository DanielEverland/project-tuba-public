using UnityEngine;

public class ChargePanelEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject panel = null;
    [SerializeField]
    private AbilityReference selectedAbility = default;

    private void Update()
    {
        PollPanel(selectedAbility.Value);
    }
    public void PollPanel(Ability currentAbility)
    {
        Toggle(currentAbility.Behaviour is ChargedBehaviour);
    }
    private void Toggle(bool active)
    {
        panel.SetActive(active);
    }
}