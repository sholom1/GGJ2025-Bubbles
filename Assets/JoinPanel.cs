using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class JoinPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI joinText;
    //[SerializeField]
    //private Image controlTheme;
    //[SerializeField]
    //private Sprite controller;
    //[SerializeField]
    //private Sprite keyboard;


    //public void SetControlTheme()
    //{
    //    if (scheme == InputControlScheme.DeviceRequirement)
    //}

    public void UpdateJoinText()
    {
        joinText.text = "Joined!";
    }
}
