using Godot;
using System;

public class Instructions : Control
{
    public override void _Ready()
    {
        GetNode<Button>("ContinueButton").Connect("pressed", this, nameof(Continue));
    }

    private void Continue()
    {
        GetTree().ChangeScene("res://World/World.tscn");
    }
}
