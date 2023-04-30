using Godot;
using System;

public partial class Chest : RigidBody2D
{
    private void OnMouseEntered()
    {
        this.Hide();
    }

    private void OnMouseExited()
    {
        this.Show();
    }
}
