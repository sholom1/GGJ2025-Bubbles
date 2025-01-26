using System;
using UnityEngine;

[Serializable]
public class CinematicScene {
    public string Text;
    public Sprite Sprite;
    public float Duration;

    public CinematicScene(string text, Sprite sprite, float duration)
    {
        Text = text;
        Sprite = sprite;
        Duration = duration;
    }
}