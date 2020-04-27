using Godot;
using System;

public class MainMenu : Control
{
    public override void _Ready()
    {
        GetNode<Button>("PlayButton").Connect("pressed", this, nameof(StartGame));
    }

    private void StartGame()
    {
        GetTree().ChangeScene("res://Instructions/Instructions.tscn");
    }
}
