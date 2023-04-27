using Godot;
using System;
using System.Linq;

public partial class InventoryUI : PanelContainer
{
    public GridContainer ItemGrid { get; private set; }

    private readonly PackedScene _slot = GD.Load<PackedScene>("res://Utils/Inventory/SlotUi/Slot.tscn");


    public override void _Ready()
    {
        this.ItemGrid = this.GetNode<MarginContainer>("MarginContainer").GetNode<GridContainer>("ItemGrid");
        var inventoryTest = GD.Load<InventoryData>("res://Utils/Inventory/InventoryData/testInv.tres");
        this.PopulateItemGrid(inventoryTest.SlotDatas);
    }

    private void PopulateItemGrid(SlotData[] slotDatas)
    {
        Array.ForEach(this.ItemGrid.GetChildren().ToArray(), children => children.QueueFree());

        foreach (var slotData in slotDatas)
        {
            var slot = this._slot.Instantiate() as Slot;
            this.ItemGrid.AddChild(slot);

            if (slotData != null)
            {
                slot.SetSlotData(slotData);
            }
        }
    }
}
