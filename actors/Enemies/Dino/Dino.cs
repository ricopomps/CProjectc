using Godot;
using System;

public partial class Dino : CharacterBody2D
{

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public new float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private bool hit = false;
    private Timer hitTimer;
    private string animation;
    private AnimationPlayer animator;
    [Export]
    public Damageable damageable;
    public override void _Ready()
    {
        animator = GetNode<AnimationPlayer>("AnimationPlayer");
        damageable.Connect("OnHitEventHandler", new Callable(this, nameof(OnDamageableHit)));
        hitTimer = GetNode<Timer>("HitTimer");
        hitTimer.Timeout += OnHitTimerTimeout;
    }
    private void OnDamageableHit(Node node, float ammountChanged)
    {
        hit = true;
        hitTimer.Start();
        GD.Print("OUCH!");
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        Velocity = velocity;
        Animation(velocity);
        MoveAndSlide();
    }

    private void Animation(Vector2 vector)
    {
        string anim;
        if (IsOnFloor())
        {
            if (hit)
            {
                anim = "hurt";
            }
            else
            {
                anim = vector.X != 0 ? "run" : "idle";
            }
        }
        else
        {
            anim = vector.Y > 0 ? "fall" : "jump";
        }
        SetAnimation(anim);
    }

    private void SetAnimation(string anim)
    {
        if (animation != anim)
        {
            animation = anim;
            animator.Play(anim);
        }
    }

    private void OnHitTimerTimeout()
    {
        hit = false;
    }

}
