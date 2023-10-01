using Godot;
using System;

public partial class CrouchState : State
{
    [Export]
    private CollisionShape2D Shape;
    [Export]
    private CollisionShape2D CrouchShape;
    [Export]
    public const float JumpVelocity = -400.0f;
    [Export]
    public State AirState;
    [Export]
    public State GroundState;
    public override void StateInput(InputEvent @event)
    {
        if (Character.IsOnFloor())
        {
            if (Input.IsActionJustPressed("crouch"))
            {
                NextState = GroundState;
                Playback.Travel("move");
            }
            if (Input.IsActionJustPressed("jump"))
            {
                Jump();
            }
            if (Input.IsActionJustPressed("move_left") || Input.IsActionJustPressed("move_right"))
            {
                NextState = GroundState;
                Playback.Travel("move");
            }
        }
    }

    public override void OnEnter()
    {
        Shape.Disabled = true;
        CrouchShape.Disabled = false;
    }

    public override void OnExit()
    {
        Shape.Disabled = false;
        CrouchShape.Disabled = true;
    }
    private void Jump()
    {
        Vector2 velocity = new Vector2(0, JumpVelocity);

        Character.Velocity = velocity;

        NextState = AirState;

        Playback.Travel("jump");
    }
}
