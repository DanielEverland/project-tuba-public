using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class MoveTowardsInteractable : ActionTask
    {
        [RequiredField]
        public BBParameter<Interactable> target = default;
        [RequiredField]
        public BBParameter<IMoveTowardsPosition> inputHandler = default;

        protected override void OnTick()
        {
            inputHandler.value.MoveTowards(target.value.transform.position);
        }
        protected override void OnStop()
        {
            inputHandler.value.ClearTarget();
        }
    }
}