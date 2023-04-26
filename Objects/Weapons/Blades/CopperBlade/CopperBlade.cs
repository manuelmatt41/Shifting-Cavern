using Godot;
using System;

public partial class CopperBlade : Node2D, IWeapon
{
    [Export]
    public Vector2 Range { get; set; } = new(100, 48);

    [Export]
    public double Damage { get; set; } = 10;

    [Export]
    public float AttackSpeed { get; set; } = 0.1f;
}
