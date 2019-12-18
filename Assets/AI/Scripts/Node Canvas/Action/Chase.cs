using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class Chase : ActionTask
    {
        public BBParameter<PerceptionTarget> target = default;
        [RequiredField]
        public BBParameter<IMoveTowardsInteractable> inputHandler = default;
        
        protected override void OnTick()
        {
            if(target.value.Target == null)
            {
                EndAction(false);
                return;
            }

            inputHandler.value.MoveTowards(target.value);
        }
        protected override void OnStop()
        {
            inputHandler.value.ClearTarget();
        }
    }
}