using Godot;
using System;

public partial class SlotData : Resource
{
    public const int MAX_STACK_SIZE = 99;

    [Export]
    public ItemData ItemData { get; set; }

    [Export(PropertyHint.Range, $"1, 99")]
    public int Quantity { get; set; } = 1;

    public SlotData() : this(null, 1) { }

    public SlotData(ItemData itemData, int quantity)
    {
        this.ItemData = itemData;
        this.Quantity = quantity;
    }
}
