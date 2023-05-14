using Godot;
using MonoCustomResourceRegistry;

/// <summary>
/// Clase que representa la informacion que se va a guardar de un cofre
/// </summary>
[RegisteredType(nameof(ChestResource), "", nameof(Resource))]
public partial class ChestResource : Resource
{
    /// <summary>
    /// Inventario del cofre
    /// </summary>
    [Export]
    public InventoryData InventoryData { get; set; }

    public ChestResource()
    {
        this.InventoryData = new InventoryData(27);
    }

    /// <summary>
    /// Inicia un cofre con un <c>InitialChest</c>
    /// </summary>
    /// <returns>El inventario que genera el <c>InitialChest</c></returns>
    public static ChestResource SetInitialLootTableChest()
    {
        var chest = new ChestResource();
        chest.InventoryData = new InitialChest();

        return chest;
    }

    /// <summary>
    /// Inicia un cofre con un <c>SecretChest</c>
    /// </summary>
    /// <returns>El inventario que genera el <c>SecretChest</c></returns>
    public static ChestResource SetSecretLootTableChest()
    {
        var chest = new ChestResource();
        chest.InventoryData = new SecretChest();

        return chest;
    }
}
