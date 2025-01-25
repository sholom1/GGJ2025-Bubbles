using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Sprite bubble;
    [SerializeField]
    private Sprite urchin;
    [SerializeField]
    private new SpriteRenderer renderer;
    [SerializeField]
    private PlayerInput playerInput;
    private PlayerType _type = PlayerType.Unassigned;
    private void Start()
    {
        GameManager.instance.RegisterPlayer(this);
    }
    public void SetPlayerType(PlayerType value)
    {
        if (_type != PlayerType.Unassigned)
        {
            Debug.LogError($"Player is already assigned to {_type}");
            return;
        }
        if (value == PlayerType.Bubble)
        {
            // Add bubble specific componets;
            renderer.sprite = bubble;
        }
        else
        {
            // Add urchin specific components;
            renderer.sprite = urchin;
        }
    }
}

public enum PlayerType
{
    Unassigned,
    Bubble,
    Urchin
}
