using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Sprite bubble;

    [SerializeField]
    private Sprite urchin;

    [SerializeField]
    private new SpriteRenderer renderer;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    protected float moveSpeed = 5;

    [SerializeField]
    protected float jumpForce = 5;

    [SerializeField]
    protected float dashSpeed = 10;

    protected Rigidbody2D rb;


    [SerializeField]
    protected Transform groundCheck;

    [SerializeField]
    protected float groundCheckDistance;

    [SerializeField]
    protected LayerMask groundLayer;

    private PlayerType _type = PlayerType.Unassigned;
    private void Start()
    {
        GameManager.instance.RegisterPlayer(this);
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetPlayerType(PlayerType value)
    {
        if (_type != PlayerType.Unassigned)
        {
            Debug.LogError($"Player is already assigned to {_type}");
            return;
        }
        if (value == PlayerType.Bubble)
        {
            // Add bubble specific componets;
            renderer.sprite = bubble;
        }
        else
        {
            // Add urchin specific components;
            renderer.sprite = urchin;
        }
    }
    public virtual void OnMove(InputValue value)
    {
        var inputValues = value.Get<Vector2>();
        if (inputValues == null)
        {
            return;
        }
        if (!IsGroundDetected())
        {
            // Cant move while in the air
            inputValues.y = 0;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocityX + inputValues.x * moveSpeed,
            rb.linearVelocityY + inputValues.y * jumpForce
        );
    }

    public virtual void OnSprint(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        var xInput = Input.GetAxisRaw("Horizontal");
        var yInput = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(
            rb.linearVelocityX + xInput * dashSpeed,
            rb.linearVelocityY + yInput * dashSpeed
        );
    }

    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)
        );
    }
}

public enum PlayerType
{
    Unassigned,
    Bubble,
    Urchin
}
