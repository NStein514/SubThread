using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrabbed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnGrabbed()
    {
        isGrabbed = true;
        rb.gravityScale = 0.5f; // Lighter while held to allow for pulling
    }

    public void OnSlammed()
    {
        isGrabbed = false; 
        rb.gravityScale = 2f;
        // TODO: Add in particle effects and take damage 
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Slam impact effect
        Debug.Log($"{gameObject.name} slammed into ground!");

        ImpactEffects.Instance?.PlaySlamEffect(transform.position);
        GetComponent<EnemyHealth>()?.TakeDamage(1);

        // Camera shake
        CameraShake shake = Camera.main.GetComponent<CameraShake>();
        if (shake != null)
            StartCoroutine(shake.Shake(0.15f, 0.3f));
    }
}