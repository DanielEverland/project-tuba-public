using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : CallbackMonobehaviour
{
    public override void OnRaised()
    {
        PauseState.Pause();
    }
}
