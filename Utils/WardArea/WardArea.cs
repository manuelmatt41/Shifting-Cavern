using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WardArea : Area2D
{
    [Signal]
    public delegate void ChangeDirectionEventHandler(Vector2 newDirection);

    public CollisionShape2D CollisionShape2D { get; set; }

    [Export]
    public const double CHANGE_DIRECTION_TIME = 5;
    public double ChangeDirectionTimeCount { get; set; } = 0;

    public Random Rand { get; set; } = new Random();

    public Vector2 AreaSize
    {
        get => this.CollisionShape2D.Shape.GetRect().Size;
    }
    public bool IsPlayerInside { get; set; } = false;

    public Player Player { get; set; }

    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _Process(double delta)
    {
        if (this.IsPlayerInside)
        {
            this.SendPlayerDirection();
            return;
        }

        this.ChangeDirectionTimeCount += delta;

        if (this.ChangeDirectionTimeCount >= CHANGE_DIRECTION_TIME)
        {
            this.SendNewDirection();
            this.ChangeDirectionTimeCount = 0;
        }
    }

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
            new Vector2(this.Rand.Next(minX, maxX), this.Rand.Next(minY, maxY))
        );
    }

    private void SendPlayerDirection() =>
        this.EmitSignal(SignalName.ChangeDirection, this.Player.GlobalPosition);

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            this.SetDeferred("IsPlayerInside", true);
            this.SetDeferred("Player", body as Player);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            this.SetDeferred("IsPlayerInside", false);
        }
    }
}
