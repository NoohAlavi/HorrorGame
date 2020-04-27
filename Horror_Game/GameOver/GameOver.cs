using Godot;
using System;

public class GameOver : Control
{
    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Visible);
        GetNode<Button>("RetryButton").Connect("pressed", this, nameof(Retry));
    }

    private void Retry()
    {
        GetTree().ChangeScene("res://MainMenu/MainMenu.tscn");
    }
}
