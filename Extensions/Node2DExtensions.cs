using Godot;

public static class NodeExtensions
{
    public static bool IsPlayer(this Node node) => node.IsInGroup("Player");
}
