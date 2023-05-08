using Godot;

/// <summary>
/// Clase que representa al item en le juego
/// </summary>
public partial class PickUpItem : Area2D
{
    /// <summary>
    /// Informacion que contiene el item y la cantidad que tiene
    /// </summary>
    [Export]
    public SlotData SlotData { get; set; }

    /// <summary>
    /// Imagen que representa al item
    /// </summary>
    public Sprite2D Sprite2D { get; set; }

    /// <summary>
    /// Forma de la colision en el juego del item
    /// </summary>
    public CollisionShape2D CollisionShape2D { get; set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>PickUpItem</c>
    /// </summary>
    public override void _Ready()
    {
        this.Sprite2D = this.GetNode<Sprite2D>("Sprite2D");
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");

        this.Sprite2D.Texture = this.SlotData.ItemData.Texture;
        var box = this.CollisionShape2D.Shape.GetRect();
        box.Size = this.SlotData.ItemData.Texture.GetSize();
    }

    /// <summary>
    /// Funcion que se ejecuta al detectar un Node2D ha entrado dentro del area del item, comprueba que sea un jugador y si es asi intenta introducir el item dentro del inventario del jugador y si lo consigue lo elimina sino lo deja como esta
    /// </summary>
    /// <param name="body"></param>
    private void OnBodyEntered(Node2D body)
    {
        if (body.IsPlayer())
        {
            var player = body as Player;

            if (player.PlayerResource.InventoryData.PickUpSlotData(this.SlotData))
            {
                this.QueueFree();
            }
        }
    }
}
