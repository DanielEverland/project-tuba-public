using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Sets the state of a toggle depending on the current room builder tool state
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class RoomBuilderToolUIToggle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Toggle toggleElement = default;
        [SerializeField]
        private RoomBuilderToolState thisState = default;
        [SerializeField]
        private RoomBuilderToolStateVariable currentTool = default;

        private void Start()
        {
            PollState();
        }
        public void PollState()
        {
            toggleElement.isOn = currentTool.Value == thisState;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Selected();
        }
        public void Selected()
        {
            currentTool.Value = thisState;
        }
        private void OnValidate()
        {
            if (toggleElement == null)
                toggleElement = GetComponent<Toggle>();
        }
    }
}