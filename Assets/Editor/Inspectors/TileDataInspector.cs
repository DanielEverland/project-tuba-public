using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor for <see cref="TileData"/>
/// </summary>
[CustomEditor(typeof(TileData))]
public class TileDataInspector : Editor
{
    private TileData Target => (TileData)target;
    private SerializedProperty brushProperty;
    private SerializedProperty brushMaterialsProperty;
    private SerializedProperty edgeMaterialProperty;
    private SerializedProperty castsShadowProperty;
    private SerializedProperty isWalkableProperty;

    private const float Spacing = 3;

    private void OnEnable()
    {
        SetupProperties();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawBrushProperty();
        DrawBrushMaterials();

        EditorGUILayout.PropertyField(edgeMaterialProperty);
        EditorGUILayout.PropertyField(castsShadowProperty);
        EditorGUILayout.PropertyField(isWalkableProperty);

        serializedObject.ApplyModifiedProperties();
    }
    private void DrawBrushProperty()
    {
        SerializedProperty uniqueIdentifierProperty = brushProperty.FindPropertyRelative("uniqueIdentifier");

        ushort uniqueIdentifier = (ushort)uniqueIdentifierProperty.intValue;
        int currentOptionIndex = BrushTypes.OrderedBrushes.IndexOf(BrushTypes.AllBrushes[uniqueIdentifier]);
        int newOptionIndex = EditorGUILayout.Popup("Brush", currentOptionIndex, BrushTypes.OrderedBrushes.Select(x => x.Name).ToArray());

        uniqueIdentifierProperty.intValue = (ushort)BrushTypes.OrderedBrushes[newOptionIndex].UniqueIdentifier;
    }
    private void DrawBrushMaterials()
    {
        SerializedProperty uniqueIdentifierProperty = brushProperty.FindPropertyRelative("uniqueIdentifier");

        ushort uniqueIdentifier = (ushort)uniqueIdentifierProperty.intValue;
        BrushBase brush = BrushTypes.AllBrushes[uniqueIdentifier];

        // Set materials lenght.
        brushMaterialsProperty.arraySize = brush.SubmeshNames.Length;

        for (int i = 0; i < brushMaterialsProperty.arraySize; i++)
        {
            SerializedProperty materialProperty = brushMaterialsProperty.GetArrayElementAtIndex(i);

            EditorGUILayout.PropertyField(materialProperty, new GUIContent(brush.SubmeshNames[i]));
        }
    }
    private void SetupProperties()
    {
        brushProperty = serializedObject.FindProperty("brush");
        brushMaterialsProperty = serializedObject.FindProperty("brushMaterials");
        edgeMaterialProperty = serializedObject.FindProperty("edgeMaterial");
        castsShadowProperty = serializedObject.FindProperty("castsShadows");
        isWalkableProperty = serializedObject.FindProperty("isWalkable");
    }
}