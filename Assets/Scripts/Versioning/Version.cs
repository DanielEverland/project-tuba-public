using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specifies the version of the build
/// </summary>
public class Version : ScriptableObject
{
    public static Version Default
    {
        get
        {
            Version version = Resources.Load<Version>("Versioning/Version");
            
            if(version == null)
            {
#if UNITY_EDITOR
                version = ScriptableObject.CreateInstance<Version>();
                version.name = "Version";

                UnityEditor.AssetDatabase.CreateAsset(version, "Assets/Resources/Versioning/Version.asset");

                Debug.LogWarning("Couldn't find version! It must be in Assets/Resources/Versioning/Version.asset. Creating new!", version);
#endif
            }

            return version;
        }
    }

    [SerializeField]
    private string qualifier = "Pre-Alpha";
    [SerializeField]
    private byte major = 0;
    [SerializeField]
    private byte minor = 0;
    [SerializeField]
    private byte maintenance = 0;
    [SerializeField]
    private ushort build = 0;
    
    public void IncrementBuild()
    {
#if UNITY_EDITOR
        build++;

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    public override string ToString()
    {
        return $"Version {major}.{minor}.{maintenance} ({build}) ({qualifier})";
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.PostProcessBuild]
    private static void PostBuild(UnityEditor.BuildTarget target, string pathToBuiltProject)
    {
        Default.IncrementBuild();
    }
#endif
}
