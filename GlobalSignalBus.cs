using Godot;
using System;

public partial class GlobalSignalBus : Node
{

    [Signal]
    public delegate void HealthChangedEventHandler(Node node, float amountChanged);
    public static string HealthChangedEventHandlerName = nameof(HealthChangedEventHandler);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddUserSignal(nameof(HealthChangedEventHandler));
    }
}