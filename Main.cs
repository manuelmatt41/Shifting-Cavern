using Godot;

public partial class Main : Node2D
{
    public readonly PackedScene PickUpItem = GD.Load<PackedScene>(
        "res://Utils/PickUpItem/pick_up_item.tscn"
    );

    public readonly PackedScene PlayerScene = GD.Load<PackedScene>(
        "res://Actors/Player/player.tscn"
    );

    public readonly PackedScene MainMenuControlUiScene = GD.Load<PackedScene>(
        "res://UI/MainMenu/main_menu_control_ui.tscn"
    );

    public readonly PackedScene InventoryControlUIScene = GD.Load<PackedScene>(
        "res://UI/Inventory/inventory_control_ui.tscn"
    );

    public readonly PackedScene MapContainerScene = GD.Load<PackedScene>(
        "res://Utils/MapContainer/map_container.tscn"
    );

    public Player Player { get; private set; }
    public CanvasLayer MainMenuUI { get; private set; }
    public MainMenuControl MainMenuControl { get; private set; }

    public CanvasLayer InventoryControlUI { get; private set; }

    public InventoryControl InventoryControl { get; private set; }

    public MapContainer MapContainer { get; private set; }

    public static SaveGame SaveGame = SaveGame.LoadGame() as SaveGame;

    public override void _Ready()
    {
        this.MainMenuUI = this.MainMenuControlUiScene.Instantiate<CanvasLayer>();

        this.MainMenuControl = this.MainMenuUI.GetNode<MainMenuControl>(
            nameof(this.MainMenuControl)
        );
        this.MainMenuControl.NewGame += this.OnNewGame;

        this.AddChild(this.MainMenuUI);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionPressed("CloseGame"))
        {
            Main.SaveGame.PlayerResource = this.Player.PlayerResource;
            Main.SaveGame.PlayerGlobalPosition = this.Player.GlobalPosition;

            foreach (var map in this.MapContainer.GetChildren())
            {
                if (map is TileMap)
                {
                    foreach (var tileMapChild in map.GetChildren())
                    {
                        if (tileMapChild is Chest chest)
                        {
                            Main.SaveGame.ChestResources[chest.Name] = chest.ChestResource;
                        }
                    }
                }
            }

            Main.SaveGame.CurrentMap = this.MapContainer.CurrentMap;

            Main.SaveGame.Save();
            this.GetTree().Quit();
        }
    }

    private void OnPlayerDamageChange(double newDamage)
    {
        this.Player.HitBox.Damage = newDamage;
    }

    private void OnChangePlayerPosition(
        Vector2 newPlayerPosition,
        Vector2I cameraLimitsStart,
        Vector2I cameraLimitsEnd
    )
    {
        this.Player.GlobalPosition = newPlayerPosition;
        this.Player.SetCameraLimits(cameraLimitsStart, cameraLimitsEnd);
    }

    private void OnNewGame()
    {
        this.Player = this.PlayerScene.Instantiate<Player>();
        this.InventoryControlUI = this.InventoryControlUIScene.Instantiate<CanvasLayer>();
        this.MapContainer = this.MapContainerScene.Instantiate<MapContainer>();

        this.AddChild(this.InventoryControlUI);
        this.AddChild(this.MapContainer);
        this.MapContainer.AddChild(this.Player);

        this.Player.Initialize(Main.SaveGame.PlayerResource);

        this.InventoryControl = this.InventoryControlUI.GetNode<InventoryControl>(
            nameof(this.InventoryControl)
        );
        this.InventoryControl.ToogleInventoryControl += this.OnToogleInventoryInterface;
        this.InventoryControl.DropSlotData += this.OnInventoryControlDropSlotData;
        this.InventoryControl.PlayerDamageChange += this.OnPlayerDamageChange;
        this.InventoryControl.SetPlayerInventoryData(this.Player.PlayerResource.InventoryData);
        this.InventoryControl.SetPlayerEquipmentInventoryData(
            this.Player.PlayerResource.EquipmentInventoryData
        );

        this.MapContainer.ChangePlayerPosition += this.OnChangePlayerPosition;
        this.MapContainer.ViewExternalInventory += this.OnViewExternalInventory;
        this.MapContainer.OnChangeChangeMap(
            Main.SaveGame.CurrentMap,
            Main.SaveGame.PlayerGlobalPosition
        );

        this.MainMenuUI.QueueFree();
    }

    private void OnToogleInventoryInterface()
    {
        this.InventoryControl.Visible = !this.InventoryControl.Visible;
        this.InventoryControl.ExternalInvetoryUI.Visible = false;
        this.InventoryControl.PlayerInventory.AnchorsPreset = 8;
        this.Player.IsInventoryVisible = this.InventoryControl.Visible;
        this.InventoryControl.ClearExternalInventoryData();
    }

    private void OnInventoryControlDropSlotData(SlotData slotData)
    {
        var pickUpItem = this.PickUpItem.Instantiate<PickUpItem>();

        pickUpItem.SlotData = slotData;
        pickUpItem.Position = new Vector2(
            this.Player.GlobalPosition.X + 20,
            this.Player.GlobalPosition.Y + 10
        );
        this.MapContainer.CallDeferred("add_child", pickUpItem);
    }

    private void OnViewExternalInventory(InventoryData inventoryData)
    {
        this.InventoryControl.Visible = true;
        this.InventoryControl.SetExternalInventoryData(inventoryData);
        this.InventoryControl.ExternalInvetoryUI.Visible = true;

        this.InventoryControl.PlayerInventory.AnchorsPreset = 4;
        this.InventoryControl.PlayerInventory.Position = new(
            75,
            this.InventoryControl.PlayerInventory.Position.Y
        );

        this.InventoryControl.ExternalInvetoryUI.AnchorsPreset = 6;
        this.InventoryControl.ExternalInvetoryUI.Position = new(
            this.InventoryControl.ExternalInvetoryUI.Position.X - 75,
            this.InventoryControl.ExternalInvetoryUI.Position.Y
        );

        this.Player.IsInventoryVisible = true;
        this.GetTree().Paused = true;
    }
}
