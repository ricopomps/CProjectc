using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterStateMachine : Node
{
    [Export]
    public State CurrentState;
    [Export]
    private CharacterBody2D Character;
    [Export]
    private AnimationTree AnimationTree;
    private List<State> States = new List<State>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                States.Add(state);
                state.Character = Character;
                state.Playback = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");

                state.Connect("InterruptStateEventHandler", new Callable(this, nameof(OnStateInterruptState)));
            }
        }
    }

    public bool CheckIfCanMove()
    {
        return CurrentState.CanMove;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (CurrentState.NextState != null)
        {
            SwitchStates(CurrentState.NextState);
        }

        CurrentState.StateProcess(delta);
    }

    private void SwitchStates(State newState)
    {
        GD.Print("GOING FROM " + CurrentState.Name + " TO " + newState.Name);
        if (CurrentState != null)
        {
            CurrentState.OnExit();
            CurrentState.NextState = null;
        }

        CurrentState = newState;

        CurrentState.OnEnter();
    }

    public override void _Input(InputEvent @event)
    {
        CurrentState.StateInput(@event);
    }

    private void OnStateInterruptState(State newState)
    {
        SwitchStates(newState);
    }
}
