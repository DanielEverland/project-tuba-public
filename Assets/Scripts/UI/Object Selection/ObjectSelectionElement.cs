using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectSelectionElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleTextElement = default;
    [SerializeField]
    private TMP_Text descriptionTextElement = default;

    protected IObjectSelectionPanel Owner { get; private set; }
    protected IObjectDescription Object { get; private set; }

    public void Initialize(IObjectSelectionPanel owner, IObjectDescription obj)
    {
        Owner = owner;
        Object = obj;

        if(obj != null)
        {
            titleTextElement.text = obj.Name;
            descriptionTextElement.text = obj.Description;
        }
        else
        {
            titleTextElement.text = "None";
            descriptionTextElement.text = string.Empty;
        }
    }
    public void Selected()
    {
        Owner.SelectObject(Object);
    }
}
