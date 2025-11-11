using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 10f;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalInput;

    // Ground Check variables
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public float groundCheckRadius = 0.2f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Coyote Time
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    // Jump Buffering
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    // Double Jump
    [Header("Double Jump")]
    public int extraJumps = 1;
    private int extraJumpsValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsValue = extraJumps; // Set initial jumps
    }
    
    void Update()
    {
        // Leer input de movimiento
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        // --- Ground Check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Coyote Time & Double Jump Reset ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumpsValue = extraJumps; // Reset double jumps
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffering ---
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // --- COMBINED Jump Input Check ---
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f) // Priority 1: Ground Jump (uses coyote time)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f; // Consume coyote time
                jumpBufferCounter = 0f; // Consume buffer
            }
            else if (extraJumpsValue > 0) // Priority 2: Air Jump
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumpsValue--; // Consume an air jump
                jumpBufferCounter = 0f; // Consume buffer
            }
        }
    }
    
    void FixedUpdate()
    {
        // Aplicar movimiento horizontal
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        
        // --- Better Falling Logic ---
        if (rb.linearVelocity.y < 0)
        {
            // We are falling - apply the fallMultiplier
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // We are rising, but not holding Jump - apply the lowJumpMultiplier
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
