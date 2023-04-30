using Godot;

/// <summary>
/// Clase que representa una caja de colisiones para danyar a otras entidades
/// </summary>
[Tool]
public partial class HitBox : Area2D
{
    /// <summary>
    /// Danyo que va realizar el area
    /// </summary>
    /// <value>Por defecto: 1</value>
    [Export]
    public double Damage { get; set; } = 1;

    /// <summary>
    /// Forma de la colision que va actuar con el resto de areas
    /// </summary>
    public CollisionShape2D CollisionShape2D { get; private set; }

    /// <summary>
    /// <c>Timer</c> que comprueba el tiempo que se va a desactivar las colisiones
    /// </summary>
    public Timer DisableTimer { get; private set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>HurtBox</c>
    /// </summary>
    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.DisableTimer = this.GetNode<Timer>("DisableTimer");
    }

    /// <summary>
    /// Desactiva las colisiones y activa <c>DisableTimer</c>
    /// </summary>
    public void DisableHitBoxCollision()
    {
        this.CollisionShape2D.CallDeferred("set_disabled", true);
        this.DisableTimer.Start();
    }

    /// <summary>
    /// Evento que se ejecuta al terminar <c>DisableTimer</c> para activar las colisiones
    /// </summary>
    private void OnHitBoxTimerTimeOut()
    {
        this.CollisionShape2D.CallDeferred("set_disabled", true);
    }
}
