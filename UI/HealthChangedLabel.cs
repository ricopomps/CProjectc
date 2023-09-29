using Godot;
using System;

public partial class HealthChangedLabel : Label
{
    [Export]
    public Vector2 floatSpeed = new Vector2(0, -50);

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position += floatSpeed * (float)delta;
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}