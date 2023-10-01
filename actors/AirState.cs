using Godot;
using System;

public partial class AirState : State
{
    [Export]
    private State LandingState;
    [Export]
    private float DoubleJumpVelocity = -100;

    private bool HasDoubleJumped = false;

    public override void StateProcess(double delta)
    {
        if (Character.IsOnFloor() && Character.Velocity.Y == 0)
        {
            NextState = LandingState;
        }

        if (Character.Velocity.Y > 0)
        {
            Playback.Travel("fall");
        }
    }

    public override void OnExit()
    {
        if (NextState == LandingState)
        {
            Playback.Travel("landed");
            HasDoubleJumped = false;
        }
    }

    public override void StateInput(InputEvent @event)
    {
        if (@event.IsActionPressed("jump") && !HasDoubleJumped) DoubleJump();
    }


    private void DoubleJump()
    {
        Vector2 velocity = new Vector2(0, DoubleJumpVelocity);
        if (Character.Velocity.Y > 0)
        {
            Playback.Travel("jump");
        }
        Character.Velocity = velocity;
        HasDoubleJumped = true;
    }
}
