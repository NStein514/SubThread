using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    [Header("References")]
    public ChainWeapon chainWeapon;

    private Rigidbody2D rb;
    private bool isGrounded;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        HandleMovement();
        HandleInput();
    }

    public void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        // Check if Grounded 
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Ground")))
            isGrounded = true;
        else
            isGrounded = false;

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
            chainWeapon.ForwardSlashAttack();

        if (Input.GetKeyDown(KeyCode.K))
            chainWeapon.OverheadSwingAttack();

        if (Input.GetKeyDown(KeyCode.S))
            chainWeapon.SlamEnemy();
    }

    public bool IsGrounded() => isGrounded;

    public Rigidbody2D GetRigidbody() => rb; 
}
