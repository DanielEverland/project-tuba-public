using UnityEngine;
using XInputDotNetPure;

public class ControllerRumble : MonoBehaviour
{
    private PlayerIndex playerIndex;
    private GamePadState state;
    private GamePadState prevState;

    private float vibrationStart;
    private float vibrationDuration;

    private bool ShouldVibrate => Time.unscaledTime - vibrationStart < vibrationDuration;

    public void Vibrate(float duration)
    {
        vibrationStart = Time.unscaledTime;
        vibrationDuration = duration;
    }
    private void Update()
    {
        PollControllerConnection();
        SetState();
        PollRumble();
    }
    private void SetState()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);
    }
    private void PollRumble()
    {
        float amount = ShouldVibrate ? 1 : 0;
        GamePad.SetVibration(playerIndex, amount, amount);
    }
    private void PollControllerConnection()
    {
        if (!prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                }
            }
        }
    }
    private void OnDestroy()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }
}