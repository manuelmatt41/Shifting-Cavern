using Godot;
using System;

public partial class RottenFish : ItemData
{
    public RottenFish()
    {
        this.Texture = ResourceLoader.Load<AtlasTexture>("res://Items/RottenFish/RottenFishTexture.tres");
        this.Name = this.Tr($"{nameof(RottenFish).ToUpper()}_{nameof(this.Name).ToUpper()}");
        this.Description = this.Tr($"{nameof(RottenFish).ToUpper()}_{nameof(this.Description).ToUpper()}");
        this.IsStackable = true;
    }
}
