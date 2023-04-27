using Godot;
using MonoCustomResourceRegistry;


[RegisteredType(nameof(ItemData), "", nameof(Resource))]

public partial class ItemData : Resource
{
    [Export]
    public string Name { get; set; }

    [Export]
    public string Description { get; set; }

    [Export]
    public AtlasTexture Texture { get; set; }

    public ItemData()
    {
        this.Name = "";
        this.Description = "";
        this.Texture = null;
    }
}
