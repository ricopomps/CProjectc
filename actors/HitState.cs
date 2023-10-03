using Godot;
using System;

public partial class HitState : State
{
    [Export]
    public Damageable Damageable;
    [Export]
    public CharacterStateMachine CharacterStateMachine;
    [Export]
    public State DeadState;
    [Export]
    private string DeadAnimationNode = "dead";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Damageable.Connect("OnHitEventHandler", new Callable(this, nameof(OnDamageableHit)));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnDamageableHit(Node node, float ammountChanged)
    {
        if (Damageable.Health > 0)
            EmitSignal("InterruptStateEventHandler", this);
        else
        {
            EmitSignal("InterruptStateEventHandler", DeadState);
            Playback.Travel(DeadAnimationNode);
        }
    }
}
