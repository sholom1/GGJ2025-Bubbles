using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    protected PlayerComponentScriptableObject playerComponent;
    
    protected float moveSpeed = 5;
    
    protected float jumpForce = 5;
    protected int jumpFrequency = 1;
    
    protected float dashDistance = 10;
    protected float dashCooldown = 2;
    
    protected float gravity = 9.8f;


    protected Rigidbody2D rb;


    [SerializeField]
    protected Transform groundCheck;

    [SerializeField]
    protected float groundCheckDistance;

    [SerializeField]
    protected LayerMask groundLayer;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = playerComponent.MovementSpeed;
        jumpForce = playerComponent.JumpForce;
        jumpFrequency = playerComponent.JumpFrequency;
        dashDistance = playerComponent.DashDistance;
        dashCooldown = playerComponent.DashCooldown;
        gravity = playerComponent.Gravity;
    }

    
    public virtual void OnMove(InputValue value)
    {
        var inputValues = value.Get<Vector2>();
        if (inputValues == null) {
            return;
        }
        if (!IsGroundDetected()) {
            // Cant move while in the air
            inputValues.y = 0;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocityX + inputValues.x * moveSpeed,
            rb.linearVelocityY + inputValues.y * jumpForce
        );
    }

    public virtual void OnSprint(InputValue value) {
        if(!value.isPressed) {
            return;
        }

        var xInput = Input.GetAxisRaw("Horizontal");
        var yInput = Input.GetAxisRaw("Vertical");
        
        rb.linearVelocity = new Vector2(
            rb.linearVelocityX + xInput * dashDistance,
            rb.linearVelocityY + yInput * dashDistance
        );
    }

    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    private void OnDrawGizmos() {
        Gizmos.DrawLine(
            groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)
        );
    }
}
