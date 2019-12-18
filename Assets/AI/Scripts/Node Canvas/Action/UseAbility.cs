using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Services;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Everland Games")]
	public class UseAbility : ActionTask
    {
        [RequiredField]
        public BBParameter<IAttacker> attacker;
        public BBParameter<PerceptionTarget> target;

		protected override void OnStart()
        {
            if (target.value == null)
            {
                Logger.LogError($"<b>{nameof(UseAbility)}</b>: Missing {nameof(PerceptionTarget)}", "Execution", this);
                EndAction(false);
                return;
            }

            if (target.value.Target == null)
            {
                EndAction(false);
                return;
            }

            attacker.value.OnStart(target.value.Target);
		}
        
		protected override void OnTick()
        {
            Status status = attacker.value.OnUpdate();

            if(status == Status.Success || status == Status.Failure)
                EndAction(status == Status.Success ? true : false);
		}
        
		protected override void OnStop()
        {
            attacker.value.OnStop();
		}
	}
}