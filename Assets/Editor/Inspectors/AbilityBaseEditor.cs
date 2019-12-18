using UnityEditor;

[CustomEditor(typeof(AbilityPart), true)]
public class AbilityBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawHelpBox(target);
    }
    public static void DrawHelpBox(UnityEngine.Object target)
    {
        string path = AssetDatabase.GetAssetPath(target);

        if (!path.Contains("Resources"))
        {
            EditorGUILayout.HelpBox("Asset is not in Resources folder", MessageType.Warning);
        }
    }
}