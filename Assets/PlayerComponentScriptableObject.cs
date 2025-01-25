using System.Collections.Generic;
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
    public float DashCooldown = 2f;
    public float DashDuration = 0.2f;
    
    public float Gravity = 9.81f;
}