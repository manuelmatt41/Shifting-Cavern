using Godot;
using System;
using System.Linq;

public partial class InventoryUI : PanelContainer
{
    public GridContainer ItemGrid { get; private set; }

    private readonly PackedScene _slot = GD.Load<PackedScene>(
        "res://UI/Inventory/SlotUi/slot_ui.tscn"
    );

    public override void _Ready()
    {
        this.ItemGrid = this.GetNode<MarginContainer>("MarginContainer")
            .GetNode<GridContainer>("ItemGrid");
    }

    public void SetInventoryData(InventoryData inventoryData)
    {
        inventoryData.InventoryUpdate += this.PopulateItemGrid;
        this.PopulateItemGrid(inventoryData);
    }

    private void PopulateItemGrid(InventoryData inventoryData)
    {
        Array.ForEach(this.ItemGrid.GetChildren().ToArray(), children => children.QueueFree());

        foreach (var slotData in inventoryData.SlotDatas)
        {
            var slot = this._slot.Instantiate() as SlotUI;
            this.ItemGrid.AddChild(slot);

            slot.SlotClicked += inventoryData.OnSlotClicked;

            if (slotData != null)
            {
                slot.SetSlotData(slotData);
            }
        }
    }
}
