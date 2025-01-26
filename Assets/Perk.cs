using System;

public class Perk {

    public string Name {get; set;}
    public Action<PlayerController> Action {get; set;}

    public Perk(string name, Action<PlayerController> action)
    {
        Name = name;
        Action = action;
    }
}

