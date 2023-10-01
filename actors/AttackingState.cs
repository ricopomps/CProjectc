using Godot;
using System;

public partial class AttackingState : State
{
    [Export]
    private GroundState GroundState;
    [Export]
    private string Atatack1Name = "attack1";
    [Export]
    private string Atatack2Name = "attack2";
    [Export]
    private string Atatack3Name = "attack3";
    private Timer Timer;
    public override void _Ready()
    {
        Timer = GetNode<Timer>("Timer");
    }

    public override void StateInput(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            Timer.Start();
        }
    }
    public void OnAnimationFinished(string animationName)
    {
        if (animationName == "combo") NextState = GroundState;
        if (animationName == Atatack1Name)
        {
            if (Timer.IsStopped())
                Return();
            else
                Playback.Travel(Atatack2Name);
        }
        if (animationName == Atatack2Name)
        {
            if (Timer.IsStopped())
                Return();
            else
                Playback.Travel(Atatack3Name);
        }
        if (animationName == Atatack3Name)
            Return();
    }

    private void Return()
    {
        NextState = GroundState;
        Playback.Travel("move");
    }
}
