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
    private RayCast _rayCast;

    public override void _Ready()
    {
        _nav = GetParent<Navigation>();
        _targetPlayer = GetNode<Player>("../Player");
        _rayCast = GetNode<RayCast>("RayCast");
    }

    public override void _PhysicsProcess(float delta)
    {
        //implement fsm

        //pathfinding logic
        _points = new List<Vector3>(_nav.GetSimplePath(GlobalTransform.origin, _targetPlayer.GlobalTransform.origin));

        if (_points.Count > 0)
        {
            if (RoundVector(GlobalTransform.origin).x == RoundVector(_points[0]).x && RoundVector(GlobalTransform.origin).z == RoundVector(_points[0]).z)
            {
                _points.RemoveAt(0);
            }
            Velocity = Translation.DirectionTo(_points[0]) * Speed;
            Velocity = MoveAndSlide(Velocity);
        }

        //Make sure y pos is 2.5
        // Vector3 pos = GlobalTransform.origin;
        // GlobalTransform = new Transform(GlobalTransform.basis, new Vector3(pos.x, 3f, pos.z));

        //Collision Logic

        _rayCast.CastTo = _targetPlayer.GlobalTransform.origin;
        if (_rayCast.IsColliding())
        {
            Spatial collider = (Spatial)_rayCast.GetCollider();
            if (collider is Player)
            {
                GetTree().ChangeScene("res://GameOver/GameOver.tscn");
            }
        }
    }

    private Vector3 RoundVector(Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
    }
}
