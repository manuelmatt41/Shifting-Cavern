using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector2(
            Input.GetActionStrength("Right") - Input.GetActionStrength("Left"),
            Input.GetActionStrength("Down") - Input.GetActionStrength("Up")
        );

        GD.Print(inputDirection);
    }
}
