using Godot;
using System;

public partial class ChangeMapArea : Area2D
{
    [Signal]
    public delegate void ChangeMapEventHandler(
        TileMap newMap,
        Vector2 spawnPosition,
        Vector2I mapSize
    );

    [Export]
    public string SpawnPointName { get; set; }

    [Export]
    public string NewMapPath { get; set; }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            var newMap = GD.Load<PackedScene>(this.NewMapPath).Instantiate<TileMap>();
            this.EmitSignal(
                SignalName.ChangeMap,
                newMap,
                newMap.GetNode<Node2D>($"{SpawnPointName}SpawnPoint").GlobalPosition,
                newMap.GetUsedRect().Size * newMap.CellQuadrantSize
            );
        }
    }
}
