using Godot;
using System;

public partial class Damageable : Node
{
    [Export]
    public float health = 20f;
    // Called when the node enters the scene tree for the first time.
    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float value)
    {
        var globalSignals = GetNode<GlobalSignalBus>("/root/GlobalSignalBus");
        globalSignals.EmitSignal("HealthChangedEventHandler", GetParent(), value - health);
        GD.Print("EmitSignal", value);
        health = value;
    }
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Hit(float damage)
    {
        SetHealth(GetHealth() - damage);
        if (GetHealth() <= 0)
        {
            GetParent().QueueFree();
        }
    }
}
