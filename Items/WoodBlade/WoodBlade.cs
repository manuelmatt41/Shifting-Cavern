using Godot;
using System;

public partial class WoodBlade : WeaponItemData
{
    public WoodBlade()
    {
        this.Damage = 25;
        this.Texture = ResourceLoader.Load<AtlasTexture>(
            "res://Items/WoodBlade/WoodBladeTexture.tres"
        );
        this.Name = this.Tr($"{nameof(WoodBlade).ToUpper()}_{nameof(this.Name).ToUpper()}");
        this.Description = this.Tr($"{nameof(WoodBlade).ToUpper()}_{nameof(this.Description).ToUpper()}");
        this.IsStackable = false;
    }
}
