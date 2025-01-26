using System;

public class Perk {

    public string Name {get; set;}
    public string Description {get; set;}
    public Action<PlayerController> Action {get; set;}

    public Perk(string name, string description, Action<PlayerController> action)
    {
        Name = name;
        Description = description;
        Action = action;
    }
}

