using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(ChestResource), "", nameof(Resource))]

public partial class ChestResource : Resource
{
    [Export]
    public InventoryData InventoryData { get; set; }

    public ChestResource()
    {
        this.InventoryData = new InventoryData(27);
    }

    public static ChestResource SetInitialLootTableChest()
    {
        var chest = new ChestResource();
        chest.InventoryData = new InitialChest();

        return chest;
    }

    public static ChestResource SetSecretLootTableChest()
    {
        var chest = new ChestResource();
        chest.InventoryData = new SecretChest();

        return chest;
    }
}
