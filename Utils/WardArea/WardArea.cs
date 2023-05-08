using Godot;

/// <summary>
/// Clase que recrea un area para detectar cuerpos al entrar o coger una posicion aleatoria dentro del area
/// </summary>
public partial class WardArea : Area2D
{
    /// <summary>
    /// Evento que se lanza cada cierto tiempo para mandar una nueva posicion
    /// </summary>
    /// <param name="newDirection">Nueva posicion</param>
    [Signal]
    public delegate void DetectPlayerEventHandler(Vector2 playerDirection);

    /// <summary>
    /// Forma de la colision de <c>WardArea</c>
    /// </summary>
    public CollisionShape2D CollisionShape2D { get; private set; }

    /// <summary>
    /// Tamanyo del area de <c>CollisionShape2D</c>
    /// </summary>
    public Vector2 AreaSize
    {
        get => this.CollisionShape2D.Shape.GetRect().Size;
    }

    /// <summary>
    /// Referencia a <c>Player</c>
    /// </summary>
    public Player Player { get; set; }

    /// <summary>
    /// Valor que dice si el jugador esta dentro del area (<c>true</c>) o no (<c>false</c>
    /// </summary>
    public bool IsPlayerInside { get; set; } = false;

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta en el editor para avisar de posibles problemas
    /// </summary>
    /// <returns>La cadena explicando el problema</returns>
    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta cada frame mandando la nueva direccion ya sea del juagdor si esta dentro de area o una aleatoria cuando se haya alancazado el temporizar el tiempo determinado
    /// </summary>
    /// <param name="delta">Tiempo entre frames</param>
    /// <returns>La cadena explicando el problema</returns>
    public override void _Process(double delta)
    {
        if (this.IsPlayerInside)
        {
            this.SendPlayerDirection();
        }
    }

    /// <summary>
    /// lanza el evento <c>ChangeDirection</c> con la posicion actual de <c>Player</c>
    /// </summary>
    private void SendPlayerDirection() =>
        this.EmitSignal(SignalName.DetectPlayer, this.Player.GlobalPosition);

    /// <summary>
    /// Evento que se ejecuta cuando un cuerpo entra en contacto con <c>WardArea</c>, solo detecta a <c>Player</c>
    /// </summary>
    /// <param name="body">Cuerpo que ha entrado en <c>WardArea</c></param>
    private void OnBodyEntered(Node2D body)
    {
        if (body.IsPlayer())
        {
            if (this.Player == null)
            {
                this.SetDeferred("Player", body as Player);
            }

            this.SetDeferred("IsPlayerInside", true);
        }
    }

    /// <summary>
    /// Evento que se ejecuta cuando un cuerpo sale del contacto de <c>WardArea</c>, solo detecta a <c>Player</c>
    /// </summary>
    /// <param name="body">Cuerpo que ha salido de <c>WardArea</c></param>
    private void OnBodyExited(Node2D body)
    {
        if (body.IsPlayer())
        {
            this.SetDeferred("IsPlayerInside", false);
        }
    }
}
