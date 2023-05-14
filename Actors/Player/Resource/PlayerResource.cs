using Godot;
using MonoCustomResourceRegistry;

/// <summary>
/// Clase que representa los datos que se van a guardar del <c>Player</c> al cerrar el juego
/// </summary>
[RegisteredType(nameof(PlayerResource), "", nameof(Resource))]
public partial class PlayerResource : Resource
{
    /// <summary>
    /// Vida del <c>Player</c>
    /// </summary>
    [Export]
    public double Life { get; set; }

    /// <summary>
    /// Velocidad de movimiento del <c>Player</c> en px/s
    /// </summary>
    [Export]
    public float MoveSpeed { get; set; }

    /// <summary>
    /// Velocidad del movimiento dash del <c>Player</c>
    /// </summary>
    [Export]
    public float DashSpeed { get; set; }

    /// <summary>
    /// Informacion del inventario del <c>Player</c>
    /// </summary>
    [Export]
    public InventoryData InventoryData { get; set; }

    /// <summary>
    /// Informacion del inventario de equimpamiento del <c>Player</c>
    /// </summary>
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
