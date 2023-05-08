using Godot;
using System;

public partial class Coin : ItemData
{
    public Coin()
    {
        this.Texture = ResourceLoader.Load<AtlasTexture>("res://Items/Coin/CoinTexture.tres");
        this.Name = this.Tr($"{nameof(Coin).ToUpper()}_{nameof(this.Name).ToUpper()}");
        this.Description = this.Tr($"{nameof(Coin).ToUpper()}_{nameof(this.Description).ToUpper()}");
        this.IsStackable = true;
    }
}
