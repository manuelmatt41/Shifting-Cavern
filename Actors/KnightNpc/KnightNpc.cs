using Godot;
using System;

public partial class KnightNpc : CharacterBody2D
{
    [Export]
    public double Speed { get; private set; }
}
