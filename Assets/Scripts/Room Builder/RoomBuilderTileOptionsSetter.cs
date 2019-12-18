using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Sets the dropdown options to include all tiles
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class RoomBuilderTileOptionsSetter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown dropDown;

        private void OnEnable()
        {
            dropDown.options.AddRange(TileData.AllTiles.Select(x => new TMP_Dropdown.OptionData(x.name)).ToList());
        }

        private void OnValidate()
        {
            if (dropDown == null)
                dropDown = GetComponent<TMP_Dropdown>();
        }
    }
}