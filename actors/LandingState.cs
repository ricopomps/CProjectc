using Godot;
using System;

public partial class LandingState : State
{
    [Export]
    private GroundState GroundState;
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnAnimationFinished(string animationName)
    {
        if (animationName == "landed") NextState = GroundState;
    }
}
