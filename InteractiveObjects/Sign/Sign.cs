using Godot;

/// <summary>
/// Clase que representa una senial en el juego
/// </summary>
public partial class Sign : RigidBody2D
{
    /// <summary>
    /// Control que muestra un pequenio globo con texto al acercarse a la senial
    /// </summary>
    public BalloonTextControl BalloonTextControl { get; private set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>Sign</c>
    /// </summary>
    public override void _Ready()
    {
        this.BalloonTextControl = this.GetNode<BalloonTextControl>(nameof(this.BalloonTextControl));
    }

    /// <summary>
    /// Funcion que se ejecuta al entrar un <c>Node2D</c> dentro del area y ensenia el <c>BalloonTextControl</c>
    /// </summary>
    /// <param name="body">Nodo que ha entrado dentro del area</param>
    private void OnArea2DBodyEntered(Node2D body)
    {
        if (body.IsPlayer())
        {
            this.BalloonTextControl.Start();
        }
    }

    /// <summary>
    /// Funcion que se ejecuta al salir un <c>Node2D</c> dentro del area y oculta el <c>BalloonTextControl</c>
    /// </summary>
    /// <param name="body">Nodo que ha salido dentro del area</param>
    private void OnArea2DBodyExited(Node2D body)
    {
        if (body.IsPlayer())
        {
            this.BalloonTextControl.Reset();
        }
    }
}
