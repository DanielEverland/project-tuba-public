using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PerceptionTargetObjectDrawer : ObjectDrawer<PerceptionTarget>
{
    public override PerceptionTarget OnGUI(GUIContent content, PerceptionTarget instance)
    {
        if(instance != null)
        {
            instance.Target = (Interactable)EditorGUILayout.ObjectField(instance, typeof(Interactable), false);
        }
        else
        {
            using (new EditorGUI.DisabledGroupScope(false))
            {
                EditorGUILayout.ObjectField(instance, typeof(Interactable), false);
            }            
        }
        
        return instance;
    }
}
