using Godot;
using System;

public partial class HitBox : Area2D
{
    [Export]
    public double Damage { get; set; } = 1;

    private CollisionShape2D collisionShape;

    private Timer disableTimer;

    public override void _Ready()
    {
        this.collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.disableTimer = this.GetNode<Timer>("DisableTimer");
    }

    public void TempDisable()
    {
        this.collisionShape.SetDeferred("disable", true);
        this.disableTimer.Start();
    }

    private void OnHitBoxTimerTimeOut()
    {
        this.collisionShape.SetDeferred("disable", false);
    }
}
