using Godot;

[Tool]
public partial class ChangeMapArea : Area2D
{
    [Signal]
    public delegate void ChangeMapEventHandler(TileMap map, Vector2 spawnPosition);

    [Export]
    public TileMap MapChange { get; set; }
    public Node2D SpawnPoint { get; private set; }

    public CollisionShape2D CollisionShape2D { get; private set; }

    public override void _Ready()
    {
        this.SpawnPoint = this.GetNode<Node2D>(nameof(this.SpawnPoint));
        this.CollisionShape2D = this.GetNode<CollisionShape2D>(nameof(this.CollisionShape2D));
    }
    private void OnBodyEntered(Node2D body)
    {
        if (body.IsPlayer())
        {
            this.EmitSignal(SignalName.ChangeMap, this.MapChange as TileMap, this.SpawnPoint.GlobalPosition);
        }
    }

    public override void _Draw()
    {
        if (Engine.IsEditorHint())
        {
            this.DrawDashedLine(this.CollisionShape2D.Position, this.SpawnPoint.Position, Color.Color8(255, 0, 0), -1, 10);
        }
    }
}
