using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainWeapon : MonoBehaviour
{
    [Header("Chain Settings")]
    public float forwardSlashRange = 2.5f;
    public float forwardSlashForce = 12f;
    public float overheadSwingRange = 3f;
    public LayerMask enemyMask;
    public LineRenderer chainRenderer;

    private Enemy attachedEnemy;
    private PlayerController player;
    private bool isAttached;

    public void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void ForwardSlashAttack()
    {
        // Raycast forward
        Vector2 direction = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, forwardSlashRange, enemyMask);

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy)
            {
                Vector2 pushDir = direction * forwardSlashForce;
                enemy.TakeImpact(pushDir);
            }
        }
    }

    public void OverheadSwingAttack()
    {
        // Circle cast to detect enemies above
        Collider2D hit = Physics2D.OverlapCircle(transform.position + Vector3.up * 1.5f, overheadSwingRange, enemyMask);

        if (hit)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                AttachToEnemy(enemy);
            }
        }
    }

    public void AttachToEnemy(Enemy enemy)
    {
        attachedEnemy = enemy;
        isAttached = true;
        enemy.OnAttachedToChain(this);

        if (chainRenderer)
        {
            chainRenderer.enabled = true;
            chainRenderer.SetPosition(0, transform.position);
            chainRenderer.SetPosition(1, enemy.transform.position);
        }
    }

    public void SlamEnemy()
    {
        if (!isAttached || attachedEnemy == null) return;

        // Player must be grounded to perform instant slam
        if (player.IsGrounded())
        {
            attachedEnemy.SlamIntoGround();
            Detach();
        }
        else
        {
            // Midair slam: apply velocity to player
            player.GetRigidbody().velocity += Vector2.down * 10f;
            attachedEnemy.SlamIntoGround();
            Detach();
        }
    }

    public void Detach()
    {
        if (attachedEnemy)
            attachedEnemy.OnDetachedFromChain();

        attachedEnemy = null;
        isAttached = false;

        if (chainRenderer)
            chainRenderer.enabled = false;
    }

    public void Update()
    {
        if (isAttached && chainRenderer && attachedEnemy)
        {
            chainRenderer.SetPosition(0, transform.position);
            chainRenderer.SetPosition(1, attachedEnemy.transform.position);
        }
    }
}
