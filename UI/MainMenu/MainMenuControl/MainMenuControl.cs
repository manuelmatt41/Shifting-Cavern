using Godot;
using System;

public partial class MainMenuControl : Control
{
    [Signal]
    public delegate void NewGameEventHandler();
    public Button NewGameButton { get; set; }

    public override void _Ready()
    {
        var buttonsContainer = this.GetNode<VBoxContainer>("ButtonsContainer");

        this.NewGameButton = buttonsContainer.GetNode<Button>(nameof(this.NewGameButton));
    }

    private void OnNewGameButtonPressed()
    {
        this.EmitSignal(SignalName.NewGame);
    }
}
