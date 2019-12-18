using UnityEngine;

public class ElementToggle : MonoBehaviour
{
    [SerializeField]
    private bool autoSetStartState = false;
    [SerializeField]
    private bool startState = false;
    [SerializeField]
    private GameObject target = null;
    [SerializeField]
    private KeyCode keyCode = KeyCode.None;

    private void Awake()
    {
        if (autoSetStartState)
            SetState(startState);
    }
    private void Update()
    {
        if (Input.GetKeyUp(keyCode))
        {
            try
            {
                SetState(!target.activeInHierarchy);
            }
            catch (System.NullReferenceException)
            {
                if (target == null)
                {
                    throw new System.NullReferenceException("Element Toggle Error: Target cannot be null");
                }

                throw;
            }
        }
    }
    private void EnsureTargetIsNotSelf()
    {
        if (target == gameObject)
        {
            Debug.LogError("Element Toggle Error: Target cannot be self. This will disallow event capture when disabled", this);
        }
    }
    private void SetState(bool active)
    {
        try
        {
            target.SetActive(active);
        }
        catch (System.NullReferenceException) when (target == null)
        {
            throw new System.NullReferenceException("Element Toggle Error: Target cannot be null");
        }
    }
}