using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

    private AnimatedSprite2D sprite;
    private string animation;
    private AnimationPlayer animator;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private bool doubleJumpAvailable = true;
    // private bool landed = true;
    
    
    public override void _Ready(){
        sprite = GetNode<AnimatedSprite2D>("Sprite");
        animator = GetNode<AnimationPlayer>("AnimationPlayer");
        
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Add the gravity.
		if (!IsOnFloor()){
			velocity.Y += gravity * (float)delta;
            // landed = false;
        }

        if (IsOnFloor())
        {
            // landed = false;
            doubleJumpAvailable = true;
        }

		// Handle Jump.
		if ((Input.IsActionJustPressed("ui_accept") || Input.IsActionJustPressed("move_up") )&& (doubleJumpAvailable || IsOnFloor())){
            if (!IsOnFloor())
                doubleJumpAvailable = false;
			velocity.Y = JumpVelocity;
        }

        Flip(velocity);
        Animation(velocity);

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		// Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector2 direction = new Vector2(
    Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
    Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
);
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


private void Flip(Vector2 vector){
    
    if (vector.X != 0)
        sprite.FlipH = (vector.X > 0) ? false : true;
}

private void Animation(Vector2 vector){
    string anim;
    if(IsOnFloor()){
        anim = vector.X != 0 ? "run" : "idle";
    } else {
        anim = vector.Y > 0 ? "fall" : "jump";
    }
    SetAnimation(anim);
}

private void SetAnimation(string anim){
    if(animation != anim){
        animation = anim;
        sprite.Play(animation);
    }
}
    
}
