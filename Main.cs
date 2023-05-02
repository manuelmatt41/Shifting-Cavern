using Godot;

public partial class Main : Node2D
{
    public Player Player { get; set; }

    public Goblin Goblin { get; set; }
    //public Goblin Goblin2 { get; set; }
    public InventoryControl InventoryControl { get; set; }

    public Chest Chest { get; set; }
    public Chest Chest2 { get; set; }

    public readonly PackedScene PickUpItem = GD.Load<PackedScene>(
        "res://Utils/PickUpItem/pick_up_item.tscn"
    );

    public override void _Ready()
    {
        this.Player = this.GetNode<Player>("Player");
        this.Goblin = this.GetNode<Goblin>("Goblin");
        //this.Goblin2 = this.GetNode<Goblin>("Goblin2");
        this.Chest = this.GetNode<Chest>("Chest");
        this.Chest2 = this.GetNode<Chest>("Chest2");
        this.InventoryControl = this.GetNode<CanvasLayer>("UI")
            .GetNode<InventoryControl>("InventoryControl");

        this.Player.ToogleInventoryControl += this.OnToogleInventoryInterface;
        this.Goblin.DropLoot += this.OnDropLoot;
        //this.Goblin2.DropLoot += this.OnDropLoot;

        this.InventoryControl.SetPlayerInventoryData(this.Player.InventoryData);
        this.InventoryControl.SetPlayerEquipmentInventoryData(this.Player.EquipmentInventoryData);
        this.Chest.OpenChestInventory += this.OnOpenChestInventory;
        this.Chest2.OpenChestInventory += this.OnOpenChestInventory;
    }

    private void OnToogleInventoryInterface()
    {
        this.InventoryControl.Visible = !this.InventoryControl.Visible;
        this.InventoryControl.ExternalInvetoryUI.Visible = false;
        this.InventoryControl.PlayerInventory.AnchorsPreset = 8;
        this.Player.IsInventoryVisible = this.InventoryControl.Visible;
    }

    private void OnInventoryControlDropSlotData(SlotData slotData)
    {
        var pickUpItem = this.PickUpItem.Instantiate<PickUpItem>();
        GD.Print("aaa");
        pickUpItem.SlotData = slotData;
        pickUpItem.Position = new Vector2(
            this.Player.GlobalPosition.X + 20,
            this.Player.GlobalPosition.Y + 10
        );
        this.CallDeferred("add_child", pickUpItem);
    }

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

    private void OnOpenChestInventory(InventoryData inventoryData)
    {
        if (!this.InventoryControl.Visible)
        {
            this.InventoryControl.Visible = true;
        }

        this.InventoryControl.SetExternalInventoryData(inventoryData);
        this.InventoryControl.ExternalInvetoryUI.Visible = true;

        this.InventoryControl.PlayerInventory.AnchorsPreset = 4;
        this.InventoryControl.PlayerInventory.Position = new(
            75,
            this.InventoryControl.PlayerInventory.Position.Y
        );

        this.InventoryControl.ExternalInvetoryUI.AnchorsPreset = 6;
        this.InventoryControl.ExternalInvetoryUI.Position = new(
            this.InventoryControl.ExternalInvetoryUI.Position.X - 75,
            this.InventoryControl.ExternalInvetoryUI.Position.Y
        );

        this.Player.IsInventoryVisible = this.InventoryControl.Visible;
    }
}
