using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    private Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public static void Shake(float intensity, float duration)
    {
        if (instance != null)
            instance.StartCoroutine(instance.ShakeRoutine(intensity, duration));
    }

    private System.Collections.IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.localPosition = originalPos + Random.insideUnitSphere * intensity;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
