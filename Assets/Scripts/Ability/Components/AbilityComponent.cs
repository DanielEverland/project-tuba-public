using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Ability
{
    public abstract class AbilityComponent
    {
        public AbilityComponent(Ability ability)
        {
            Ability = ability;
        }
        
        protected Ability Ability { get; private set; }

        public abstract AbilityStateDelta PollInput(AbilityInput input);
    }
}
