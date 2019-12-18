using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("Everland Games")]
    public class SetRangedTargetPosition : ActionTask
    {
        public BBParameter<Vector2> output;
        public BBParameter<PerceptionTarget> perceptionTarget;
        public BBParameter<float> distance = null;
        [SliderField(0, 360)]
        public BBParameter<float> circumferenceInterval = 360;

        protected override string info => $"Set Ranged Position\n{distance} --> {circumferenceInterval}º";

        protected override void OnStart()
        {
            if (perceptionTarget.value == null)
                LogError($"Unassigned {nameof(PerceptionTarget)} value", "Execution");

            if(perceptionTarget.value.Target == null)
            {
                EndAction(false);
                return;
            }

            Vector3 delta = agent.transform.position - perceptionTarget.value.Target.transform.position;
            float startAngle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            float minAngle = startAngle - circumferenceInterval.value / 2;
            float maxAngle = startAngle + circumferenceInterval.value / 2;

            float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3()
            {
                x = Mathf.Cos(angle),
                y = Mathf.Sin(angle),
            };

            Vector2 farthestPosition = Utility.GetFarthestWalkablePositionInDirection(perceptionTarget.value.Target.transform.position, direction.normalized, distance.value);
            Vector2 nearestHexCenter = Utility.RoundToNearestHexagonalPosition(farthestPosition);

            output.value = nearestHexCenter;

            EndAction();
        }
    }
}