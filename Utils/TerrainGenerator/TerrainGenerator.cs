using System;
using Godot;
using Godot.Collections;

public partial class TerrainGenerator : Node
{
    /// <summary>
    /// Anncho del mapa generado
    /// </summary>
    [Export]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Alto del mapa generado
    /// </summary>
    [Export]
    public int Height { get; set; } = 100;

    /// <summary>
    /// Tipo de ruido que genera la imagen de sonidos
    /// </summary>
    [Export]
    public FastNoiseLite.NoiseTypeEnum NoiseTypeEnum { get; set; } =
        FastNoiseLite.NoiseTypeEnum.Simplex;

    //Cambiar variable a un Singleton para no instaciar demasiadas veces la misma clase
    public FastNoiseLite Latitude;
    public FastNoiseLite Temperature;

    /// <summary>
    /// Mapa donde se va a generar el mapa y donde tiene los Tiles que se van a colocar
    /// </summary>
    public TileMap TileMap { get; set; }

    private Dictionary<AvailableTiles, Vector2I> tiles = new Dictionary<AvailableTiles, Vector2I>()
    {
        {AvailableTiles.WATER, new(1, 11)},
        {AvailableTiles.DIRT, new(1, 1)},
        {AvailableTiles.PATH, new(6, 0) }
    };

    /// <summary>
    /// Crea una semilla al ejecutar la clase y crea un mapa de forma aleatoria
    /// </summary>
    public override void _Ready()
    {
        TileMap = this.GetNode<TileMap>("TileMap");
        Latitude = new();
        Temperature = new();
        Latitude.NoiseType = this.NoiseTypeEnum;
        Temperature.NoiseType = this.NoiseTypeEnum;
        var seed = GenerateNewSeed();
        Latitude.Seed = seed;
        Temperature.Seed = seed;

        ValidateTileMap();
        GenerateRandomTerrain(Latitude);
    }

    /// <summary>
    /// Se ejecuta durante el juego y comprueba si se pulsa el esdpacio para generar un mapa nuevo
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        if (Input.GetActionStrength("GenerateTerrain") > 0)
        {
            ValidateTileMap();
            this.Latitude.Seed = GenerateNewSeed();
            GenerateRandomTerrain(Latitude);
        }
    }

    /// <summary>
    /// Valida si el TileMap existe y elimina cualquier Tile que pueda contener
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
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

    /// <summary>
    /// Genera el terreno del mapa de forma aleatoria por el mapa de ruidos
    /// </summary>
    /// <param name="noiseGenerator">Mapa de ruidos que va a generar el mapa</param>
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
                TileMap.SetCell(
                    0,
                    cellCoord,
                    0,
                    GetTile(noiseGenerator.GetNoise2D(x, y)
                ));
            }
        }

        //Random r = new(); //TODO Cambiarlo, no debe recorrer el mapa dos veces!!
        //for (int x = 0; x < Width; x++)
        //{
        //    for (int y = 0; y < Height; y++)
        //    {
        //        cellCoord.X = x;
        //        cellCoord.Y = y;

        //        if (r.Next(1, 11) == 2)
        //        {
        //            TileMap.SetCell(0, cellCoord, 0, new Vector2I(6, 7));
        //        }
        //    }
        //}
    }

    /// <summary>
    /// Comprueba la latitud obtenido en el mapa de ruidos y comprueba a que Tile le corresponde a dicha celda
    /// </summary>
    /// <param name="noiseLatitude">Latitud de determinada celda del mapa</param>
    /// <returns></returns>
    private Vector2I GetTile(float noiseLatitude)
    {
        switch (noiseLatitude)
        {
            case <= 0.0f:
                return this.tiles[AvailableTiles.WATER];
            case <= 0.5f:
                return this.tiles[AvailableTiles.DIRT];
            default:
                return this.tiles[AvailableTiles.PATH];
        }
    }

    /// <summary>
    /// Genera unos limites en los bordes del mapa colocando unos Tiles en especifico (tiene que ser cuadrado)
    /// </summary>
    private void GenerateLimiters()
    {
        SetParallelLinesLimiter(Width, Height);
        SetParallelLinesLimiter(Height, Width);
    }

    /// <summary>
    /// Genera unas lineas paralelas entre ellas que limitan el mapa colocando unos Tiles en especifico
    /// </summary>
    /// <param name="linesDistance">Distancia de las lineas</param>
    /// <param name="lineSeparationDistance">Distancia que hay entre las lineas</param>
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

    private int GenerateNewSeed()
    {
        return Guid.NewGuid().GetHashCode();
    }
}

public enum AvailableTiles // TODO  Mejorar la localizacion de Tiles
{
    WATER,
    DIRT,
    PATH
} // TODO Mejorar la documentacion y especificar unidades de medida
