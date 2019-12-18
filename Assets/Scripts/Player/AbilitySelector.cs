using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    [SerializeField]
    private AbilityVariable selectedAbility = null;
    [SerializeField]
    private AbilityCollection abilityInventory = null;
    [SerializeField]
    private AbilityGameEvent onAbilitySelectionUpdated = null;
    [SerializeField]
    private IntVariable ammoCapacity = null;
    [SerializeField]
    private FloatVariable chargeTime = null;
    [SerializeField]
    private FloatVariable cooldownTime = null;
    [SerializeField]
    private FloatVariable reloadTime = null;
    [SerializeField]
    private IntVariable seekersToFire = null;
    [SerializeField]
    private BoolVariable useCharge = null;
    [SerializeField]
    private IntVariable currentAmmo = null;
    [SerializeField]
    private bool useScrollWheel = true;
    [SerializeField]
    private bool useAlphaNumericKeys = true;
    [SerializeField]
    private bool useControllerBumpers = true;

    private void Start()
    {
        if (abilityInventory.Count > 0)
        {
            selectedAbility.Value = null;
            SelectAbility(0);
        }
    }
    private void Update()
    {
        if (useScrollWheel)
            PollScrollWheel();

        if (useAlphaNumericKeys)
            PollKeys();

        if (useControllerBumpers)
            PollControllerBumpers();

        SetProperties(selectedAbility);
    }
    private void PollControllerBumpers()
    {
        if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            SelectPrevious();
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            SelectNext();
        }
    }
    private void PollScrollWheel()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            SelectNext();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            SelectPrevious();
        }
    }
    private void PollKeys()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && abilityInventory.Count > 0)
        {
            SelectAbility(0);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && abilityInventory.Count > 1)
        {
            SelectAbility(1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3) && abilityInventory.Count > 2)
        {
            SelectAbility(2);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4) && abilityInventory.Count > 3)
        {
            SelectAbility(3);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5) && abilityInventory.Count > 4)
        {
            SelectAbility(4);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6) && abilityInventory.Count > 5)
        {
            SelectAbility(5);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha7) && abilityInventory.Count > 6)
        {
            SelectAbility(6);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha8) && abilityInventory.Count > 7)
        {
            SelectAbility(7);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha9) && abilityInventory.Count > 8)
        {
            SelectAbility(8);
        }
    }
    private void SelectNext()
    {
        int index = (GetIndex() + 1).Wrap(0, abilityInventory.Count - 1);

        SelectAbility(index);
    }
    private void SelectPrevious()
    {
        int index = (GetIndex() - 1).Wrap(0, abilityInventory.Count - 1);

        SelectAbility(index);
    }
    private int GetIndex()
    {
        return abilityInventory.IndexOf(selectedAbility.Value);
    }
    private void SelectAbility(int index)
    {
        SelectAbility(abilityInventory[index]);
    }
    private void SelectAbility(Ability newSelection)
    {
        if (selectedAbility.Value == newSelection)
            return;

        selectedAbility.Value = newSelection;

        SetProperties(newSelection);

        onAbilitySelectionUpdated.Raise(selectedAbility.Value);
    }
    private void SetProperties(Ability newSelection)
    {
        ammoCapacity.Value = newSelection.Behaviour.AmmoCapacity;
        cooldownTime.Value = newSelection.Behaviour.CooldownDuration;
        reloadTime.Value = newSelection.Behaviour.ReloadTime;
        seekersToFire.Value = newSelection.Behaviour.ObjectsToFire;
        currentAmmo.Value = newSelection.CurrentAmmo;

        if(newSelection.Behaviour is ChargedBehaviour chargeBehaviour)
        {
            useCharge.Value = true;
            chargeTime.Value = chargeBehaviour.ChargeTime;
        }
        else
        {
            useCharge.Value = false;
        }
    }
}