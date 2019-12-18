using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Handles the selection of tile options
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class RoomBuilderTileOptionsSelector : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown dropDown = default;
        [SerializeField]
        private TileDataVariable selectedTileData = default;

        private void Start()
        {
            if (selectedTileData.Value != null)
            {
                dropDown.value = TileData.AllTiles.IndexOf(selectedTileData.Value);
            }
        }
        public void ValueChanged(int index)
        {
            selectedTileData.Value = TileData.AllTiles[index];
        }
        private void OnValidate()
        {
            if (dropDown == null)
                dropDown = GetComponent<TMP_Dropdown>();
        }
    }
}