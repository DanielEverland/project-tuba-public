using TMPro;
using UnityEngine;

public class TextIndexSetter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text targetText = null;
    [SerializeField]
    private BaseVariable variable = null;
    [SerializeField]
    private BaseCollection collection = null;
    [SerializeField, EnumFlags]
    private UpdateState updateState = UpdateState.Start;

    private void Awake()
    {
        if (!variable.Type.IsAssignableFrom(collection.Type))
            throw new System.ArgumentException("Type mismatch");

        if (updateState.HasFlag(UpdateState.Awake))
            UpdateText();
    }
    private void Start()
    {
        if (updateState.HasFlag(UpdateState.Start))
            UpdateText();
    }
    private void Update()
    {
        if (updateState.HasFlag(UpdateState.Update))
            UpdateText();
    }
    private void OnValidate()
    {
        if (targetText == null)
            targetText = GetComponent<TMP_Text>();
    }
    public void UpdateText() => targetText.text = collection.List.IndexOf(variable.BaseValue).ToString();

    [System.Flags, System.Serializable]
    private enum UpdateState
    {
        Awake = 0,
        Start = 2,
        Update = 4,
    }
}