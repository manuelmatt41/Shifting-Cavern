using Godot;

public partial class GameLevel : Node2D
{
    public Player Player { get; set; }
    //public Goblin Goblin { get; set; }
    //public Goblin Goblin2 { get; set; }
    public InventoryControl InventoryControl { get; set; }

    public readonly PackedScene PickUpItem = GD.Load<PackedScene>(
        "res://Utils/PickUpItem/pick_up_item.tscn"
    );

    public override void _Ready()
    {
        this.Player = this.GetNode<Player>("Player");
        //this.Goblin = this.GetNode<Goblin>("Goblin");
        //this.Goblin2 = this.GetNode<Goblin>("Goblin2");
        this.InventoryControl = this.GetNode<CanvasLayer>("UI")
            .GetNode<InventoryControl>("InventoryControl");

        this.Player.ToogleInventoryControl += this.OnToogleInventoryInterface;
        //this.Goblin.DropLoot += this.OnDropLoot;
        //this.Goblin2.DropLoot += this.OnDropLoot;

        this.InventoryControl.SetPlayerInventoryData(this.Player.InventoryData);
    }

    private void OnToogleInventoryInterface()
    {
        this.InventoryControl.Visible = !this.InventoryControl.Visible;
        this.Player.IsInventoryVisible = this.InventoryControl.Visible;
        //Input.MouseMode = this.InventoryControl.Visible
        //    ? Input.MouseModeEnum.Visible
        //    : Input.MouseModeEnum.Hidden;
    }

    private void OnInventoryControlDropSlotData(SlotData slotData)
    {
        var pickUpItem = this.PickUpItem.Instantiate<PickUpItem>();

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
}
