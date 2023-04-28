using System;
using System.Reflection;
using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(InventoryData), "", nameof(Resource))]
public partial class InventoryData : Resource
{
    [Signal]
    public delegate void InventoryInteractEventHandler(
        InventoryData inventoryData,
        int index,
        int mouseButtonIndex
    );

    [Signal]
    public delegate void InventoryUpdateEventHandler(InventoryData inventory);

    [Export]
    public SlotData[] SlotDatas;

    public InventoryData()
    {
        this.SlotDatas = null;
    }

    public SlotData GrabSlotData(int index)
    {
        var slotdata = this.SlotDatas[index];

        if (slotdata != null)
        {
            this.SlotDatas[index] = null;
            this.EmitSignal(SignalName.InventoryUpdate, this);
        }

        return slotdata;
    }

    public SlotData DropSlotData(SlotData grabbedSlotData, int index)
    {
        var slotData = this.SlotDatas[index];

        SlotData returnSlotData = null;

        if (slotData != null && slotData.CanFullyMergeWith(grabbedSlotData))
        {
            slotData.FullyMergeWith(grabbedSlotData);
        }
        else
        {
            this.SlotDatas[index] = grabbedSlotData;
            returnSlotData = slotData;
        }

        this.EmitSignal(SignalName.InventoryUpdate, this);

        return returnSlotData;
    }

    public SlotData DropSingleSlotData(SlotData grabbedSlotData, int index)
    {
        var slotData = this.SlotDatas[index];

        if (slotData == null)
        {
            this.SlotDatas[index] = grabbedSlotData.CreateSingleSlotData();
        }
        else if (slotData.CanMergeWith(grabbedSlotData))
        {
            slotData.FullyMergeWith(grabbedSlotData.CreateSingleSlotData());
        }

        this.EmitSignal(SignalName.InventoryUpdate, this);

        if (grabbedSlotData.Quantity > 0)
        {
            return grabbedSlotData;
        }

        return null;
    }

    public bool PickUpSlotData(SlotData slotData)
    {
        for (int i = 0; i < this.SlotDatas.Length; i++)
        {
            if (this.SlotDatas[i] != null && this.SlotDatas[i].CanFullyMergeWith(slotData))
            {
                this.SlotDatas[i].FullyMergeWith(slotData);
                this.EmitSignal(SignalName.InventoryUpdate, this);
                return true;
            }
        }

        for (int i = 0; i < this.SlotDatas.Length; i++)
        {
            if (this.SlotDatas[i] == null)
            {
                this.SlotDatas[i] = slotData;
                this.EmitSignal(SignalName.InventoryUpdate, this);
                return true;
            }
        }

        return false;
    }

    public void OnSlotClicked(int index, int mouseButtonIndex) // NO se puede pasar el objeto, hay que hacerlo en la propia funcion
    {
        this.EmitSignal(SignalName.InventoryInteract, this, index, mouseButtonIndex);
    }
}
