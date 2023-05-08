using Godot;

public static class Node2DExtensions
{
    public static bool IsPlayer(this Node2D node2D) => node2D.IsInGroup("Player");
}
