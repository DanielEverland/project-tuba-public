using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class CreatePerceptionTarget : ActionTask
    {
        public BBParameter<Interactable> from;
        public BBParameter<PerceptionTarget> to;
        [RequiredField]
        public BBParameter<Perception> perception;

        protected override void OnStart()
        {
            if (from.value == null)
            {
                LogError("From value not assigned!", "Execution");
                EndAction(false);
            }

            to.value = new PerceptionTarget(perception.value, from.value);
            EndAction(true);
        }
    }
}