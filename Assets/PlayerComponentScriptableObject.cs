using UnityEngine;

[CreateAssetMenu(fileName = "Component", menuName = "ScriptableObjects/PlayerComponent", order = 1)]
public class PlayerComponentScriptableObject : ScriptableObject
{
    public float MovementSpeed = 5f;
    
    public float JumpForce = 5f;
    public int JumpFrequency = 1;
    
    public float DashDistance = 10f;
    public float DashCooldown = 2f;
    
    public float Gravity = 9.8f;
}