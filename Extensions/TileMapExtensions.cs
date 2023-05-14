using Godot;

/// <summary>
/// Clase para extensiones de TileMap
/// </summary>
public static class TileMapExtensions
{
    /// <summary>
    /// Inicializa nodos que puede contener un TileMap
    /// </summary>
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

    /// <summary>
    /// Finaliza nodos que puede contener un TileMap
    /// </summary>

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
