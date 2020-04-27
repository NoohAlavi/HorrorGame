using Godot;
using System;
using System.Collections.Generic;

public class Killer : KinematicBody
{
    [Export]
    public Vector3 Velocity = new Vector3();
    [Export]
    public float Speed = 2f;

    public enum States
    {
        Idle,
        Walk,
        FollowNoise,
        Chase
    }

    [Export]
    private States _state = States.Idle;

    private Player _targetPlayer;
    private List<Vector3> _points;
    private Navigation _nav;

    public override void _Ready()
    {
        _nav = GetParent<Navigation>();
        _targetPlayer = GetNode<Player>("../Player");
    }

    public override void _PhysicsProcess(float delta)
    {
        //implement fsm

        //

        _points = new List<Vector3>(_nav.GetSimplePath(GlobalTransform.origin, _targetPlayer.GlobalTransform.origin));

        if (_points.Count > 0)
        {
            GD.Print(RoundVector(_points[0]), " ", RoundVector(GlobalTransform.origin), " ", (RoundVector(GlobalTransform.origin).x == RoundVector(_points[0]).x && RoundVector(GlobalTransform.origin).z == RoundVector(_points[0]).z));
            if (RoundVector(GlobalTransform.origin).x == RoundVector(_points[0]).x && RoundVector(GlobalTransform.origin).z == RoundVector(_points[0]).z)
            {
                GD.Print("Removed Point 0");
                _points.RemoveAt(0);
            }
            Velocity = Translation.DirectionTo(_points[0]) * Speed;
            Velocity = MoveAndSlide(Velocity);
        }

        //Make sure y pos is 2.5
        Vector3 pos = GlobalTransform.origin;
        GlobalTransform = new Transform(GlobalTransform.basis, new Vector3(pos.x, 2.5f, pos.z));
    }

    private Vector3 RoundVector(Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
    }
}
