using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class ToggleComponent : ActionTask
    {
        [RequiredField]
        public BBParameter<Behaviour> component;
        public bool enabled;

        protected override void OnStart()
        {
            component.value.enabled = enabled;

            EndAction();
        }
    }
}