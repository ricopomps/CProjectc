using Godot;
using System;

public partial class State : Node
{
    [Export]
    public bool CanMove = true;
    public CharacterBody2D Character;
    public State NextState;
    public AnimationNodeStateMachinePlayback Playback;
    public virtual void StateInput(InputEvent inputEvent)
    {

    }

    public virtual void StateProcess(double delta)
    {
    }

    public virtual void StateProcess(double delta, Vector2 velocity)
    {
    }

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }
    public virtual void OnExit()
    {

    }

    public virtual void OnEnter()
    {

    }
}
