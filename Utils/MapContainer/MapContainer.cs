using Godot;
using System;

public partial class MapContainer : Node2D
{
    [Signal]
    public delegate void ChangePlayerPositionEventHandler(Vector2 newPlayerPosition, Vector2I mapSize);

    [Signal]
    public delegate void SpawnEnemiesEventHandler(CharacterBody2D enemy);
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

            if (child is EnemySpawner enemySpawner)
            {
                enemySpawner.CreateEnemies += this.OnCreateEnemies;
            }
        }

        this.CurrentMap = newMap;
        this.CallDeferred("add_child", this.CurrentMap);
    }

    private void OnCreateEnemies(CharacterBody2D[] enemies)
    {
        foreach (var enemy in enemies)
        {
            this.CurrentMap.CallDeferred("add_child", enemy);
        }
    }
}
