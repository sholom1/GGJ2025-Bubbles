public class PlayerAttributeController
{
    public float MoveSpeed { get; private set; }

    public float JumpForce { get; private set; }
    public int JumpFrequency { get; private set; }

    public float DashSpeed { get; private set; }
    public float DashCooldown { get; private set; }
    public float DashDuration { get; private set; }

    public float StunForce { get; private set; }
    public float StunCooldown { get; private set; }
    
    public float GravityScale { get; private set; }

    public PlayerAttributeController(PlayerComponentScriptableObject playerComponent)
    {
        MoveSpeed = playerComponent.MovementSpeed;
        JumpForce = playerComponent.JumpForce;
        JumpFrequency = playerComponent.JumpFrequency;
        DashSpeed = playerComponent.DashSpeed;
        DashCooldown = playerComponent.DashCooldown;
        DashDuration = playerComponent.DashDuration;
        StunForce = playerComponent.StunForce;
        StunCooldown = playerComponent.StunCooldown;
    }
    
    public void SetMovementSpeed(float multiplier)
    {
        MoveSpeed *= multiplier;
    }

    public void SetJumpForce(float multiplier)
    {
        JumpForce *= multiplier;
    }

    public void AddExtraJump(int value)
    {
        JumpFrequency += value;
    }

    public void SetDashSpeed(float multiplier)
    {
        DashSpeed *= multiplier;
    }

    public void SetDashCooldown(float multiplier)
    {
        DashCooldown *= multiplier;
    }

    public void SetDashDuration(float multiplier)
    {
        DashDuration *= multiplier;
    }

    public void SetGravityScale(float multiplier)
    {
        GravityScale *= multiplier;
    }
    
    public void ResetAllAttributes()
    {
        MoveSpeed = 5f;
        JumpForce = 5f;
        JumpFrequency = 1;
        DashSpeed = 10f;
        DashCooldown = 3f;
        DashDuration = 0.2f;
        StunForce = 5f;
        StunCooldown = 3f;
        GravityScale = 1f;
    }
}