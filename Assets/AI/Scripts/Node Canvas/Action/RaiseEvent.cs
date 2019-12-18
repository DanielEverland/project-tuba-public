using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class RaiseEvent : ActionTask
    {
        [RequiredField]
        public BBParameter<IEventHandler> target = default;

		protected override void OnStart()
        {
            target.value.Raise();

            EndAction();
		}
	}
}