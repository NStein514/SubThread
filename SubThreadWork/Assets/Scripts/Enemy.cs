using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDead;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeImpact(Vector2 force)
    {
        if (isDead) return;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void OnAttachedToChain(ChainWeapon chain)
    {
        rb.isKinematic = true;
    }

    public void OnDetachedFromChain()
    {
        rb.isKinematic = false;
    }

    public void SlamIntoGround()
    {
        if (isDead) return;

        isDead = true;
        rb.isKinematic = false;
        rb.AddForce(Vector2.down * 20f, ForceMode2D.Impulse);
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall") && !isDead)
        {
            // Enemy dies when slammed into a wall
            SlamIntoGround();
        }
    }
}
