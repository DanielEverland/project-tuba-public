using System.Linq;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class QueryPerception : ActionTask
    {
        public BBParameter<PerceptionTarget> output;
        [RequiredField]
        public BBParameter<Perception> perception;

        public List<TagType> inclusiveTags;
        public List<TagType> bannedTags;

        protected override void OnStart()
        {
            List<PerceptionTarget> results = perception.value.QueryTags(inclusiveTags, bannedTags);

            if(results.Count == 0)
            {
                EndAction(false);
                return;
            }

            output.value = results[0];
            EndAction(true);
        }
    }
}