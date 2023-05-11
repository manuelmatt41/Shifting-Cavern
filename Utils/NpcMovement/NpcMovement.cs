using Godot;
using System;

public partial class NpcMovement : Node2D
{
    public CharacterBody2D Entity { get; private set; }

    public PathFollow2D PathFollow2D { get; private set; }

    public override void _Ready()
    {
        this.PathFollow2D = this.GetNode<Path2D>("Path2D").GetNode<PathFollow2D>("PathFollow2D");
        this.Entity = this.PathFollow2D.GetNode<CharacterBody2D>("KnightNpc");
    }

    public override void _PhysicsProcess(double delta)
    {
        this.PathFollow2D.Progress += (float)(100 * delta);
    }
}
