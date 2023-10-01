using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
    public const float Speed = 300.0f;
    private Sprite2D sprite;
    private AnimationTree animationTree;
    private Area2D swordHitBox;
    [Export]
    private CharacterStateMachine StateMachine;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        swordHitBox = GetNode<Area2D>("Sword");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        Flip(velocity);
        SetAnimationTree(velocity);
        if (StateMachine.CheckIfCanMove()) HandleMovement(velocity);

    }


    private void Flip(Vector2 vector)
    {
        if (vector.X != 0)
        {
            sprite.FlipH = (vector.X > 0) ? false : true;
            swordHitBox.Scale = (vector.X > 0) ? new Vector2(1, 1) : new Vector2(-1, 1);
        }
    }

    private void SetAnimationTree(Vector2 vector)
    {
        animationTree.Set("parameters/move/blend_position", vector.X);
    }

    private void HandleMovement(Vector2 velocity)
    {
        Vector2 direction = new Vector2(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
                                                Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"));

        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        Velocity = velocity;
        MoveAndSlide();
    }

}
