using Godot;
using System;

public partial class Damageable : Node
{
    [Export]
    public float health = 20f;

    [Signal]
    public delegate void OnHitEventHandler(Node node, float amountChanged);
    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float value)
    {
        var globalSignals = GetNode<GlobalSignalBus>("/root/GlobalSignalBus");
        globalSignals.EmitSignal("HealthChangedEventHandler", GetParent(), value - health);
        health = value;
    }
    public override void _Ready()
    {
        AddUserSignal(nameof(OnHitEventHandler));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Hit(float damage)
    {
        EmitSignal(nameof(OnHitEventHandler), GetParent(), damage);
        SetHealth(GetHealth() - damage);
        // if (GetHealth() <= 0)
        // {
        //     GetParent().QueueFree();
        // }
    }

    private void OnAnimationFinished(string animationName)
    {
        if (animationName == "dead")
        {
            GetParent().QueueFree();
        }
    }
}
