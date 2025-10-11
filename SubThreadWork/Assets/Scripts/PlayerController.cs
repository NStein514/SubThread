using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f; 
    public float jumpForce = 12f;

    [Header("References")]
    public RopeController rope; 

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Trigger the Rope Grab
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rope.TryGrabEnemy();
        }

        // Slam if Holding Down 
        if (Input.GetKeyDown(KeyCode.S))
        {
            rope.SlamEnemy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true; 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false; 
    }
}
