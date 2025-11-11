using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 10f;

    // Ground Check variables
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public float groundCheckRadius = 0.2f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;


    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
   void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Coyote Time Logic ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        //imprimir un log
        Debug.Log("input jump: " + Input.GetButtonDown("Jump"));

        // --- Modify Jump Input Check ---
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f) // Check counter, not isGrounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f; // Use up the coyote time jump
        }
    }
    
    // FixedUpdate remains the same as Step 3
    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }



    // Helper function to visualize the ground check radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
