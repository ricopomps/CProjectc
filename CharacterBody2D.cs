using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;

    private Sprite2D sprite;
    private string animation;
    private AnimationPlayer animator;
    private AnimationTree animationTree;
    private Timer attackTimer;
    private Timer landTimer;
    private Area2D swordHitBox;
    private CollisionShape2D shape;
    private CollisionShape2D crouchShape;
    [Export]
    private CharacterStateMachine StateMachine;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private bool doubleJumpAvailable = true;
    private bool landing = false;
    private bool shouldTriggerLandAnimation = false;
    private bool attacking = false;
    private bool crouching = false;
    private int attackChain = 0;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        animator = GetNode<AnimationPlayer>("AnimationPlayer");
        attackTimer = GetNode<Timer>("AttackTimer");
        landTimer = GetNode<Timer>("LandTimer");
        shape = GetNode<CollisionShape2D>("CollisionShape2D");
        crouchShape = GetNode<CollisionShape2D>("CrouchColisionShape");
        swordHitBox = GetNode<Area2D>("Sword");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        // StateMachine = GetNode<CharacterStateMachine>("CharacterStateMachine");
        attackTimer.Timeout += OnAttackTimerTimeout;
        landTimer.Timeout += OnLandTimerTimeout;
        crouchShape.Disabled = true;
        animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
            shouldTriggerLandAnimation = true;
            DisableCrouch();
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

        // if (Input.IsActionJustPressed("crouch") && IsOnFloor() && !attacking && !landing)
        // {
        //     HandleCrouch();
        // }

        // Handle Jump.
        // if ((Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("move_up")) && (doubleJumpAvailable || IsOnFloor()))
        // {
        //     if (!IsOnFloor())
        //         doubleJumpAvailable = false;
        //     velocity.Y = JumpVelocity;
        // }

        if (Input.IsActionJustPressed("attack") && IsOnFloor())
            HandleAttack();

        Flip(velocity);
        // Animation(velocity);
        SetAnimationTree(velocity);
        if (!attacking && StateMachine.CheckIfCanMove()) HandleMovement(velocity);

    }


    private void Flip(Vector2 vector)
    {
        if (vector.X != 0)
        {
            sprite.FlipH = (vector.X > 0) ? false : true;
            swordHitBox.Scale = (vector.X > 0) ? new Vector2(1, 1) : new Vector2(-1, 1);
        }
    }

    private void Animation(Vector2 vector)
    {
        string anim;
        if (IsOnFloor())
        {
            if (attacking)
            {
                // if (attackChain == 0)
                // {
                //     anim = "attack1";
                // }
                // else if (attackChain == 1)
                // {
                //     anim = "attack2";
                // }
                // else if (attackChain == 2)
                // {
                //     anim = "attack3";
                // }
                // else
                // {
                //     anim = "combo";
                // }
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
            // anim = vector.Y > 0 ? "fall" : "jump";
            anim = "";
        }
        SetAnimation(anim, vector);
    }

    private void SetAnimation(string anim, Vector2 vector)
    {
        if (animation != anim)
        {
            animation = anim;
            animationTree.Set("parameters/move/blend_position", vector.X);
            // animator.Play(anim);
        }
    }

    private void SetAnimationTree(Vector2 vector)
    {
        animationTree.Set("parameters/move/blend_position", vector.X);
    }

    private void HandleAttack()
    {
        attacking = true;
        DisableCrouch();
        attackTimer.Start();
    }

    private void ResetAttackChain()
    {
        attackChain = 0;
    }

    private void HandleLand()
    {
        landing = true;
        landTimer.Start();
    }

    private void HandleCrouch()
    {
        if (crouching)
            DisableCrouch();
        else
            EnableCrouch();
    }

    private void DisableCrouch()
    {
        crouching = false;
        shape.Disabled = false;
        crouchShape.Disabled = true;
    }

    private void EnableCrouch()
    {
        crouching = true;
        shape.Disabled = true;
        crouchShape.Disabled = false;
    }

    private void OnAttackTimerTimeout()
    {
        attacking = false;
        attackChain = (attackChain + 1) % 3;
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
            DisableCrouch();
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        Velocity = velocity;
        MoveAndSlide();
    }

}
