using UnityEngine;

[System.Serializable]
public class PlayerAttributeController
{
    [SerializeField]
    public float MoveSpeed { get; private set; }

[SerializeField]
    public float JumpForce { get; private set; }
    [SerializeField]
    public int JumpFrequency { get; private set; }

[SerializeField]
    public float DashSpeed { get; private set; }
    [SerializeField]
    public float DashCooldown { get; private set; }
    [SerializeField]
    public float DashDuration { get; private set; }
[SerializeField]
    public float StunForce { get; private set; }
    [SerializeField]
    public float StunCooldown { get; private set; }
    [SerializeField]
    public float GravityScale { get; private set; }

    private PlayerComponentScriptableObject _playerComponent;

    public PlayerAttributeController(PlayerComponentScriptableObject playerComponent)
    {
        _playerComponent = playerComponent;

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
        MoveSpeed = _playerComponent.MovementSpeed;
        JumpForce = _playerComponent.JumpForce;
        JumpFrequency = _playerComponent.JumpFrequency;
        DashSpeed = _playerComponent.DashSpeed;
        DashCooldown = _playerComponent.DashCooldown;
        DashDuration = _playerComponent.DashDuration;
        StunForce = _playerComponent.StunForce;
        StunCooldown = _playerComponent.StunCooldown;
    }
}