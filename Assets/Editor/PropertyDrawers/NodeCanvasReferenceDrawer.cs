using ParadoxNotion.Design;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeCanvasReferenceDrawer : ObjectDrawer<BaseReference>
{
    /// <summary>
    /// Options to display in the popup to select constant or variable.
    /// </summary>
    private static readonly string[] popupOptions =
    {
        "Use Constant",
        "Use Variable"
    };

    private const BindingFlags variableBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

    public override BaseReference OnGUI(GUIContent content, BaseReference instance)
    {
        instance = EnsureInstanceExists(instance);

        Rect fieldRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

        FieldInfo useConstant = instance.GetType().GetField("_useConstant", variableBindingFlags);
        FieldInfo constantValue = instance.GetType().GetField("_constantValue", variableBindingFlags);
        FieldInfo variable = instance.GetType().GetField("_variable", variableBindingFlags);

        Rect labelRect = GetLabelRect(fieldRect);
        Rect buttonRect = GetButtonRect(labelRect);
        Rect valueRect = GetValueRect(fieldRect, buttonRect);

        EditorGUI.LabelField(labelRect, content);
        bool useConstantValue = ButtonField(buttonRect, (bool)useConstant.GetValue(instance));
        useConstant.SetValue(instance, useConstantValue);

        ValueField(valueRect, useConstantValue, instance, constantValue, variable);

        return instance;
    }

    private static void ValueField(Rect rect, bool useConstantValue, object instance, FieldInfo constantValue, FieldInfo variable)
    {
        if (useConstantValue)
        {
            DrawConstantValue(rect, instance, constantValue);
        }
        else
        {
            Object variableValue = (Object)variable.GetValue(instance);
            variableValue = EditorGUI.ObjectField(rect, variableValue, variable.FieldType, false);
            variable.SetValue(instance, variableValue);
        }
    }
    private static void DrawConstantValue(Rect rect, object instance, FieldInfo constantValue)
    {
        object value = constantValue.GetValue(instance);
        
        switch (value)
        {
            case double d:
                value = EditorGUI.DoubleField(rect, d);
                break;
            case float f:
                value = EditorGUI.FloatField(rect, f);
                break;
            case Gradient g:
                value = EditorGUI.GradientField(rect, g);
                break;
            case long l:
                value = EditorGUI.LongField(rect, l);
                break;
            case int i:
                value = EditorGUI.IntField(rect, i);
                break;
            case string s:
                value = EditorGUI.TextField(rect, s);
                break;
            case Layer l:
                value = EditorGUI.LayerField(rect, (int)l);
                break;
            case Rect r:
                value = EditorGUI.RectField(rect, r);
                break;
            case Object o:
                value = EditorGUI.ObjectField(rect, o, constantValue.FieldType, false);
                break;
            default:
                throw new System.NotImplementedException();
        }

        constantValue.SetValue(instance, value);
    }
    private static bool ButtonField(Rect rect, bool useConstant)
    {
        return EditorGUI.Popup(rect, useConstant ? 0 : 1, popupOptions, Styles.PopupStyle) == 0;
    }
    private static Rect GetValueRect(Rect fieldRect, Rect buttonRect)
    {
        return new Rect(fieldRect)
        {
            x = buttonRect.x + buttonRect.width,
            width = fieldRect.width - (buttonRect.x + buttonRect.width),
        };
    }
    private static Rect GetButtonRect(Rect labelRect)
    {
        return new Rect(labelRect)
        {
            x = labelRect.x + labelRect.width,
            y = labelRect.y + Styles.PopupStyle.margin.top,
            width = Styles.PopupStyle.fixedWidth + Styles.PopupStyle.margin.right
        };
    }
    private static Rect GetLabelRect(Rect fieldRect)
    {
        return new Rect(fieldRect)
        {
            width = EditorGUIUtility.labelWidth,
        };
    }

    private BaseReference EnsureInstanceExists(BaseReference instance)
    {
        if(instance == null)
            instance = (BaseReference)System.Activator.CreateInstance(fieldInfo.FieldType);

        return instance;
    }

    private static class Styles
    {
        static Styles()
        {
            PopupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            PopupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        public static readonly GUIStyle PopupStyle;
    }
}
