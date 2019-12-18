using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
    [Category("Everland Games")]
    public class IsEntityHealthFull : ConditionTask
    {
        [RequiredField]
        public BBParameter<Entity> entity = default;
        
        protected override bool OnCheck()
        {
            return entity.value.Health.IsFullHealth;
        }
    }
}