using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Saves the room that's currently being built
    /// </summary>
    public class RoomBuilderSaver : MonoBehaviour
    {
        [SerializeField]
        private RoomDataVariable currentRoom = default;

        public void Save()
        {
#if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanelInProject("Save Room", "", "asset", "");
            
            AssetDatabase.CreateAsset(currentRoom.Value, path);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
#else
        throw new System.NotImplementedException();
#endif
        }
    }
}