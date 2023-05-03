using Godot;

public partial class ChestResource : Resource
{
    [Export]
    public InventoryData InventoryData { get; set; }

    public ChestResource()
    {
        this.InventoryData = new InventoryData(27);
    }
}
