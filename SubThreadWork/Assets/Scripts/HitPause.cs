using UnityEngine;
using System.Collections;

public class HitPause : MonoBehaviour
{
    private static bool isPaused;

    public static void BriefPause(float duration)
    {
        if (!isPaused)
            instance.StartCoroutine(PauseRoutine(duration));
    }

    private static HitPause instance;

    void Awake()
    {
        instance = this;
    }

    private static IEnumerator PauseRoutine(float duration)
    {
        isPaused = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalTimeScale;
        isPaused = false;
    }
}
