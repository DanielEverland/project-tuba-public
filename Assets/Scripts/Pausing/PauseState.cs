using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseState
{
    public static bool IsPaused
    {
        get
        {
            return Time.frameCount - lastPauseFrame < FrameMargin;
        }
    }

    private const int FrameMargin = 3;

    private static int lastPauseFrame = -FrameMargin;
    private static TimeScaler timeScaler;

    public static void Pause()
    {
        lastPauseFrame = Time.frameCount;

        PollTimeScaler();
    }
    private static void PollTimeScaler()
    {
        if (timeScaler == null)
            CreateTimeScaler();
    }
    private static void CreateTimeScaler()
    {
        GameObject timeScalerGameObject = new GameObject("Time Scaler");
        timeScalerGameObject.hideFlags |= HideFlags.HideInHierarchy;

        timeScaler = timeScalerGameObject.AddComponent<TimeScaler>();
    }

    private class TimeScaler : MonoBehaviour
    {
        public void Update()
        {
            Time.timeScale = IsPaused ? 0 : 1;
        }
        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}
