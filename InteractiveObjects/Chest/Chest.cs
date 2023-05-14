using Godot;

/// <summary>
/// Clase que representa un cofre en el juego
/// </summary>
public partial class Chest : RigidBody2D
{
    /// <summary>
    /// Evento qeue se lanza para abrir el inventario
    /// </summary>
    /// <param name="chestInventory">Informacion del inventario del cofre</param>
    [Signal]
    public delegate void OpenChestInventoryEventHandler(InventoryData chestInventory);

    /// <summary>
    /// Datos que se van a guardar del cofre
    /// </summary>
    [Export]
    public ChestResource ChestResource { get; set; }

    /// <summary>
    /// Tipo de cofre que generara el inventario dependiendo de este tipo
    /// </summary>
    [Export]
    public ChestLootType ChestLootType { get; set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Chest</c>
    /// </summary>
    public override void _Ready()
    {
        this.ChestResource = Main.SaveGame.ChestResources.ContainsKey(this.Name)
            ? Main.SaveGame.ChestResources[this.Name]
            : null;

        if (this.ChestResource == null)
        {
            switch (this.ChestLootType)
            {
                case ChestLootType.INITIAL:
                    this.ChestResource = ChestResource.SetInitialLootTableChest();
                    break;
                case ChestLootType.SECRET:
                    this.ChestResource = ChestResource.SetSecretLootTableChest();
                    break;
            }
        }
    }

    /// <summary>
    /// Funcion que se ejecuta al hacer click en el cofre
    /// </summary>
    /// <param name="viewport">Tamanio en pantalla del juego</param>
    /// <param name="event">Tipo de evento que ha ejecutado la funcion</param>
    /// <param name="shapeIdx"></param>
    private void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (
            @event is InputEventMouseButton mouseEvent
            && mouseEvent.ButtonIndex == MouseButton.Right
            && mouseEvent.IsPressed()
        )
        {
            this.EmitSignal(SignalName.OpenChestInventory, this.ChestResource.InventoryData);
        }
    }
}

/// <summary>
/// Tipos de cofre que generara el inventario y los items que contendra dentro
/// </summary>
public enum ChestLootType
{
    INITIAL,
    SECRET
}
