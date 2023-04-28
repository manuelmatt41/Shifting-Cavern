using Godot;

public partial class PickUpItem : Area2D
{
    [Export]
    public SlotData SlotData { get; set; }

    public Sprite2D Sprite2D { get; set; }

    public CollisionShape2D CollisionShape2D { get; set; }

    public override void _Ready()
    {
        this.Sprite2D = this.GetNode<Sprite2D>("Sprite2D");
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");

        this.Sprite2D.Texture = this.SlotData.ItemData.Texture;
        var box = this.CollisionShape2D.Shape.GetRect();
        box.Size = this.SlotData.ItemData.Texture.GetSize();
    }

    public override void _PhysicsProcess(double delta)
    {
        this.Sprite2D.Rotate((float)delta);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            var player = body as Player;

            if (player.InventoryData.PickUpSlotData(this.SlotData))
            {
                this.QueueFree();
            }
        }
    }
}
