using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions{

	[Category("Everland Games")]
	public class RaycastToTarget : ConditionTask
    {
        public BBParameter<PerceptionTarget> perceptionTarget;
        public BBParameter<LayerMask> layerMask;

        protected override bool OnCheck()
        {
            if (perceptionTarget.value.Target == null)
                return false;

            Vector2 delta = perceptionTarget.value.Target.transform.position - agent.transform.position;

            return Physics2D.Raycast(agent.transform.position, delta.normalized, delta.magnitude, layerMask.value);
        }
    }
}