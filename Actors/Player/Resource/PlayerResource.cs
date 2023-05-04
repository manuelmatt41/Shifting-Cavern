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
    public InventoryData InventoryData { get; set; }

    [Export]
    public EquipmentInventoryData EquipmentInventoryData { get; set; }

    public PlayerResource()
        : this(100, 100, 3, new InventoryData(27), new EquipmentInventoryData()) { }

    public PlayerResource(
        double life,
        float moveSpeed,
        float dashSpeed,
        InventoryData inventoryData,
        EquipmentInventoryData equipmentInventoryData
    )
    {
        this.Life = life;
        this.MoveSpeed = moveSpeed;
        this.DashSpeed = dashSpeed;
        this.InventoryData = inventoryData;
        this.EquipmentInventoryData = equipmentInventoryData;
    }
}
