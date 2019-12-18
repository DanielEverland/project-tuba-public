using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AbilityObject))]
public class AbilityObjectDrawer : PropertyDrawer
{
    /// <summary> Cached style to use to draw the popup button. </summary>
    private GUIStyle popupStyle;

    private static readonly Dictionary<AbilityObject.Type, string> SupportedTypes = new Dictionary<AbilityObject.Type, string>()
    {
        { AbilityObject.Type.Projectile, "projectile" },
        { AbilityObject.Type.Beam, "beam" },
        { AbilityObject.Type.HitScan, "hitScanObject" },
        { AbilityObject.Type.Explosion, "explosionObject" },
        { AbilityObject.Type.DamageOnTouch, "damageOnTouch" },
    };
        
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty typeProperty = property.FindPropertyRelative("type");
        AbilityObject.Type type = (AbilityObject.Type)typeProperty.enumValueIndex;

        if (!SupportedTypes.ContainsKey(type))
            throw new System.NotImplementedException();

        SerializedProperty objectProperty = property.FindPropertyRelative(SupportedTypes[type]);

        // Calculate rect for configuration button
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        string[] options = System.Enum.GetNames(typeof(AbilityObject.Type));
        typeProperty.enumValueIndex = EditorGUI.Popup(buttonRect, typeProperty.enumValueIndex, options, popupStyle);
        EditorGUI.PropertyField(position, objectProperty, GUIContent.none);

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
