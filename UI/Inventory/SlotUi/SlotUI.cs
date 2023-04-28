using Godot;
using System;

public partial class SlotUI : PanelContainer
{
    [Signal]
    public delegate void SlotClickedEventHandler(int index, int mouseButtonIndex);

    public TextureRect TextureRect { get; private set; }
    public Label QuantityLabel { get; private set; }

    public override void _Ready()
    {
        this.TextureRect = this.GetNode<MarginContainer>("MarginContainer")
            .GetNode<TextureRect>("TextureRect");
        this.QuantityLabel = this.GetNode<Label>("QuantityLabel");
    }

    public void SetSlotData(SlotData slotData)
    {
        var itemData = slotData.ItemData;

        this.TextureRect.Texture = itemData.Texture;
        this.TooltipText = $"{itemData.Name}\n{itemData.Description}";

        if (slotData.Quantity > 1)
        {
            this.QuantityLabel.Text = $"{slotData.Quantity}";
            this.QuantityLabel.Show();
        }
        else
        {
            this.QuantityLabel.Hide();
        }
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (
            @event is InputEventMouseButton mouseEvent
            && (
                mouseEvent.ButtonIndex == MouseButton.Left
                || mouseEvent.ButtonIndex == MouseButton.Right
            )
            && mouseEvent.IsPressed()
        )
        {
            this.EmitSignal(SignalName.SlotClicked, this.GetIndex(), (int)mouseEvent.ButtonIndex); // El enumerado de MouseButton hereda de long
        }
    }
}
