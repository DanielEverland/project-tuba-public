using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Displays the version object
/// </summary>
[CustomEditor(typeof(Version))]
public class VersionInspector : Editor
{
    private SerializedProperty qualifierProperty;
    private SerializedProperty majorProperty;
    private SerializedProperty minorProperty;
    private SerializedProperty maintenanceProperty;
    private SerializedProperty buildProperty;

    private float spaceUsed;
    private string fieldWidthRequirement = "999";

    private void OnEnable()
    {
        qualifierProperty = serializedObject.FindProperty("qualifier");
        majorProperty = serializedObject.FindProperty("major");
        minorProperty = serializedObject.FindProperty("minor");
        maintenanceProperty = serializedObject.FindProperty("maintenance");
        buildProperty = serializedObject.FindProperty("build");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawVersion();
        CopyToClipboardButton();

        serializedObject.ApplyModifiedProperties();
    }
    private void CopyToClipboardButton()
    {
        if(GUILayout.Button("Copy to clipboard"))
        {
            EditorGUIUtility.systemCopyBuffer = target.ToString();
        }
    }
    private void DrawVersion()
    {
        spaceUsed = 0;
        Rect rect = EditorGUILayout.GetControlRect();

        DrawLabel("Version ", rect);
        majorProperty.intValue = DrawIntField(majorProperty.intValue, rect);
        DrawLabel(".", rect);
        minorProperty.intValue = DrawIntField(minorProperty.intValue, rect);
        DrawLabel(".", rect);
        maintenanceProperty.intValue = DrawIntField(maintenanceProperty.intValue, rect);
        DrawLabel(" ", rect);
        DrawLabel($"({buildProperty.intValue.ToString()}) ", rect);

        DrawLabel("(", rect);
        qualifierProperty.stringValue = DrawStringField(qualifierProperty.stringValue, rect);
        DrawLabel(")", rect);
    }
    private void DrawLabel(string label, Rect parentRect)
    {
        float width = Styles.textFieldStyle.CalcSize(new GUIContent(label.ToString())).x;
        Rect rect = new Rect(parentRect)
        {
            x = parentRect.x + spaceUsed,
            width = width,
        };
        spaceUsed += width;

        EditorGUI.LabelField(rect, new GUIContent(label), Styles.labelStyle);
    }
    private int DrawIntField(int label, Rect parentRect)
    {
        float width = Styles.textFieldStyle.CalcSize(new GUIContent(fieldWidthRequirement)).x;
        Rect rect = new Rect(parentRect)
        {
            x = parentRect.x + spaceUsed,
            width = width,
        };
        spaceUsed += width;

        return EditorGUI.IntField(rect, label, Styles.textFieldStyle);
    }
    private string DrawStringField(string label, Rect parentRect)
    {
        float width = Styles.textFieldStyle.CalcSize(new GUIContent(label)).x;
        Rect rect = new Rect(parentRect)
        {
            x = parentRect.x + spaceUsed,
            width = width,
        };
        spaceUsed += width;

        return EditorGUI.TextField(rect, label, Styles.textFieldStyle);
    }

    [MenuItem("Window/Version #v", priority = -100)]
    private static void Select()
    {
        Selection.activeObject = Version.Default;
    }

    private class Styles
    {
        static Styles()
        {
            labelStyle = new GUIStyle(EditorStyles.label);
            textFieldStyle = new GUIStyle(EditorStyles.textField);
            textFieldStyle.alignment = TextAnchor.MiddleCenter;
        }

        public static GUIStyle labelStyle;
        public static GUIStyle textFieldStyle;
    }
}
