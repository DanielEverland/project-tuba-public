using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

/// <summary>
/// Editor for <see cref="Interactable"/>
/// </summary>
[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    private SerializedProperty entityfield;
    private SerializedProperty inheritField;
    private SerializedProperty tagsField;

    private AnimBool inheritAnim;
    private AnimBool tagsAnim;

    private bool ShowInheritField => entityfield.objectReferenceValue != null;
    private bool ShowTagsField => !ShowInheritField || inheritField.boolValue == false;

    private void OnEnable()
    {
        entityfield = serializedObject.FindProperty("entity");
        inheritField = serializedObject.FindProperty("inheritEntityTags");
        tagsField = serializedObject.FindProperty("tags");

        inheritAnim = new AnimBool(ShowInheritField);
        inheritAnim.valueChanged.AddListener(Repaint);

        tagsAnim = new AnimBool(ShowTagsField);
        tagsAnim.valueChanged.AddListener(Repaint);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        inheritAnim.target = ShowInheritField;
        tagsAnim.target = ShowTagsField;

        DrawEntityField();
        DrawInheritField();
        DrawTagsField();

        serializedObject.ApplyModifiedProperties();
    }
    private void DrawEntityField()
    {
        EditorGUILayout.PropertyField(entityfield);
    }
    private void DrawInheritField()
    {
        using (var inheritFadeGroup = new EditorGUILayout.FadeGroupScope(inheritAnim.faded))
        {
            if (inheritFadeGroup.visible)
                EditorGUILayout.PropertyField(inheritField);
        }
    }
    private void DrawTagsField()
    {
        using (var tagsFadeGroup = new EditorGUILayout.FadeGroupScope(tagsAnim.faded))
        {
            if (tagsFadeGroup.visible)
                EditorGUILayout.PropertyField(tagsField);
        }
    }
}
