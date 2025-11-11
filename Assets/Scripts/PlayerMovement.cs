using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;
    
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Buscar el Animator en los hijos (KyleRobot)
        animator = GetComponentInChildren<Animator>();
        
        // Debug para verificar que los componentes están presentes
        if (rb == null) Debug.LogError("Rigidbody2D no encontrado!");
        if (animator == null) Debug.LogError("Animator no encontrado!");
        else Debug.Log("Animator encontrado en: " + animator.gameObject.name);
    }
    
    void Update()
    {
        // Leer el input horizontal (A/D o flechas izq/der)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        // Ground Check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        
        // Actualizar parámetros del Animator
        UpdateAnimations();
    }
    
    void FixedUpdate()
    {
        if (rb == null) return;
        
        // Calcular la velocidad objetivo
        float targetSpeed = horizontalInput * moveSpeed;
        
        // Suavizar el movimiento con aceleración
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        
        // Aplicar el movimiento
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    
    void UpdateAnimations()
    {
        if (animator == null || rb == null) return;
        
        // Speed - velocidad horizontal actual (valor absoluto)
        float speed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", speed);
        
        // Grounded - si está en el suelo
        animator.SetBool("Grounded", isGrounded);
        
        // Jump - si está saltando (velocidad vertical positiva Y en el aire)
        bool isJumping = !isGrounded && rb.linearVelocity.y > 0.1f;
        animator.SetBool("Jump", isJumping);
        
        // FreeFall - si está cayendo (velocidad vertical negativa Y en el aire)
        bool isFreeFalling = !isGrounded && rb.linearVelocity.y < -0.1f;
        animator.SetBool("FreeFall", isFreeFalling);
        
        // MotionSpeed - velocidad vertical (de -1 a 1)
        float motionSpeed = Mathf.Clamp(rb.linearVelocity.y / 10f, -1f, 1f);
        animator.SetFloat("MotionSpeed", motionSpeed);
        
        // Debug
        Debug.Log($"Grounded: {isGrounded}, Jump: {isJumping}, FreeFall: {isFreeFalling}, Speed: {speed:F2}");
    }
}
