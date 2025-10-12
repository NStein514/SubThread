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
    private DistanceJoint2D swingJoint;

    public void Start()
    {
        player = GetComponent<PlayerController>();
        swingJoint = gameObject.AddComponent<DistanceJoint2D>();
        swingJoint.enabled = false;
        swingJoint.autoConfigureDistance = false;
        swingJoint.maxDistanceOnly = true;
    }

    public void ForwardSlashAttack()
    {
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

        swingJoint.enabled = true;
        swingJoint.connectedBody = enemy.GetRigidbody();
        swingJoint.distance = Vector2.Distance(transform.position, enemy.transform.position);

        if (chainRenderer)
        {
            chainRenderer.enabled = true;
            chainRenderer.positionCount = 2;
        }
    }

    public void SlamEnemy()
    {
        if (!isAttached || attachedEnemy == null) return;

        attachedEnemy.SlamIntoGround();
        Detach();
    }

    public void ReleaseSwing()
    {
        if (!isAttached) return;

        // Maintain current velocity
        Detach();
    }

    void Detach()
    {
        if (attachedEnemy)
            attachedEnemy.OnDetachedFromChain();

        attachedEnemy = null;
        isAttached = false;

        swingJoint.enabled = false;
        swingJoint.connectedBody = null;

        if (chainRenderer)
            chainRenderer.enabled = false;
    }

    void Update()
    {
        if (isAttached && chainRenderer && attachedEnemy)
        {
            chainRenderer.SetPosition(0, transform.position);
            chainRenderer.SetPosition(1, attachedEnemy.transform.position);

            // Swing control
            float move = Input.GetAxisRaw("Horizontal");
            player.GetRigidbody().AddForce(new Vector2(move * 8f, 0f));

            // Jump to release
            if (Input.GetButtonDown("Jump"))
                ReleaseSwing();
        }
    }
}
