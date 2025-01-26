using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private new SpriteRenderer renderer;

    [SerializeField]
    private PlayerInput playerInput;
    
    protected Rigidbody2D rb;


    [SerializeField]
    protected Transform groundCheck;

    [SerializeField]
    protected float groundCheckDistance;

    [SerializeField]
    protected LayerMask groundLayer;

    private PlayerType _type = PlayerType.Unassigned;
    
    public  PlayerAttributeController attributeController;
    private int currentJumpCount = 0;
    private float dashDurationTimer;
    private float lastDashTime;

    private List<Perk> _perks = new List<Perk>();

    private Vector2 _inputValues;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.instance.RegisterPlayer(this);
    }

    public void OnPlayerJoined(PlayerType playerType)
    {
        Debug.LogError("VAR");
    }
    
    private void FixedUpdate() {
        dashDurationTimer -= Time.deltaTime;
        PlayerMovement();
    }

    public PlayerType GetPlayerType() {
        return _type;
    }

    public void SetPlayerType(PlayerType type)
    {
        if (_type != PlayerType.Unassigned)
        {
            Debug.LogError($"Player is already assigned to {_type}");
            return;
        }
        _type = type;
    }

    public void SetPlayerPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetPlayerComponent(PlayerComponentScriptableObject playerComponent)
    {
        if (playerComponent == null)
        {
            return;
        }

        attributeController = new PlayerAttributeController(playerComponent);
        
        rb.gravityScale = playerComponent.GravityScale;

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
        if (IsDashing() || attributeController == null) {
            return;
        }
        rb.linearVelocity = new Vector2(
            _inputValues.x * attributeController.MoveSpeed,
            rb.linearVelocityY
        );
    }

    public virtual void OnSprint(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if(Time.time - lastDashTime < attributeController.DashCooldown) {
            return;
        }

        dashDurationTimer = attributeController.DashDuration;
        lastDashTime = Time.time;
        rb.linearVelocity = new Vector2(
            _inputValues.x * attributeController.DashSpeed,
            rb.linearVelocityY
        );
    }
    
    private bool CanJump() {
        bool groundDetected = IsGroundDetected();
        if (groundDetected) {
            currentJumpCount = 0;
            return true;
        }
        if (attributeController == null) {
            return false;
        }
        if (currentJumpCount < attributeController.JumpFrequency) {
            return true;
        }

        return false;
    }

    void OnJump(InputValue value) 
    {
        if (value.isPressed && CanJump())
        {
            Jump();
            CinematicOpenerController.instance?.SkipScene();
        }
    } 

    private void Jump() {
        rb.linearVelocity = new Vector2(
            rb.linearVelocityX,
            attributeController.JumpForce
        );

        currentJumpCount++;
    }

    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    bool IsDashing() => dashDurationTimer > 0;

    public void AddPerk(Perk perk) {
        _perks.Add(perk);
    }

    public void ApplyPerks() {
        _perks.ForEach(perk => perk.Action(this));
    }

        public virtual void OnAllHeartsCollected()
    {
        if (_type == PlayerType.Bubble)
        {
            // Change bubble appearance or behavior when all hearts are collected
            // This could be overridden in a derived class if needed
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_type != PlayerType.Bubble) return;

        PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (otherPlayer != null && otherPlayer.GetPlayerType() == PlayerType.Urchin)
        {
            GameManager.instance.BubbleReachedUrchin();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)
        );
    }

    [ContextMenu("Reset Attributes To Default")]
    public void ResetAllAttributes()
    {
        attributeController.ResetAllAttributes();
    }
}