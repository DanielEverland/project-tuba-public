using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class SelfDestruct : ActionTask
    {
        [RequiredField]
        public BBParameter<ISelfDestructExecutor> executor;

        protected override void OnStart()
        {
            executor.value.SelfDestruct();

            EndAction();
        }
    }
}