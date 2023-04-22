using Godot;

public partial class HitBox : Area2D
{
    /// <summary>
    /// Danyo que va realizar el area
    /// </summary>
    [Export]
    public double Damage { get; set; } = 1;

    /// <summary>
    /// Forma de la colision que va actuar con el resto de areas
    /// </summary>
    public CollisionShape2D CollisionShape { get; set; }

    /// <summary>
    /// <c>Timer</c> que comprueba el tiempo que se va a desactivar las colisiones
    /// </summary>
    private Timer disableTimer;
    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>HurtBox</c>
    /// </summary>
    public override void _Ready()
    {
        this.CollisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.disableTimer = this.GetNode<Timer>("DisableTimer");
    }

    /// <summary>
    /// Desactiva las colisiones y activa <c>disableTimer</c>
    /// </summary>
    public void TempDisable()
    {
        this.CollisionShape.SetDeferred("disable", true);
        this.disableTimer.Start();
    }

    /// <summary>
    /// Evento que se ejecuta al terminar <c>disableTimer</c> para activar las colisiones
    /// </summary>
    private void OnHitBoxTimerTimeOut()
    {
        this.CollisionShape.SetDeferred("disable", false);
    }
}
