using Godot;

public partial class PlayerResource : Resource
{
    [Export]
    public double Life { get; set; }

    [Export]
    public float MoveSpeed { get; set; }

    [Export]
    public float DashSpeed { get; set; }

    [Export]
    public InventoryData PlayerInventory { get; set; }

    [Export]
    public EquipmentInventoryData EquipmentInventory { get; set; }

    public PlayerResource()
    {
        this.Life = 100;
        this.MoveSpeed = 100f;
        this.DashSpeed = 3f;
        this.PlayerInventory = new InventoryData(27);
        this.EquipmentInventory = new EquipmentInventoryData(); //TODO Arreglar contructor
    }
}
