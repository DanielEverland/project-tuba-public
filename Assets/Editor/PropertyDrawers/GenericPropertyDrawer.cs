using UnityEditor;
using UnityEngine;

public class GeneralPropertyDrawer : PropertyDrawer
{
    public static GeneralPropertyDrawer Drawer
    {
        get
        {
            if (drawer == null)
                drawer = new GeneralPropertyDrawer();

            return drawer;
        }
    }
    private static GeneralPropertyDrawer drawer;

    public bool SkipScriptField { get; set; } = true;

    protected virtual float SingleLineHeight => EditorGUIUtility.singleLineHeight;
    protected virtual float TopPadding => 2;
    protected virtual float Spacing => 2;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        OnGUI(position, serializedObject, property, label);
    }
    public void OnGUI(Rect position, SerializedObject serializedObject, SerializedProperty property, GUIContent label)
    {
        if (serializedObject == null)
            return;

        serializedObject.Update();

        position.height = SingleLineHeight;
        position.y += TopPadding;

        SerializedProperty element = serializedObject.GetIterator();
        // Skip the scripts field

        if (SkipScriptField)
            element.NextVisible(true);

        if (element.NextVisible(true))
        {
            do
            {
                if (element != null)
                    EditorGUI.PropertyField(position, element);

                position.y += SingleLineHeight;
                position.y += Spacing;
            }
            while (element.NextVisible(false));
        }

        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        return SingleLineHeight * ElementCount(serializedObject, property) + (Spacing * (ElementCount(serializedObject, property) - 1)) + TopPadding;
    }
    public float GetPropertyHeight(SerializedObject serializedObject, SerializedProperty property, GUIContent label)
    {
        return SingleLineHeight * ElementCount(serializedObject, property) + (Spacing * (ElementCount(serializedObject, property) - 1)) + TopPadding;
    }
    protected virtual int ElementCount(SerializedObject serializedObject, SerializedProperty property)
    {
        SerializedProperty prop = serializedObject.GetIterator();

        int i = 0;
        if (prop.NextVisible(true))
        {
            while (prop.NextVisible(false))
            {
                i++;
            }
        }

        if (!SkipScriptField)
            i++;

        return i;
    }
}