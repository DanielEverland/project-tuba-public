using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Property drawer for <see cref="BrushData"/>
/// </summary>
[CustomPropertyDrawer(typeof(BrushData))]
public class BrushDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty uniqueIdentifierProperty = property.FindPropertyRelative("uniqueIdentifier");

        ushort uniqueIdentifier = (ushort)uniqueIdentifierProperty.intValue;
        int currentOptionIndex = BrushTypes.OrderedBrushes.IndexOf(BrushTypes.AllBrushes[uniqueIdentifier]);
        int newOptionIndex = EditorGUI.Popup(position, "Brush", currentOptionIndex, BrushTypes.OrderedBrushes.Select(x => x.Name).ToArray());

        uniqueIdentifierProperty.intValue = (ushort)BrushTypes.OrderedBrushes[newOptionIndex].UniqueIdentifier;
    }
}
