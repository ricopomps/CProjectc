using Godot;
using System;

public partial class Dino : CharacterBody2D
{

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public new float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public float Speed = 30.0f;
    private Sprite2D sprite;
    private bool hit = false;
    private bool dead = false;
    private Timer hitTimer;
    private Timer movementTimer;
    private string animation;
    private AnimationPlayer animator;
    [Export]
    public Damageable damageable;
    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        animator = GetNode<AnimationPlayer>("AnimationPlayer");
        damageable.Connect("OnHitEventHandler", new Callable(this, nameof(OnDamageableHit)));
        hitTimer = GetNode<Timer>("HitTimer");
        movementTimer = GetNode<Timer>("MovementTimer");
        hitTimer.Timeout += OnHitTimerTimeout;
        movementTimer.Timeout += OnMovementTimerTimeout;
    }
    private void OnDamageableHit(Node node, float ammountChanged)
    {
        if (damageable.Health <= 0) dead = true;
        hit = true;
        hitTimer.Start();
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        Animation(velocity);
        if (!hit) HandleMovement(velocity);
    }

    private void HandleMovement(Vector2 velocity)
    {
        Vector2 direction = new Vector2(-1, 0);
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        Velocity = velocity;
        Flip(velocity);
        MoveAndSlide();
    }

    private void Flip(Vector2 vector)
    {
        if (vector.X != 0)
        {
            sprite.FlipH = vector.X > 0;
        }
    }

    private void Animation(Vector2 vector)
    {
        string anim;
        if (IsOnFloor())
        {
            if (dead)
            {
                anim = "dead";
            }
            else if (hit)
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

    private void OnMovementTimerTimeout()
    {
        Speed = -Speed;
    }

}
