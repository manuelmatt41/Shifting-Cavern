using Godot;
using System;

public partial class LifeBarControl : Control
{
    public TextureProgressBar TextureProgressBar { get; private set; }

    public override void _Ready()
    {
        this.TextureProgressBar = this.GetNode<TextureProgressBar>(nameof(this.TextureProgressBar));
    }
}
