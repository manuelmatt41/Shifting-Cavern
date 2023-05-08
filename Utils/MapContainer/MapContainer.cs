using Godot;

public partial class MapContainer : Node2D
{
    public readonly PackedScene PickUpItem = GD.Load<PackedScene>(
        "res://Utils/PickUpItem/pick_up_item.tscn"
    );

    [Signal]
    public delegate void ChangePlayerPositionEventHandler(
        Vector2 newPlayerPosition,
        Vector2I cameraLimitsStart,
        Vector2I cameraLimitsEnd
    );

    [Signal]
    public delegate void ViewExternalInventoryEventHandler(InventoryData inventoryData);

    public TileMap CurrentMap { get; set; }

    public void OnChangeChangeMap(TileMap newMap, Vector2 spawnPosition)
    {
        this.DespawnEnemies();

        if (this.CurrentMap != null)
        {
            this.CurrentMap.StopMapComponents();
        }

        var mapSize = newMap.GetUsedRect().Size * newMap.CellQuadrantSize;

        this.EmitSignal(
            SignalName.ChangePlayerPosition,
            spawnPosition,
            newMap.GlobalPosition,
            mapSize + newMap.GlobalPosition
        );

        this.CurrentMap = newMap;

        this.CurrentMap.StartMapComponents();
    }

    public void DespawnEnemies()
    {
        foreach (var child in this.GetChildren())
        {
            if (child is CharacterBody2D && child is not Player)
            {
                child.QueueFree();
            }
        }
    }

    private void OnCreateEnemies(CharacterBody2D[] enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy is Goblin goblin)
            {
                goblin.DropLoot += this.OnDropLoot;
            }

            this.CallDeferred("add_child", enemy);
        }
    }

    private void OnOpenChest(InventoryData inventoryData) =>
        this.EmitSignal(SignalName.ViewExternalInventory, inventoryData);

    private void OnDropLoot(SlotData[] loot, Vector2 position)
    {
        foreach (var item in loot)
        {
            var itemPickUp = this.PickUpItem.Instantiate<PickUpItem>();

            itemPickUp.SlotData = item;
            itemPickUp.Position = position;

            this.CallDeferred("add_child", itemPickUp);
        }
    }
}
