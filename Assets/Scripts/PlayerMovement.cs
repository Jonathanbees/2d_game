using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Leer el input horizontal (A/D o flechas izq/der)
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }
    
    void FixedUpdate()
    {
        // Calcular la velocidad objetivo
        float targetSpeed = horizontalInput * moveSpeed;
        
        // Suavizar el movimiento con aceleración
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        
        // Aplicar el movimiento
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
