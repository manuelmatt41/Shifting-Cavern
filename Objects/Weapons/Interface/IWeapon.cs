using Godot;

public interface IWeapon
{
    public Vector2 Range { get; set; }
    public double Damage { get; set; }
    public float AttackSpeed { get; set; }
}
