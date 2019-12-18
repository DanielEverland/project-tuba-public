using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

[CustomEditor(typeof(UpgradeObject))]
public class UpgradeObjectEditor : Editor
{
    private UpgradeObject Target => (UpgradeObject)target;
    private ScriptableObjectList<UpgradeModifier> reorderableList;
    private SerializedProperty descriptionProperty;

    private void OnEnable()
    {
        reorderableList = new ScriptableObjectList<UpgradeModifier>(serializedObject.FindProperty("modifiers"), Target.Modifiers);
        descriptionProperty = serializedObject.FindProperty("description");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(descriptionProperty);
        reorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
