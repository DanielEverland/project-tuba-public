using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(SelfDestructionExecutor))]
public class SelfDestructionExecutorEditor : Editor
{
    private bool UseRadius => dealDamageProperty.boolValue || applyForceProperty.boolValue;
    private bool UseLayerMask => dealDamageProperty.boolValue || applyForceProperty.boolValue;

    private SerializedProperty ownerProperty;
    private SerializedProperty layerMaskProperty;
    private SerializedProperty radiusProperty;
    private SerializedProperty dealDamageProperty;
    private SerializedProperty actionProperty;
    private SerializedProperty destroyProperty;
    private SerializedProperty applyForceProperty;
    private SerializedProperty forceProperty;
    private SerializedProperty onSelfDestructProperty;

    private AnimBool radiusAnim;
    private AnimBool dealDamageAnim;
    private AnimBool applyForceAnim;
    private AnimBool layerMaskAnim;

    private void OnEnable()
    {
        CreateSerializedProperties();
        CreateAnimationBoolFields();
    }
    private void CreateSerializedProperties()
    {
        ownerProperty = serializedObject.FindProperty("owner");
        layerMaskProperty = serializedObject.FindProperty("layerMask");
        radiusProperty = serializedObject.FindProperty("radius");
        dealDamageProperty = serializedObject.FindProperty("dealDamage");
        actionProperty = serializedObject.FindProperty("action");
        destroyProperty = serializedObject.FindProperty("destroy");
        applyForceProperty = serializedObject.FindProperty("applyForce");
        forceProperty = serializedObject.FindProperty("force");
        onSelfDestructProperty = serializedObject.FindProperty("onSelfDestruct");
    }
    private void CreateAnimationBoolFields()
    {
        radiusAnim = new AnimBool(UseRadius);
        radiusAnim.valueChanged.AddListener(Repaint);

        dealDamageAnim = new AnimBool(dealDamageProperty.boolValue);
        dealDamageAnim.valueChanged.AddListener(Repaint);

        applyForceAnim = new AnimBool(applyForceProperty.boolValue);
        applyForceAnim.valueChanged.AddListener(Repaint);

        layerMaskAnim = new AnimBool(UseLayerMask);
        layerMaskAnim.valueChanged.AddListener(Repaint);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawFields();

        serializedObject.ApplyModifiedProperties();
    }
    private void DrawFields()
    {
        EditorGUILayout.PropertyField(ownerProperty);
        EditorGUILayout.PropertyField(destroyProperty);

        DrawDamageProperty();
        DrawForceProperty();
        DrawLayerMaskProperty();
        DrawRadiusProperty();

        EditorGUILayout.PropertyField(onSelfDestructProperty);
    }
    private void DrawDamageProperty()
    {
        DrawPropertyAnimationDependency(dealDamageProperty, dealDamageAnim);
        DrawIndentedAnimatedField(actionProperty, dealDamageAnim);
    }
    private void DrawForceProperty()
    {
        DrawPropertyAnimationDependency(applyForceProperty, applyForceAnim);
        DrawIndentedAnimatedField(forceProperty, applyForceAnim);
    }
    private void DrawLayerMaskProperty()
    {
        layerMaskAnim.target = UseLayerMask;
        DrawAnimatedField(layerMaskProperty, layerMaskAnim);
    }
    private void DrawRadiusProperty()
    {
        radiusAnim.target = UseRadius;
        DrawAnimatedField(radiusProperty, radiusAnim);
    }
    private void DrawPropertyAnimationDependency(SerializedProperty dependency, AnimBool animation)
    {
        EditorGUILayout.PropertyField(dependency);
        animation.target = dependency.boolValue;
    }
    private void DrawIndentedAnimatedField(SerializedProperty field, AnimBool animation)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            DrawAnimatedField(field, animation);
        }
    }
    private void DrawAnimatedField(SerializedProperty field, AnimBool animation)
    {
        using (var scope = new EditorGUILayout.FadeGroupScope(animation.faded))
        {
            if (scope.visible)
                EditorGUILayout.PropertyField(field);
        }
    }
}
