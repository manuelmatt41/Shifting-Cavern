using Godot;
using System;

public partial class Slot : PanelContainer
{
    public TextureRect TextureRect { get; private set; }
    public Label QuantityLabel { get; private set; }

    public override void _Ready()
    {
        this.TextureRect = this.GetNode<MarginContainer>("MarginContainer").GetNode<TextureRect>("TextureRect");
        this.QuantityLabel = this.GetNode<Label>("QuantityLabel");
    }

    public void SetSlotData(SlotData slotData)
    {
        var itemData = slotData.ItemData;

        this.TextureRect.Texture = itemData.Texture;
        this.TooltipText = $"{itemData.Name}\n{itemData.Description}";

        if (slotData.Quantity > 1) {
            this.QuantityLabel.Text = $"{slotData.Quantity}";
            this.QuantityLabel.Show();
        }
    }
}
