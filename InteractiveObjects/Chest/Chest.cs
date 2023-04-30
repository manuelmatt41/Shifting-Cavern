using Godot;
using System;

public partial class Chest : RigidBody2D
{
    [Signal]
    public delegate void OpenChestInventoryEventHandler(InventoryData chestInventory);

    [Export]
    public InventoryData InventoryData { get; set; }

    private void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (
            @event is InputEventMouseButton mouseEvent
            && mouseEvent.ButtonIndex == MouseButton.Right
            && mouseEvent.IsPressed()
        )
        {
            this.EmitSignal(SignalName.OpenChestInventory, this.InventoryData);
        }
    }
}
