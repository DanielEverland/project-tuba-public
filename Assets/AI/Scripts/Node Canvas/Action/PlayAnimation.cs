using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class PlayAnimation : ActionTask
    {
        [RequiredField]
        public BBParameter<Behaviour> animation;
        public BBParameter<float> duration = 1;

        private float animationStartTime;

        protected override void OnStart()
        {
            Play();
        }
        protected override void OnTick()
        {
            PollIsFinished();
        }

        private void Play()
        {
            animationStartTime = Time.time;
            animation.value.enabled = true;
        }
        private void PollIsFinished()
        {
            if (Time.time - animationStartTime > duration.value)
                Stop();
        }
        private void Stop()
        {
            animation.value.enabled = false;

            EndAction();
        }
    }
}