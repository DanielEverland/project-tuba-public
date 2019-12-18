using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class RemoveShieldLayer : ActionTask
    {
        [RequiredField]
        public BBParameter<Shield> targetShield;

        protected override void OnStart()
        {
            targetShield.value.RemoveLayer();
            EndAction();
        }
    }
}