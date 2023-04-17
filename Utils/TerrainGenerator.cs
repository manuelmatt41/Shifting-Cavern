using Godot;
using Godot.Collections;
using System;

public partial class TerrainGenerator : Node
{
    public enum AvailableTiles
    {
        WATER = 11, DIRT = 1
    }
    [Export]
    public TileMap TileMap { get; set; }

    [Export]
    public int Width { get; set; }

    [Export]
    public int Height { get; set; }


    [Export]
    public FastNoiseLite.NoiseTypeEnum NoiseTypeEnum { get; set; } =
        FastNoiseLite.NoiseTypeEnum.Simplex;

    public override void _Ready()
    {
        FastNoiseLite p = new();
        p.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        p.Seed = Guid.NewGuid().GetHashCode();

        ValidateTileMap();
        GenerateRandomTerrain(p);
    }

    private void ValidateTileMap()
    {
        // Comprobamos que no sea null para poder ejecutar el programa
        if (TileMap == null)
        {
            throw new ArgumentNullException(nameof(TileMap));
        }

        // Borramos todas las celdas para reconstruir el mapa
        TileMap.Clear();
    }

    private void GenerateRandomTerrain(FastNoiseLite noiseGenerator)
    {
        GenerateLimiters();

        Vector2I cellCoord = new();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cellCoord.X = x;
                cellCoord.Y = y;
                TileMap.SetCell(0, cellCoord, TileMap.GetCellTileData(0, new Vector2I(1,11)).);
            }
        }
    }

    private int GetTile(float noiseLatitude)
    {
        switch (noiseLatitude)
        {
            case <= 0.0f:
                return (int)AvailableTiles.WATER;
            default:
                return (int)AvailableTiles.DIRT;
        }
    }

    private void GenerateLimiters()
    {
        SetParallelLinesLimiter(Width, Height);
        SetParallelLinesLimiter(Height, Width);
    }

    private void SetParallelLinesLimiter(int linesDistance, int lineSeparationDistance)
    {
        Vector2I cellCoord = new();

        for (int x = -1; x <= lineSeparationDistance; x += lineSeparationDistance)
        {
            for (int y = 0; y < linesDistance; y++)
            {
                cellCoord.X = x;
                cellCoord.Y = y;
                TileMap.SetCell(0, cellCoord, 6);
            }
        }
    }
}
