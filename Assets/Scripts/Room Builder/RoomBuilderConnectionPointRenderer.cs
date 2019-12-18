using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Draws an icon that represents the connection point of the room
    /// </summary>
    public class RoomBuilderConnectionPointRenderer : MonoBehaviour
    {
        [SerializeField]
        private RoomDataReference currentRoom = default;
        [SerializeField]
        private GameObject indicatorPrefab = default;

        private List<GameObject> indicatorStash = new List<GameObject>();
        
        private void Update()
        {
            if (currentRoom.Value == null)
                return;

            indicatorStash.ForEach(x => x.SetActive(false));

            for (int i = 0; i < currentRoom.Value.ConnectionPoints.Count; i++)
            {
                GameObject obj = GetIndicator(i);

                Vector2 worldPosition = Utility.AxialToWorldPosition(currentRoom.Value.ConnectionPoints[i]);
                Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

                obj.SetActive(true);
                obj.transform.position = screenPos;
            }
        }
        private GameObject GetIndicator(int index)
        {
            while (indicatorStash.Count <= index)
            {
                GameObject obj = GameObject.Instantiate(indicatorPrefab);
                obj.transform.SetParent(transform);

                indicatorStash.Add(obj);
            }

            return indicatorStash[index];
        }
    }
}
