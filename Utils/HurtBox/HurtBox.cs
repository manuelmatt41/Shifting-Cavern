using Godot;
using System;

public partial class HurtBox : Area2D
{
    [Signal]
    public delegate void HurtEventHandler(double damage);

    [Export]
    public HurtBoxType HurtBoxType { get; set; }

    private CollisionShape2D collisionShape2D;

    private Timer cooldownTimer;

    public override void _Ready()
    {
        this.collisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.cooldownTimer = this.GetNode<Timer>("CooldownTimer");
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("Attack"))
        {
            var damage = area.Get("Damage").AsDouble();

            if (damage > 0)
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

    private void OnCooldownTimeout()
    {
        this.collisionShape2D.CallDeferred("set_disabled", false);
    }
}

public enum HurtBoxType
{
    Cooldown,
    HitOnce,
    DisableHitBox
}
