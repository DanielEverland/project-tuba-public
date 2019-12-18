using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class ChangeState : ActionTask
    {
        [RequiredField]
        public BBParameter<StateExecutor> enabler;
        [RequiredField]
        public BBParameter<StateObject> targetState;

        protected override void OnStart()
        {
            enabler.value.EnableState(targetState.value);

            EndAction();
        }
    }
}