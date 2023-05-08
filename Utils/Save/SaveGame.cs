using Godot;
using Godot.Collections;

public partial class SaveGame : Resource
{
    public const string SAVE_GAME_PATH = "res://Utils/Save/SaveGame.tres";

    [Export]
    public PlayerResource PlayerResource { get; set; }

    [Export]
    public Vector2 PlayerGlobalPosition { get; set; }

    [Export]
    public Dictionary<string, ChestResource> ChestResources { get; set; }

    [Export]
    public TileMap CurrentMap { get; set; }

    public SaveGame()
    {
        this.PlayerResource = new();
        this.PlayerGlobalPosition = Vector2.Zero;
        this.ChestResources = new();
        this.CurrentMap = GD.Load<PackedScene>("res://Levels/test_map.tscn").Instantiate<TileMap>();
    }

    public void Save() => ResourceSaver.Save(this, SAVE_GAME_PATH);

    public static SaveGame LoadGame()
    {
        if (ResourceLoader.Exists(SAVE_GAME_PATH))
        {
            return GD.Load<SaveGame>(SAVE_GAME_PATH);
        }

        return new();
    }
}
