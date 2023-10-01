using Godot;
using System;

public partial class GroundState : State
{
    [Export]
    public State AirState;
    [Export]
    public State CrouchState;
    [Export]
    public State AttackingState;
    [Export]
    public string AttackAnimation = "combo";
    [Export]
    public const float JumpVelocity = -400.0f;

    public override void StateInput(InputEvent @event)
    {
        if (Character.IsOnFloor())
        {
            if (@event.IsActionPressed("jump")) Jump();
            if (@event.IsActionPressed("attack")) Attack();
            if (Input.IsActionJustPressed("crouch")) Crouch();

        }
    }

    private void Jump()
    {
        Vector2 velocity = new Vector2(0, JumpVelocity);

        Character.Velocity = velocity;

        NextState = AirState;

        Playback.Travel("jump");
    }

    private void Attack()
    {
        NextState = AttackingState;

        Playback.Travel(AttackAnimation);
    }

    private void Crouch()
    {
        NextState = CrouchState;

        Playback.Travel("crouch");
    }

    public override void StateProcess(double delta)
    {
        if (!Character.IsOnFloor())
        {
            NextState = AirState;
        }
    }

}
