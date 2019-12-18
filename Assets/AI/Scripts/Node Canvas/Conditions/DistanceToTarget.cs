using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;

namespace NodeCanvas.Tasks.Conditions{

	[Category("Everland Games")]
	public class DistanceToTarget : ConditionTask
    {
        public BBParameter<PerceptionTarget> perceptionTarget;
        public BBParameter<float> distance;
        public CompareMethod comparison = CompareMethod.EqualTo;
        
        protected override string info => $"Distance To Target {OperationTools.GetCompareString(comparison)} {distance}";

        protected override bool OnCheck()
        {
            if (perceptionTarget.value == null)
            {
                Logger.LogError($"<b>Distance To Target</b>: No {nameof(PerceptionTarget)} assigned!", "OnCheck", this);
                return false;
            }                

            if (perceptionTarget.value.Target == null)
                return false;

            float distanceToTarget = Vector2.Distance(agent.transform.position, perceptionTarget.value.Target.transform.position);
            return OperationTools.Compare(distanceToTarget, distance.value, comparison, 0f);
        }
    }
}