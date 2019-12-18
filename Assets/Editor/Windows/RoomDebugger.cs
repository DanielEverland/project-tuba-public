using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

/// <summary>
/// Debugs all available rooms
/// </summary>
public class RoomDebugger : EditorWindow
{
    [SerializeField]
    private GameObject previousRoomParent;
    [SerializeField]
    private Theme theme;

    [MenuItem("Window/Room Debugger")]
    private static void GetWindow()
    {
        RoomDebugger window = EditorWindow.GetWindow<RoomDebugger>();
        window.Show();
    }
    private void OnGUI()
    {
        theme = (Theme)EditorGUILayout.ObjectField(new GUIContent("Theme"), theme, typeof(Theme), false);

        EditorGUILayout.Space();

        throw new System.NotImplementedException();

        //foreach (Type type in GetRooms())
        //{
        //    if(GUILayout.Button(type.Name))
        //    {
        //        if (_previousRoomParent != null)
        //            DestroyImmediate(_previousRoomParent);

        //        LevelEntityGenerator generator = (LevelEntityGenerator)System.Activator.CreateInstance(type);
        //        LevelBuildData data = new LevelBuildData();
        //        generator.Generate(_theme).AddToLevel(data);

        //        Level level = LevelBuilder.Build(data);

        //        _previousRoomParent = level.Root;
        //    }
        //}
    }
    //private List<Type> GetRooms()
    //{
    //    return new List<Type>(typeof(LevelEntityGenerator).Assembly.GetTypes()
    //        .Where(x => typeof(LevelEntityGenerator).IsAssignableFrom(x))
    //        .Where(x => x.IsAbstract == false));
    //}
}
