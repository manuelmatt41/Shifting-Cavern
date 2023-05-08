using Godot;
using System;

public partial class MagicSword : WeaponItemData
{
    public MagicSword()
    {
        this.Damage = 100;
        this.Texture = ResourceLoader.Load<AtlasTexture>("res://Items/MagicSword/MagicSwordTexture.tres");
        this.Name = this.Tr($"{nameof(MagicSword).ToUpper()}_{nameof(this.Name).ToUpper()}");
        this.Description = this.Tr($"{nameof(MagicSword).ToUpper()}_{nameof(this.Description).ToUpper()}");
        this.IsStackable = false;
    }
}
