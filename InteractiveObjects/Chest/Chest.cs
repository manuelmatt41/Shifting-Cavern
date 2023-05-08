using Godot;

public partial class Chest : RigidBody2D
{
    [Signal]
    public delegate void OpenChestInventoryEventHandler(InventoryData chestInventory);

    [Export]
    public ChestResource ChestResource { get; set; }

    [Export]
    public ChestLootType ChestLootType { get; set; }

    public override void _Ready()
    {
        this.ChestResource = Main.SaveGame.ChestResources.ContainsKey(this.Name)
            ? Main.SaveGame.ChestResources[this.Name]
            : null;

        if (this.ChestResource == null)
        {
            switch (this.ChestLootType)
            {
                case ChestLootType.INITIAL:
                    this.ChestResource = ChestResource.SetInitialLootTableChest();
                    break;
                case ChestLootType.SECRET:
                    this.ChestResource = ChestResource.SetSecretLootTableChest();
                    break;
            }
        }
    }

    private void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (
            @event is InputEventMouseButton mouseEvent
            && mouseEvent.ButtonIndex == MouseButton.Right
            && mouseEvent.IsPressed()
        )
        {
            this.EmitSignal(SignalName.OpenChestInventory, this.ChestResource.InventoryData);
        }
    }
}

public enum ChestLootType
{
    INITIAL,
    SECRET
}
