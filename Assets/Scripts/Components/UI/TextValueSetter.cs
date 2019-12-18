using TMPro;
using UnityEngine;

public class TextValueSetter : MonoBehaviour
{
    [SerializeField]
    private Object variable = null;
    [SerializeField]
    private TMP_Text textTarget = null;
    [SerializeField]
    private bool runInEditor = false;

    private void Update()
    {
        textTarget.text = variable.ToString();
    }
    private void OnValidate()
    {
        if (textTarget == null)
            textTarget = GetComponent<TMP_Text>();

        if (runInEditor && textTarget != null && variable != null)
            Update();
    }
}