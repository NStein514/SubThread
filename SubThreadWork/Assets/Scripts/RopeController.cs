using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeController : MonoBehaviour
{
    public Transform ropeTip; // Empty GameObject at the end of the rope
    public LayerMask enemyLayer; 
    public float grabRadius; 

    private LineRenderer line;
    private Transform targetEnemy;
    private Rigidbody2D enemyRb;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2; 
    }

    void Update()
    {
        if (targetEnemy != null)
        {
            DrawRope(transform.position, targetEnemy.position);
        }
        else
        {
            line.enabled = false; 
        }
    }

    public void TryGrabEnemy()
    {
        Collider2D hit = Physics2D.OverlapCircle(ropeTip.position, grabRadius, enemyLayer);
        if (hit != null)
        {
            targetEnemy = hit.transform;
            enemyRb = hit.attachedRigidbody;
            line.enabled = true;
        }
    }

    public void SlamEnemy()
    {
        if (targetEnemy != null && enemyRb != null)
        {
            enemyRb.AddForce(Vector2.down * 30f, ForceMode2D.Impulse);
        }
    }

    private void DrawRope(Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
