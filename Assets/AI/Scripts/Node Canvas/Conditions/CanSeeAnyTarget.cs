using System.Linq;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions{

	[Category("Everland Games")]
	public class CanSeeAnyTarget : ConditionTask
    {
        [RequiredField]
        public BBParameter<Perception> perception;

        public List<TagType> inclusiveTags;
        public List<TagType> bannedTags;
        
        protected override bool OnCheck()
        {
            return perception.value.QueryTags(inclusiveTags, bannedTags).Count > 0;
        }
	}
}