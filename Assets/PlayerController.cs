using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

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


    private Vector2 _inputValues;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.instance.RegisterPlayer(this);
    }
    

    private void FixedUpdate() {
        PlayerMovement();
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

        _inputValues = inputValues;
    }

    private void PlayerMovement() {
        rb.linearVelocity = new Vector2(
            _inputValues.x * moveSpeed,
            rb.linearVelocityY
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

    private int currentJumpCount = 0;
    
    private bool CanJump() {
        bool groundDetected = IsGroundDetected();
        if (groundDetected) {
            currentJumpCount = 0;
            return true;
        }

        if (currentJumpCount < _playerComponent.JumpFrequency) {
            return true;
        }

        return false;
    }
    void OnJump(InputValue value) 
    {
        if (value.isPressed && CanJump())
        {
            Jump();
        }
    } 

    private void Jump() {
        rb.linearVelocity = new Vector2(
            rb.linearVelocityX,
            jumpForce
        );
        
        currentJumpCount++;
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
