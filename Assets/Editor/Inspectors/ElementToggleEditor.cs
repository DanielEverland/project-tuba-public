using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

[CustomEditor(typeof(ElementToggle))]
[CanEditMultipleObjects]
public class ElementToggleEditor : Editor
{
    private SerializedProperty targetProperty;
    private SerializedProperty autoSetStartState;
    private SerializedProperty startState;
    private SerializedProperty keyCode;

    private AnimBool _startStateAnim;

    private void OnEnable()
    {
        targetProperty = serializedObject.FindProperty("target");
        autoSetStartState = serializedObject.FindProperty("autoSetStartState");
        startState = serializedObject.FindProperty("startState");
        keyCode = serializedObject.FindProperty("keyCode");

        _startStateAnim = new AnimBool(autoSetStartState.boolValue);
        _startStateAnim.valueChanged.AddListener(Repaint);
    }
    public override void OnInspectorGUI()
    {
        //Target
        EditorGUILayout.ObjectField(targetProperty);

        if (targetProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Target cannot be null", MessageType.Error);
        }
        else if (targetProperty.objectReferenceValue == ((MonoBehaviour)target).gameObject)
        {
            EditorGUILayout.HelpBox("Target cannot be self\nThis will disallow key capture when target is disabled", MessageType.Warning);
        }

        //Keycode
        EditorGUILayout.PropertyField(keyCode);

        //Auto-set Start State
        EditorGUILayout.PropertyField(autoSetStartState);
        _startStateAnim.target = autoSetStartState.boolValue;

        using (var group = new EditorGUILayout.FadeGroupScope(_startStateAnim.faded))
        {
            if (group.visible)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(startState);

                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}