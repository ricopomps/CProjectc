using Godot;
using System;

public partial class StateDebugLabel : Label
{

    [Export]
    private CharacterStateMachine StateMachine;

    public override void _Process(double delta)
    {
        Text = "State: " + StateMachine.CurrentState.Name;
    }
}
