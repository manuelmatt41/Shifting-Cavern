using Godot;

[Tool]
public partial class HurtBox : Area2D
{
    /// <summary>
    /// Evento que se lanza al colisionar con otra area en grupo Attack
    /// </summary>
    /// <param name="damage">Valor que representa el danyo que ha causado el otro area</param>
    [Signal]
    public delegate void HurtEventHandler(double damage);

    /// <summary>
    /// Tipo de HurtBox
    /// </summary>
    /// <value>Por defecto: Cooldown</value>
    [Export]
    public HurtBoxType HurtBoxType { get; set; } = HurtBoxType.Cooldown;

    /// <summary>
    /// Forma de la colision que va actuar con el resto de areas
    /// </summary>
    public CollisionShape2D CollisionShape2D { get; set; }

    /// <summary>
    /// <c>Timer</c> que comprueba el tiempo entre detecciones de areas
    /// </summary>
    public Timer CooldownTimer { get; set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>HurtBox</c>
    /// </summary>
    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.CooldownTimer = this.GetNode<Timer>("CooldownTimer");
    }

    /// <summary>
    /// Evento que se ejecuta al detectar la entrada de otra <c>Area2D</c>
    /// </summary>
    /// <param name="area">Area que ha entrado</param>
    private void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("Attack"))
        {
            var damage = area.Get("Damage").AsDouble();

            if (damage > 0) //TODO Comporbar si se puede quitar esta condicion
            {
                switch (this.HurtBoxType)
                {
                    case HurtBoxType.Cooldown:
                        this.CollisionShape2D.CallDeferred("set_disabled", true);
                        this.CooldownTimer.Start();
                        break;
                    case HurtBoxType.DisableHitBox:
                        if (area.HasMethod("DisableHitBoxCollision"))
                        {
                            area.Call("DisableHitBoxCollision");
                        }
                        break;
                }

                this.EmitSignal(SignalName.Hurt, damage);
            }
        }
    }

    /// <summary>
    /// Evento que se ejecuta al terminar <c>cooldownTimer</c) para reactivar las colisiones
    /// </summary>
    private void OnCooldownTimeout()
    {
        this.CollisionShape2D.CallDeferred("set_disabled", false);
    }
}

/// <summary>
/// Tipos de HurtBox
/// </summary>
public enum HurtBoxType
{
    /// <summary>
    /// Al ser golpeado se desactiva la colision de <c>HurtBox</c>
    /// </summary>
    Cooldown,
    HitOnce,
    /// <summary>
    /// Al ser golpeado se desactiva la colision de <c>HitBox</c>
    /// </summary>
    DisableHitBox
}
