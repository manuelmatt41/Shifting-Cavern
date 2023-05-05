using Godot;
using System;

public partial class MapContainer : Node2D
{
    [Signal]
    public delegate void ChangePlayerPositionEventHandler(
        Vector2 newPlayerPosition,
        Vector2I cameraLimitsStart,
        Vector2I cameraLimitsEnd
    );

    public TileMap CurrentMap { get; set; }

    public void OnChangeChangeMap(TileMap newMap, Vector2 spawnPosition)
    {
        var mapSize = newMap.GetUsedRect().Size * newMap.CellQuadrantSize;

        this.EmitSignal(
            SignalName.ChangePlayerPosition,
            spawnPosition,
            newMap.GlobalPosition,
            mapSize + newMap.GlobalPosition
        );

        this.CurrentMap = newMap;
    }

    private void OnCreateEnemies(CharacterBody2D[] enemies)
    {
        foreach (var enemy in enemies)
        {
            //this.CurrentMap.CallDeferred("add_child", enemy);
        }
    }
}
