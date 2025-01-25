using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private new SpriteRenderer renderer;

    [SerializeField]
    private PlayerInput playerInput;

    protected float moveSpeed = 5;

    protected float jumpForce = 5;
    protected int jumpFrequency = 1;

    protected float dashSpeed = 10;
    protected float dashCooldown = 2f;
    
    protected float gravity = 9.81f;

    protected Rigidbody2D rb;


    [SerializeField]
    protected Transform groundCheck;

    [SerializeField]
    protected float groundCheckDistance;

    [SerializeField]
    protected LayerMask groundLayer;

    private PlayerType _type = PlayerType.Unassigned;

    private PlayerComponentScriptableObject _playerComponent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.instance.RegisterPlayer(this);
    }
    
    public void SetPlayerComponent(PlayerComponentScriptableObject playerComponent)
    {
        _playerComponent = playerComponent;
        
        moveSpeed = _playerComponent.MovementSpeed;
        jumpForce = _playerComponent.JumpForce;
        jumpFrequency = _playerComponent.JumpFrequency;
        dashSpeed = _playerComponent.DashSpeed;
        dashCooldown = _playerComponent.DashCooldown;
        gravity = _playerComponent.Gravity;
        
        if (_type != PlayerType.Unassigned)
        {
            Debug.LogError($"Player is already assigned to {_type}");
            return;
        }

        renderer.sprite = playerComponent.PlayerSprite;
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
