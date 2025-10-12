using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Physics")]
    public float smashKillVelocity = 8f;
    public float deathCleanupDelay = 1.5f;

    [Header("VFX")]
    public GameObject smashEffectPrefab;
    public AudioClip smashSound;
    public GameObject corpsePrefab; // pixel ragdoll prefab (simple sprite)

    private Rigidbody2D rb;
    private bool isDead;
    private Vector2 lastVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isDead)
            lastVelocity = rb.velocity;
    }

    public void TakeImpact(Vector2 force)
    {
        if (isDead) return;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void OnAttachedToChain(ChainWeapon chain)
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public void OnDetachedFromChain()
    {
        rb.isKinematic = false;
    }

    public void SlamIntoGround()
    {
        if (isDead) return;
        KillEnemy("Slam");
        SpawnEffect(smashEffectPrefab);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            float impactSpeed = lastVelocity.magnitude;
            if (impactSpeed > smashKillVelocity)
            {
                KillEnemy("WallSmash");
                SpawnEffect(smashEffectPrefab);
                CameraShake.Shake(0.15f, 0.12f);
                HitPause.BriefPause(0.04f);
            }
        }
    }

    void KillEnemy(string reason)
    {
        if (isDead) return;
        isDead = true;

        // Spawn corpse with pixel spin physics
        SpawnCorpse();

        if (smashSound)
            AudioSource.PlayClipAtPoint(smashSound, transform.position, 1f);

        // Disable the original enemy visuals immediately
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Destroy(gameObject, deathCleanupDelay);
    }

    void SpawnCorpse()
    {
        if (!corpsePrefab) return;

        GameObject corpse = Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        Rigidbody2D corpseRb = corpse.GetComponent<Rigidbody2D>();

        if (corpseRb)
        {
            // Add a little explosion effect
            float randomX = Random.Range(-2f, 2f);
            float randomY = Random.Range(2f, 4f);
            corpseRb.velocity = new Vector2(randomX, randomY);

            // Add spin
            corpseRb.angularVelocity = Random.Range(-360f, 360f);
        }

        // Add simple fade coroutine
        SpriteRenderer corpseSR = corpse.GetComponent<SpriteRenderer>();
        if (corpseSR)
            StartCoroutine(FadeOutCorpse(corpseSR));
    }

    IEnumerator FadeOutCorpse(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(0.3f);
        float t = 0f;
        Color start = sr.color;
        while (t < 1f)
        {
            t += Time.deltaTime;
            sr.color = new Color(start.r, start.g, start.b, 1f - t);
            yield return null;
        }
        Destroy(sr.gameObject);
    }

    void SpawnEffect(GameObject prefab)
    {
        if (prefab)
            Instantiate(prefab, transform.position, Quaternion.identity);
    }

    public Rigidbody2D GetRigidbody() => rb;
}
