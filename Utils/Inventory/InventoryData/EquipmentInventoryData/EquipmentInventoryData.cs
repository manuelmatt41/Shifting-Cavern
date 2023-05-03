using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(EquipmentInventoryData), "", nameof(InventoryData))]
public partial class EquipmentInventoryData : InventoryData
{
    public EquipmentInventoryData() : base(1)
    {

    }

    public override SlotData DropSlotData(SlotData grabbedSlotData, int index)
    {
        if (grabbedSlotData.ItemData is not WeaponItemData)
        {
            return grabbedSlotData;
        }

        return base.DropSlotData(grabbedSlotData, index);

    }
    public override SlotData DropSingleSlotData(SlotData grabbedSlotData, int index)
    {
        if (grabbedSlotData.ItemData is not WeaponItemData)
        {
            return grabbedSlotData;
        }

        return base.DropSingleSlotData(grabbedSlotData, index);
    }
}
