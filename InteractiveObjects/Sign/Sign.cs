using Godot;
using System;

public partial class Sign : RigidBody2D
{
    public BalloonTextControl BalloonTextControl { get; private set; }

    public override void _Ready()
    {
        this.BalloonTextControl = this.GetNode<BalloonTextControl>(nameof(this.BalloonTextControl));
    }
    private void OnArea2DBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            this.BalloonTextControl.Start();
        }
    }

    private void OnArea2DBodyExited(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            this.BalloonTextControl.Reset();
        }
    }
}
