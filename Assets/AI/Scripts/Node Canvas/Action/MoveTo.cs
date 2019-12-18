using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class MoveTo : ActionTask
    {
        [RequiredField]
        public BBParameter<Vector2> target = default;
        [RequiredField]
        public BBParameter<IMoveTowardsPosition> inputHandler = default;
        public BBParameter<float> minDistance = 0.1f;

        protected override void OnTick()
        {
            inputHandler.value.MoveTowards(target.value);
            
            if (Vector2.Distance(ownerAgent.transform.position, target.value) < minDistance.value)
                EndAction();

            if (inputHandler.value.IsStuck())
                EndAction(false);
        }
        protected override void OnStop()
        {
            inputHandler.value.ClearTarget();
        }
    }
}