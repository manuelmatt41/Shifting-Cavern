using Godot;
using System;

public partial class MapContainer : Node2D
{
    [Signal]
    public delegate void ChangePlayerPositionEventHandler(Vector2 newPlayerPosition, Vector2I mapSize);
    public TileMap CurrentMap { get; set; }

    public void OnChangeChangeMap(TileMap newMap, Vector2 spawnPosition, Vector2I mapSize)
    {
        if (this.CurrentMap != null)
        {
            this.CurrentMap.QueueFree();
        }

        this.EmitSignal(SignalName.ChangePlayerPosition, spawnPosition, mapSize);

        foreach (var child in newMap.GetChildren())
        {
            if (child is ChangeMapArea changeMapArea)
            {
                changeMapArea.ChangeMap += this.OnChangeChangeMap;
            }
        }

        this.CurrentMap = newMap;
        this.CallDeferred("add_child", this.CurrentMap);
    }
}
