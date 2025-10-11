using UnityEngine;

public class ImpactEffects : MonoBehaviour
{
    public static ImpactEffects Instance;

    public GameObject slamParticlePrefab;

    void Awake()
    {
        Instance = this;
    }

    public void PlaySlamEffect(Vector3 position)
    {
        if (slamParticlePrefab != null)
            Instantiate(slamParticlePrefab, position, Quaternion.identity);

        // Slow motion flash
        Time.timeScale = 0.4f;
        Invoke(nameof(ResetTime), 0.1f);
    }

    private void ResetTime()
    {
        Time.timeScale = 1f;
    }
}