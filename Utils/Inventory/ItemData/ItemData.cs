using Godot;
using MonoCustomResourceRegistry;

/// <summary>
/// Clase que representa la informacion de un Item
/// </summary>
[RegisteredType(nameof(ItemData), "", nameof(Resource))]
public partial class ItemData : Resource
{
    /// <summary>
    /// Nombre del item
    /// </summary>
    [Export]
    public string Name { get; set; }

    /// <summary>
    /// Descripcion de un item
    /// </summary>
    [Export]
    public string Description { get; set; }

    /// <summary>
    /// Textura que representa al item
    /// </summary>
    [Export]
    public AtlasTexture Texture { get; set; }

    /// <summary>
    /// Valor para declarar la forma en que se puede guardar el item en los inventarios
    /// </summary>
    [Export]
    public bool IsStackable { get; set; }

    public ItemData()
    {
        this.Name = "";
        this.Description = "";
        this.Texture = null;
        this.IsStackable = true;
    }

    public static bool operator ==(ItemData a, ItemData b) => a.Name == b.Name;
    public static bool operator !=(ItemData a, ItemData b) => a.Name != b.Name;
}
