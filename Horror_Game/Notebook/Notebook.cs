using Godot;
using System;

public class Notebook : TextEdit
{
    public override void _Ready()
    {
        Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ToggleNotebook"))
        {
            if (Visible)
            {
                Input.SetMouseMode(Input.MouseMode.Captured);
                Visible = false;
            }
            else
            {
                Input.SetMouseMode(Input.MouseMode.Visible);
                Visible = true;
            }   
        }
    }
}
