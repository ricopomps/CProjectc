using Godot;
using System;

public partial class HealthChangeManager : Control
{
    [Export]
    public PackedScene healthChangedLabelScene;
    [Export]
    public Color damageColor = new Color("Red");

    [Export]
    public Color healColor = new Color("Green");
    public override void _Ready()
    {
        Callable callable = new Callable(this, nameof(OnSignalHealthChanged));
        var globalSignals = GetNode<GlobalSignalBus>("/root/GlobalSignalBus");
        globalSignals.Connect(GlobalSignalBus.HealthChangedEventHandlerName, callable);
    }

    public override void _Process(double delta)
    {
    }

    private void OnSignalHealthChanged(Node node, float ammountChanged)
    {
        var labelInstance = healthChangedLabelScene.Instantiate() as Label;
        node.AddChild(labelInstance);
        labelInstance.Text = ammountChanged.ToString();

        if (ammountChanged >= 0)
            labelInstance.Modulate = healColor;
        else
            labelInstance.Modulate = damageColor;
    }
}