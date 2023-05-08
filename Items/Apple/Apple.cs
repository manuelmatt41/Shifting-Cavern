using Godot;
using System;

public partial class Apple : ItemData
{
    public Apple()
    {
        this.Texture = ResourceLoader.Load<AtlasTexture>("res://Items/Apple/AppleTexture.tres");
        this.Name = this.Tr($"{nameof(Apple).ToUpper()}_{nameof(this.Name).ToUpper()}");
        this.Description = this.Tr($"{nameof(Apple).ToUpper()}_{nameof(this.Description).ToUpper()}");
        this.IsStackable = true;
    }
}
