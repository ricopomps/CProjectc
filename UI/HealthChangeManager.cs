using Godot;
using System;

public partial class HealthChangeManager : Control
{
    [Export]
    public PackedScene healthChangedLabelScene;
    private GlobalSignalBus signalBus;
    public override void _Ready()
    {
        Callable callable = new Callable(this, nameof(OnSignalHealthChanged));
        var globalSignals = GetNode<GlobalSignalBus>("/root/GlobalSignalBus");
        globalSignals.Connect(GlobalSignalBus.HealthChangedEventHandlerName, callable);
    }

    public override void _Process(double delta)
    {
    }

    private void OnSignalHealthChanged(Node node, float value)
    {
        var labelInstance = healthChangedLabelScene.Instantiate() as Label;
        node.AddChild(labelInstance);
        labelInstance.Text = value.ToString();
    }
}