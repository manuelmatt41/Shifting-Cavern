using Godot;

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
    [Export]
    public HurtBoxType HurtBoxType { get; set; }

    /// <summary>
    /// Forma de la colision que va actuar con el resto de areas
    /// </summary>
    private CollisionShape2D collisionShape2D;

    /// <summary>
    /// <c>Timer</c> que comprueba el tiempo entre detecciones de areas
    /// </summary>
    private Timer cooldownTimer;

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>HurtBox</c>
    /// </summary>
    public override void _Ready()
    {
        this.collisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.cooldownTimer = this.GetNode<Timer>("CooldownTimer");
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
                        this.collisionShape2D.CallDeferred("set_disabled", true);
                        this.cooldownTimer.Start();
                        break;
                    case HurtBoxType.DisableHitBox:
                        if (area.HasMethod("TempDisable")) { }
                        break;
                }

                this.EmitSignal(SignalName.Hurt, damage);
            }
        }
    }

    /// <summary>
    /// Evento que se ejecuta al terminar el temporizador (<c>cooldownTimer</c>) para reactivar las colisiones
    /// </summary>
    private void OnCooldownTimeout()
    {
        this.collisionShape2D.CallDeferred("set_disabled", false);
    }
}

/// <summary>
/// Tipos de HurtBox
/// </summary>
public enum HurtBoxType
{
    Cooldown,
    HitOnce,
    DisableHitBox
}
