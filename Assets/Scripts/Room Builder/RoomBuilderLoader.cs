using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Loads a room into the builder
    /// </summary>
    public class RoomBuilderLoader : MonoBehaviour
    {
        [SerializeField]
        private RoomDataVariable currentRoom = default;
        [SerializeField]
        private GameEvent rebuildRoomsEvent = default;

        public void Load()
        {
#if UNITY_EDITOR
            string path = EditorUtility.OpenFilePanel("Load Room", "Assets", "asset");
            int assetIndex = path.IndexOf("Assets");
            path = path.Substring(assetIndex);

            if (path == string.Empty)
                return;

            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

            if (!(obj is RoomData))
            {
                Debug.LogError($"Object must be of type {nameof(RoomData)}");
                return;
            }

            currentRoom.Value = Instantiate<RoomData>(obj as RoomData);
            rebuildRoomsEvent.Raise();
#else
        throw new System.NotImplementedException();
#endif
        }
    }
}