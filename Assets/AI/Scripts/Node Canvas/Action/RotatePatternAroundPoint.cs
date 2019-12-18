using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class RotatePatternAroundPoint : ActionTask
    {
        [RequiredField]
        public BBParameter<Pattern> pattern;
        public BBParameter<float> startAngle;
        public BBParameter<float> rotationSpeed;
        public BBParameter<float> radius;
        public BBParameter<Vector2> point;
        public BBParameter<float> duration;
        
        private PatternObject patternInstance = default;
        private float startTime = default;
        private float currentAngle = default;

        private bool IsFinished => Time.time - startTime >= duration.value;

        protected override void OnStart()
        {
            startTime = Time.time;
            currentAngle = startAngle.value;

            patternInstance = CreatePattern();
		}
        
		protected override void OnTick()
        {
            if (IsFinished)
                EndAction();
        }
        
		protected override void OnStop()
        {
            GameObject.Destroy(patternInstance.gameObject);
        }

        private PatternObject CreatePattern()
        {
            PatternObject patternInstance = pattern.value.Spawn();

            AddRotationComponent(patternInstance.gameObject);

            return patternInstance;
        }
        private void AddRotationComponent(GameObject gameObject)
        {
            RotateAroundPoint rotateComponent = gameObject.AddComponent<RotateAroundPoint>();
            rotateComponent.CurrentAngle = startAngle.value;
            rotateComponent.RotationSpeed = rotationSpeed.value;
            rotateComponent.Radius = radius.value;
            rotateComponent.Point = point.value;
        }
    }
}