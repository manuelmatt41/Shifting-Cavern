using Godot;

public static class TileMapExtensions
{
    public static void StartMapComponents(this TileMap tileMap)
    {
        foreach (var child in tileMap.GetChildren())
        {
            if (child is EnemySpawner enemySpawner)
            {
                enemySpawner.Start();
            }
        }
    }

    public static void StopMapComponents(this TileMap tileMap)
    {
        foreach (var child in tileMap.GetChildren())
        {
            if (child is EnemySpawner enemySpawner)
            {
                enemySpawner.Stop();
            }
        }
    }
}
