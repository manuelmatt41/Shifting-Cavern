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
    public delegate void ChangeDirectionEventHandler(Vector2 newDirection);

    /// <summary>
    /// Tiempo que tarda en mandar una nueva direccion
    /// </summary>
    [Export]
    public double ChangeDirectionTime { get; private set; } = 5;

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
    private bool _isPlayerInside { get; set; } = false;

    /// <summary>
    /// Contador del tiempo para volver a mandar una direccion
    /// </summary>
    private double _changeDirectionTimeCount = 0;

    /// <summary>
    /// Genera numeros aleatorios
    /// </summary>
    private RandomNumberGenerator _randomNumberGenerator = new();

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
        if (this._isPlayerInside)
        {
            this.SendPlayerDirection();
            return;
        }

        this._changeDirectionTimeCount += delta;

        if (this._changeDirectionTimeCount >= ChangeDirectionTime)
        {
            this.SendNewDirection();
        }
    }

    /// <summary>
    /// lanza el evento <c>ChangeDirection</c> con una posicion aleatoria dentro de <c>WardArea</c>
    /// </summary>
    private void SendNewDirection()
    {
        var halfWidth = (this.AreaSize.X / 2);
        var halfHeight = (this.AreaSize.Y / 2);

        var minX = (int)(this.GlobalPosition.X - halfWidth);
        var minY = (int)(this.GlobalPosition.Y - halfHeight);
        var maxX = (int)(this.GlobalPosition.X + halfWidth);
        var maxY = (int)(this.GlobalPosition.Y + halfHeight);

        this.EmitSignal(
            SignalName.ChangeDirection,
            new Vector2(this._randomNumberGenerator.RandiRange(minX, maxX), this._randomNumberGenerator.RandiRange(minY, maxY))
        );

        this._changeDirectionTimeCount = 0;
    }

    /// <summary>
    /// lanza el evento <c>ChangeDirection</c> con la posicion actual de <c>Player</c>
    /// </summary>
    private void SendPlayerDirection() =>
        this.EmitSignal(SignalName.ChangeDirection, this.Player.GlobalPosition);

    /// <summary>
    /// Evento que se ejecuta cuando un cuerpo entra en contacto con <c>WardArea</c>, solo detecta a <c>Player</c>
    /// </summary>
    /// <param name="body">Cuerpo que ha entrado en <c>WardArea</c></param>
    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            if (this.Player == null)
            {
                this.SetDeferred("Player", body as Player);
            }

            this.SetDeferred("_isPlayerInside", true);
        }
    }

    /// <summary>
    /// Evento que se ejecuta cuando un cuerpo sale del contacto de <c>WardArea</c>, solo detecta a <c>Player</c>
    /// </summary>
    /// <param name="body">Cuerpo que ha salido de <c>WardArea</c></param>
    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            this.SetDeferred("_isPlayerInside", false);
        }
    }
}
