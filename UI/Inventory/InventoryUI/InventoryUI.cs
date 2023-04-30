using Godot;
using System;
using System.Linq;

/// <summary>
/// Clase que representa el inventario de forma grafica
/// </summary>
public partial class InventoryUI : PanelContainer
{
    /// <summary>
    /// Panel en forma de cuadricula que contiene los <c>SlotUI</c>
    /// </summary>
    public GridContainer ItemGrid { get; private set; }

    /// <summary>
    /// <c>SlotUI</c> de forma que se pueda instanciar
    /// </summary>
    private readonly PackedScene _slot = GD.Load<PackedScene>(
        "res://UI/Inventory/SlotUi/slot_ui.tscn"
    );

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>HurtBox</c>
    /// </summary>
    public override void _Ready()
    {
        this.ItemGrid = this.GetNode<MarginContainer>("MarginContainer")
            .GetNode<GridContainer>("ItemGrid");
    }

    /// <summary>
    /// Representa la informacion del <c>InventoryData</c> en <c>InventoryUI</c>
    /// </summary>
    /// <param name="inventoryData">Informacion que se va representar</param>
    public void SetInventoryData(InventoryData inventoryData)
    {
        inventoryData.InventoryUpdate += this.OnInventoryUpdate;
        this.OnInventoryUpdate(inventoryData);
    }

    /// <summary>
    /// Funcion que se ejecuta al actualizarse el <c>InventoryData</c> donde borra los <c>SlotUI</c> y los vuelve a instanciar con los datos actualizados
    /// </summary>
    /// <param name="inventoryData">Informacion actualizada del inventario</param>
    private void OnInventoryUpdate(InventoryData inventoryData)
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
