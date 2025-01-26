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
    protected Transform enemyCheck;

    [SerializeField]
    protected float groundCheckDistance;
    [SerializeField]
    protected float enemyCheckDistance;
    
    [SerializeField]
    protected LayerMask groundLayer;
    [SerializeField]
    protected LayerMask enemyLayer;

    private PlayerType _type = PlayerType.Unassigned;
    
    private PlayerAttributeController _attributeController;
    private int currentJumpCount = 0;
    private float dashDurationTimer;
    private float lastDashTime;
    private float lastStunTime;
    private bool isPushed = false;


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
        dashDurationTimer -= Time.deltaTime;
        PlayerMovement();
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

    public void SetPlayerComponent(PlayerComponentScriptableObject playerComponent)
    {
        if (playerComponent == null)
        {
            return;
        }

        _attributeController = new PlayerAttributeController(playerComponent);

        gameObject.layer = LayerMask.NameToLayer(_type == PlayerType.Bubble ? "Bubble" : "Urchin");

        enemyLayer = playerComponent.EnemyLayer;

        rb.gravityScale = playerComponent.GravityScale;

        renderer.sprite = playerComponent.PlayerSprite;
    }
    
    public virtual void OnMove(InputValue value)
    {
        var inputValues = value.Get<Vector2>();

        _inputValues = inputValues;
    }

    private void PlayerMovement() {
        if (IsDashing()) {
            return;
        }
        rb.linearVelocity = new Vector2(
            _inputValues.x * _attributeController.MoveSpeed,
            rb.linearVelocityY
        );
    }

    public virtual void OnSprint(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if(Time.time - lastDashTime < _attributeController.DashCooldown) {
            return;
        }

        dashDurationTimer = _attributeController.DashDuration;
        lastDashTime = Time.time;
        rb.linearVelocity = new Vector2(
            _inputValues.x * _attributeController.DashSpeed,
            rb.linearVelocityY
        );
    }
    
    private bool CanJump() {
        bool groundDetected = IsGroundDetected();
        if (groundDetected) {
            currentJumpCount = 0;
            return true;
        }

        if (currentJumpCount < _attributeController.JumpFrequency) {
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
            _attributeController.JumpForce
        );

        currentJumpCount++;
    }

    void OnAttack(InputValue value)
    {
        if(Time.time - lastStunTime < _attributeController.StunCooldown) {
            return;
        }
        lastStunTime = Time.time;
        
        if (value.isPressed && IsEnemyDetected())
        {
            Push();
        }
    }

    public void Push()
    {
        Debug.LogError($"pushed {_type}");
    }
    
    public virtual bool IsEnemyDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(enemyCheck.position, Vector2.left, enemyCheckDistance, enemyLayer);
    
        if (hit)
        {
            PlayerController detectedController = hit.transform.GetComponent<PlayerController>();
        
            if (detectedController != null && detectedController != this)
            {
                Debug.LogError($"Enemy detected: {hit.transform.name}");
                detectedController.Push();
                return true;
            }
        }
    
        return false;
    }
    
    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    bool IsDashing() => dashDurationTimer > 0;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)
        );
        
        Gizmos.DrawLine(
            enemyCheck.position,
            new Vector3(enemyCheck.position.x- enemyCheckDistance, enemyCheck.position.y)
        );
    }

    [ContextMenu("Reset Attributes To Default")]
    public void ResetAllAttributes()
    {
        _attributeController.ResetAllAttributes();
    }
}