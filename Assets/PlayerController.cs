using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 5;

    [SerializeField]
    protected float jumpForce = 5;

    protected Rigidbody2D rb;


    [SerializeField]
    protected Transform groundCheck;

    [SerializeField]
    protected float groundCheckDistance;

    [SerializeField]
    protected LayerMask groundLayer;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // public virtual bool IsGroundDetected()
    // {
    //     var groundLayer = ;
    //     return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    // }

public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    private void OnDrawGizmos() {
        Gizmos.DrawLine(
            groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)
        );
    }
}
