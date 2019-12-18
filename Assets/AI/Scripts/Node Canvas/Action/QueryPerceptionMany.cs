using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	[Category("Everland Games")]
	public class QueryPerceptionMany : ActionTask
    {
        [RequiredField]
        public BBParameter<List<PerceptionTarget>> output;
        [RequiredField]
        public BBParameter<Perception> perception;

        public List<TagType> inclusiveTags;
        public List<TagType> bannedTags;

        protected override void OnStart()
        {
            List<PerceptionTarget> queryResult = perception.value.QueryTags(inclusiveTags, bannedTags);
            
            if(queryResult.Count == 0)
            {
                EndAction(false);
                return;
            }

            output.value = queryResult;
            EndAction(true);
        }
    }
}