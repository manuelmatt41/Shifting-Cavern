using Godot;

public partial class DiamonBlade : Node2D, IWeapon
{
    [Export]
    public Vector2 Range { get; set; } = new(50, 48);
    [Export]
    public double Damage { get; set; } = 40;

    [Export]
    public float AttackSpeed { get; set; } = 1f;
}
