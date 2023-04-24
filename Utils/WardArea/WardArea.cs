using Godot;
using System;

public partial class WardArea : Area2D
{
    [Signal]
    public delegate void ChangeDirectionEventHandler(Vector2 newDirection);

    [Signal]
    public delegate void DetectPlayerEventHandler(Vector2 playerPosition);
    public CollisionShape2D CollisionShape2D { get; set; }

    public Random Rand { get; set; } = new Random();

    public Timer WardTimer { get; set; }

    public Vector2 AreaSize
    {
        get => this.CollisionShape2D.Shape.GetRect().Size;
    }

    public override void _Ready()
    {
        this.CollisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.WardTimer = this.GetNode<Timer>("WardTimer");
    }

    private void OnWardTimerTimeout()
    {
        this.EmitSignal(
            SignalName.ChangeDirection,
            new Vector2(
                this.Rand.Next(0, (int)this.AreaSize.X),
                this.Rand.Next(0, (int)this.AreaSize.Y)
            )
        );
    }

    private void OnAreaEntered(Area2D area)
    {
        GD.Print("Player detected");
        if (area.GetParent().IsInGroup("Player"))
        {
            this.WardTimer.Stop();
            this.EmitSignal(SignalName.DetectPlayer, area.GlobalPosition);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area.GetParent().IsInGroup("Player"))
        {
            this.WardTimer.Start();
            this.EmitSignal(SignalName.DetectPlayer, null);
        }
    }
}
