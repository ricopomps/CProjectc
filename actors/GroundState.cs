using Godot;
using System;

public partial class GroundState : State
{
    [Export]
    public State AirState;
    public const float JumpVelocity = -400.0f;
    public override void StateInput(InputEvent @event)
    {
        if (@event.IsActionPressed("jump")) Jump();
    }

    private void Jump()
    {
        Vector2 velocity = new Vector2(0, JumpVelocity);

        Character.Velocity = velocity;

        NextState = AirState;

        Playback.Travel("jump");
    }

    public override void StateProcess(double delta)
    {
        if (!Character.IsOnFloor())
        {
            NextState = AirState;
        }
    }

}
