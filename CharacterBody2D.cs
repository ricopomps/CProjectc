using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;

    private Sprite2D sprite;
    private string animation;
    private AnimationPlayer animator;
    private Timer attackTimer;
    private Timer landTimer;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private bool doubleJumpAvailable = true;
    private bool landing = false;
    private bool shouldTriggerLandAnimation = false;
    private bool attacking = false;
    private bool crouching = false;


    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        animator = GetNode<AnimationPlayer>("AnimationPlayer");
        attackTimer = GetNode<Timer>("AttackTimer");
        landTimer = GetNode<Timer>("LandTimer");
        attackTimer.Timeout += OnAttackTimerTimeout;
        landTimer.Timeout += OnLandTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
            shouldTriggerLandAnimation = true;
            crouching = false;
        }

        if (IsOnFloor())
        {
            if (shouldTriggerLandAnimation)
            {
                HandleLand();
                shouldTriggerLandAnimation = false;
            }

            doubleJumpAvailable = true;
        }

        if (IsOnFloor() && !attacking && Input.IsActionJustPressed("crouch"))
        {
            HandCrouch();
        }

        // Handle Jump.
        if ((Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("move_up")) && (doubleJumpAvailable || IsOnFloor()))
        {
            if (!IsOnFloor())
                doubleJumpAvailable = false;
            velocity.Y = JumpVelocity;
        }

        if (Input.IsActionJustPressed("attack") && IsOnFloor())
            HandleAttack();

        Flip(velocity);
        Animation(velocity);
        if (!attacking) HandleMovement(velocity);

    }


    private void Flip(Vector2 vector)
    {
        if (vector.X != 0)
            sprite.FlipH = (vector.X > 0) ? false : true;
    }

    private void Animation(Vector2 vector)
    {
        string anim;
        if (IsOnFloor())
        {
            if (attacking)
            {
                anim = "combo";
            }
            else if (landing)
            {
                anim = "landed";
            }
            else if (crouching)
            {
                anim = "crouch";
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
            // sprite.Play(animation);
            animator.Play(anim);
        }
    }

    private void HandleAttack()
    {
        attacking = true;
        crouching = false;
        attackTimer.Start();
    }

    private void HandleLand()
    {
        landing = true;
        landTimer.Start();
    }

    private void HandCrouch()
    {
        crouching = !crouching;
    }

    private void OnAttackTimerTimeout()
    {
        attacking = false;
    }

    private void OnLandTimerTimeout()
    {
        landing = false;
    }

    private void HandleMovement(Vector2 velocity)
    {
        Vector2 direction = new Vector2(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
                                                Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"));

        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            crouching = false;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        Velocity = velocity;
        MoveAndSlide();
    }

}
