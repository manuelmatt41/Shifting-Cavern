using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(WeaponItemData), "", nameof(ItemData))]
public partial class WeaponItemData : ItemData
{
    [Export]
    public double Damage { get; set; }
}
