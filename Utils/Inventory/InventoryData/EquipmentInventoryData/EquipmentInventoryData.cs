using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(EquipmentInventoryData), "", nameof(InventoryData))]
public partial class EquipmentInventoryData : InventoryData
{
    [Signal]
    public delegate void WeaponChangeEventHandler(WeaponItemData weaponitemData);

    public EquipmentInventoryData() : base(4)
    {

    }

    public override SlotData DropSlotData(SlotData grabbedSlotData, int index)
    {
        if (grabbedSlotData.ItemData is not WeaponItemData)
        {
            return grabbedSlotData;
        }

        this.EmitSignal(SignalName.WeaponChange, grabbedSlotData.ItemData);
        return base.DropSlotData(grabbedSlotData, index);

    }
    public override SlotData DropSingleSlotData(SlotData grabbedSlotData, int index)
    {
        if (grabbedSlotData.ItemData is not WeaponItemData)
        {
            return grabbedSlotData;
        }

        this.EmitSignal(SignalName.WeaponChange, grabbedSlotData.ItemData);
        return base.DropSingleSlotData(grabbedSlotData, index);
    }
}
