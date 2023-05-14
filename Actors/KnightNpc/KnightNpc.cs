using Godot;

/// <summary>
/// Representa a la prueba de un Npc
/// </summary>
public partial class KnightNpc : CharacterBody2D
{
    [Export]
    public double Speed { get; private set; }
}
