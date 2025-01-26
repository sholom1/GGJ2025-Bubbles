using UnityEngine;
using UnityEngine.Serialization;

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

    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    
    public float MovementSpeed = 5f;
    
    public float JumpForce = 5f;
    public int JumpFrequency = 1;
    
    public float DashSpeed = 10f;
    public float DashCooldown = 3f;
    public float DashDuration = 0.2f;
    
    public float StunForce = 5f;
    public float StunCooldown = 3f;
    
    public float GravityScale = 1f;
    
    public void ResetAllAttributes()
    {
        MovementSpeed = 5f;
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