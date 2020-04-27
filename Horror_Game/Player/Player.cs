using Godot;
using System;

public class Player : KinematicBody
{
    [Export]
    public Vector3 Velocity = new Vector3();
    [Export]
    public float Speed = 5f;
    [Export]
    public float MouseSensitivity = .25f;
    [Export]
    public bool IsHidden = false;
    [Export]
    public float FlashlightBattery = 100f; 

    private Spatial _head;
    private Camera _cam;
    private SpotLight _flashlight;
    private RayCast _interactRayCast;
    private Timer _batteryTimer;
    private ColorRect _settingsPage;
    private HSlider _sensitivitySlider;

    public override void _Ready()
    {
        _head = GetNode<Spatial>("Head");
        _cam = GetNode<Camera>("Head/Camera");
        _flashlight = GetNode<SpotLight>("Head/Flashlight");
        _interactRayCast = GetNode<RayCast>("Head/InteractRayCast");
        _batteryTimer = GetNode<Timer>("BatteryTimer");
        _settingsPage = GetNode<ColorRect>("/root/World/HUD/Settings");

        _batteryTimer.Connect("timeout", this, nameof(DrainBattery));

        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Process(float delta)
    {
        GetNode<Label>("/root/World/HUD/BatteryLabel").Text = $"Battery: {FlashlightBattery}%";
        _flashlight.LightEnergy = FlashlightBattery / 10f;
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity = MoveAndSlide(Velocity);
        //Make sure y pos is 2.5
        Vector3 pos = GlobalTransform.origin;
        GlobalTransform = new Transform(GlobalTransform.basis, new Vector3(pos.x, 2.5f, pos.z));

        if (_interactRayCast.IsColliding())
        {
            Spatial collider = (Spatial)_interactRayCast.GetCollider();
            if (collider.IsInGroup("Items"))
            {
                //add item pickup logic
            }
        }
    }

    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseMotion)
        {
            var e = @event as InputEventMouseMotion;
            RotateY(-Mathf.Deg2Rad(e.Relative.x * MouseSensitivity));
            _head.RotateX(-Mathf.Deg2Rad(e.Relative.y * MouseSensitivity));

            var rotX = Mathf.Clamp(_head.Rotation.x, -2f, 2f);
            _head.Rotation = new Vector3(rotX, _head.Rotation.y, _head.Rotation.z);
        }

        if (Input.IsActionJustPressed("ToggleFlashlight"))
        {
            _flashlight.Visible = !_flashlight.Visible;
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            if (Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                Input.SetMouseMode(Input.MouseMode.Visible);
            }
            else
            {
                Input.SetMouseMode(Input.MouseMode.Captured);
            }
        }

        // Movement Logic

        Vector3 directionVector = new Vector3();
        Basis headBasis = _head.GlobalTransform.basis;

        if (Input.IsActionPressed("MoveForward"))
        {
            directionVector -= headBasis.z;
        }
        if (Input.IsActionPressed("MoveBackward"))
        {
            directionVector += headBasis.z;
        }
        if (Input.IsActionPressed("MoveLeft"))
        {
            directionVector -= headBasis.x;
        }
        if (Input.IsActionPressed("MoveRight"))
        {
            directionVector += headBasis.x;
        }

        Velocity = directionVector.Normalized() * Speed;
    }

    private void DrainBattery()
    {
        if (_flashlight.Visible)
        {
            FlashlightBattery--;
        }
    }
}
