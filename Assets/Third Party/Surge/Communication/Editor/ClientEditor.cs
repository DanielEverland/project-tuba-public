﻿/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Custom inspector for the Client.
/// 
/// NOTE: Communication does not support Unity's new networking system introduced in Unity 2019 - a new version will be created soon.
///
/// </summary>

#if !UNITY_2019

using UnityEditor;

namespace Pixelplacement
{
	[CustomEditor(typeof(Client))]
	public class ClientEditor : Editor
	{
		#region Private Variables
		Client _target;
		#endregion

		#region Init
		void OnEnable()
		{
			_target = target as Client;
		}
		#endregion

		#region GUI:
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("broadcastingPort"));
			EditorGUILayout.LabelField("Connection Port", (_target.broadcastingPort + 1).ToString());
			EditorGUILayout.PropertyField(serializedObject.FindProperty("initialBandwidth"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("primaryQualityOfService"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("secondaryQualityOfService"));
			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}

#endif