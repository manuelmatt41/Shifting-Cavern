using Godot;

public partial class ItemData : Resource
{
    [Export]
    public string Name { get; set; }

    [Export]
    public string Description { get; set; }

    [Export]
    public AtlasTexture Texture { get; set; }

    public ItemData() : this("", "", null) { }
    public ItemData(string name, string description, AtlasTexture texture)
    {
        this.Name = name;
        this.Description = description;
        this.Texture = texture;
    }

}
