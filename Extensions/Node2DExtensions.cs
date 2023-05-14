using Godot;

/// <summary>
/// Clase para extensiones para la clase <c>Node</c>
/// </summary>
public static class NodeExtensions
{
    /// <summary>
    /// Comprueba que el nodo este en el grupo Player
    /// </summary>
    /// <returns><c>true</c> si esta en el grupo Player sino <c>false</c></returns>
    public static bool IsPlayer(this Node node) => node.IsInGroup("Player");
}
