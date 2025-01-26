using UnityEngine;

public enum PlayerType
{
    Unassigned,
    Bubble,
    Urchin
}

[CreateAssetMenu(fileName = "Component", menuName = "ScriptableObjects/PlayerComponent", order = 1)]
public class PlayerComponentScriptableObject : ScriptableObject
{
    public Sprite PlayerSprite { get; set; }

    public PlayerType playerType;
    
    public float MovementSpeed = 5f;
    
    public float JumpForce = 5f;
    public int JumpFrequency = 1;
    
    public float DashSpeed = 10f;
    public float DashCooldown = 3f;
    public float DashDuration = 0.2f;
    
    public float GravityScale = 1f;

    
    public void SetMovementSpeed(float multiplier)
    {
        MovementSpeed *= multiplier;
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
        MovementSpeed = 5f;
        JumpForce = 5f;
        JumpFrequency = 1;
        DashSpeed = 10f;
        DashCooldown = 3f;
        DashDuration = 0.2f;
        GravityScale = 1f;
    }
}