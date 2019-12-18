using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions{

	[Category("Everland Games")]
	public class CheckEntityHealth : ConditionTask
    {
        public BBParameter<Entity> entity;
        public BBParameter<FloatReference> health;
        public CompareMethod comparison = CompareMethod.EqualTo;

        protected override string info
        {
            get { return $"Health{OperationTools.GetCompareString(comparison)}{health}"; }
        }

        protected override bool OnCheck()
        {
            return OperationTools.Compare(entity.value.Health.CurrentHealth, health.value.Value, comparison, 0f);
        }
    }
}