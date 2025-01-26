public class PlayerAttributeController
{
    public float MoveSpeed { get; private set; }

    public float JumpForce { get; private set; }
    public int JumpFrequency { get; private set; }

    public float DashSpeed { get; private set; }
    public float DashCooldown { get; private set; }
    public float DashDuration { get; private set; }

    public float GravityScale { get; set; }

    public PlayerAttributeController(PlayerComponentScriptableObject playerComponent)
    {
        MoveSpeed = playerComponent.MovementSpeed;
        JumpForce = playerComponent.JumpForce;
        JumpFrequency = playerComponent.JumpFrequency;
        DashSpeed = playerComponent.DashSpeed;
        DashCooldown = playerComponent.DashCooldown;
        DashDuration = playerComponent.DashDuration;
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
        GravityScale = 1f;
    }
}