using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class InventoryControl : Control
{
    [Signal]
    public delegate void DropSlotDataEventHandler(SlotData slotData);
    public InventoryUI InventoryUI { get; set; }

    public SlotUI GrabbedSlot { get; set; }

    public SlotData GrabbedSlotData { get; set; }

    public override void _Ready()
    {
        this.InventoryUI = this.GetNode<InventoryUI>("InventoryUI");
        this.GrabbedSlot = this.GetNode<SlotUI>("GrabbedSlot");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (this.GrabbedSlot.Visible)
        {
            this.GrabbedSlot.GlobalPosition = this.GetGlobalMousePosition() + new Vector2(5, 5);
        }
    }

    public void SetPlayerInventoryData(InventoryData inventoryData)
    {
        inventoryData.InventoryInteract += this.OnInventoryInteract;
        this.InventoryUI.SetInventoryData(inventoryData);
    }

    public void UpdateGrabbedSlot()
    {
        if (this.GrabbedSlotData != null)
        {
            this.GrabbedSlot.Show();
            this.GrabbedSlot.SetSlotData(this.GrabbedSlotData);
        }
        else
        {
            this.GrabbedSlot.Hide();
        }
    }

    private void OnInventoryInteract(InventoryData inventoryData, int index, int mouseButtonIndex)
    {
        this.GrabbedSlotData = new
        {
            SlotData = this.GrabbedSlotData,
            ButtonIndex = mouseButtonIndex
        } switch
        {
            { SlotData: null, ButtonIndex: (int)MouseButton.Left }
                => inventoryData.GrabSlotData(index),
            { SlotData: _, ButtonIndex: (int)MouseButton.Left }
                => inventoryData.DropSlotData(this.GrabbedSlotData, index),
            { SlotData: null, ButtonIndex: (int)MouseButton.Right }
                => null /*inventoryData.GrabSlotData(index)*/ //TODO Coger un solo item del Slot
            ,
            { SlotData: _, ButtonIndex: (int)MouseButton.Right }
                => inventoryData.DropSingleSlotData(this.GrabbedSlotData, index),
            _
                => throw new ArgumentException(
                    "Se ha pulsado en el menu con un boton del raton incorrecto"
                ),
        };

        this.UpdateGrabbedSlot();
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (
            @event is InputEventMouseButton mouseEvent
            && mouseEvent.IsPressed()
            && this.GrabbedSlotData != null
        )
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.Left:
                    this.EmitSignal(SignalName.DropSlotData, this.GrabbedSlotData);
                    this.GrabbedSlotData = null;
                    this.UpdateGrabbedSlot();
                    break;
                case MouseButton.Right:
                    this.EmitSignal(
                        SignalName.DropSlotData,
                        this.GrabbedSlotData.CreateSingleSlotData()
                    );

                    if (this.GrabbedSlotData.Quantity < 1)
                    {
                        this.GrabbedSlotData = null;
                    }

                    this.UpdateGrabbedSlot();
                    break;
            }
        }
    }

    private void OnInventoryControlVisibilityChanged()
    {
        if (!this.Visible && this.GrabbedSlotData != null)
        {
            this.EmitSignal(SignalName.DropSlotData, this.GrabbedSlotData);
            this.GrabbedSlotData = null;
            this.UpdateGrabbedSlot();
        }
    }
}
