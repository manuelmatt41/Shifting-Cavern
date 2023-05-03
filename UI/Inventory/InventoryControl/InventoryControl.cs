using System;
using System.Threading.Tasks;
using Godot;

/// <summary>
/// Clase que representa al control de inventarios del juego
/// </summary>
public partial class InventoryControl : Control
{
    /// <summary>
    /// Evento que se lanza al soltar un <c>SlotData</c> fuera del inventario
    /// </summary>
    /// <param name="slotData"><c>SlotData</c> que se ha lanzado</param>
    [Signal]
    public delegate void DropSlotDataEventHandler(SlotData slotData);


    [Signal]
    public delegate void ToogleInventoryControlEventHandler();

    /// <summary>
    /// Interfaz que repsenta el inventario del juagador
    /// </summary>
    public InventoryUI PlayerInventory { get; set; }
    public InventoryUI PlayerEquipmentInventory { get; set; }

    /// <summary>
    /// <c>SlotUI</c> que se cogio con el raton
    /// </summary>
    public SlotUI GrabbedSlotUI { get; set; }

    public InventoryUI ExternalInvetoryUI { get; set; }

    /// <summary>
    /// <c>SlotData</c> que se cogio con el raton
    /// </summary>
    public SlotData GrabbedSlotData { get; set; }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta al crear el nodo en la escena, se usa para iniciar las variables de nodos subyacentes de <c>InventoryControl</c>
    /// </summary>
    public override void _Ready()
    {
        this.PlayerInventory = this.GetNode<InventoryUI>("PlayerInventory");
        this.PlayerEquipmentInventory = this.GetNode<InventoryUI>("EquipmentInventory");
        this.GrabbedSlotUI = this.GetNode<SlotUI>("GrabbedSlot");
        this.ExternalInvetoryUI = this.GetNode<InventoryUI>("ExternalInventoryUI");
    }

    /// <summary>
    /// Funcion integrada de Godot que se ejecuta entre frames de fisicas, para que el <c>SlotUI</c> que se cogio siga al raton
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (this.GrabbedSlotUI.Visible)
        {
            this.GrabbedSlotUI.GlobalPosition = this.GetGlobalMousePosition() + new Vector2(5, 5);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ToogleInventory"))
        {
            this.EmitSignal(SignalName.ToogleInventoryControl);
            this.GetTree().Paused = !this.GetTree().Paused;
            //OS.ShellOpen("https://github.com/manuelmatt41/Shifting-Cavern"); TODO Interesante para hacer link buttons para una pagina web
        }
    }

    /// <summary>
    /// Coloca la informacion del inventario de <c>Player</c> en el <c>InventoryUI</c>
    /// </summary>
    /// <param name="inventoryData">Informacion del inventario de <c>Player</c></param>
    public void SetPlayerInventoryData(InventoryData inventoryData)
    {
        inventoryData.InventoryInteract += this.OnInventoryInteract;
        this.PlayerInventory.SetInventoryData(inventoryData);
    }

    /// <summary>
    /// Coloca la informacion del inventario de <c>Player</c> en el <c>InventoryUI</c>
    /// </summary>
    /// <param name="inventoryData">Informacion del inventario de <c>Player</c></param>
    public void SetPlayerEquipmentInventoryData(InventoryData inventoryData)
    {
        inventoryData.InventoryInteract += this.OnInventoryInteract;
        this.PlayerEquipmentInventory.SetInventoryData(inventoryData);
    }

    public void SetExternalInventoryData(InventoryData inventoryData)
    {
        inventoryData.InventoryInteract -= this.OnInventoryInteract;
        inventoryData.InventoryInteract += this.OnInventoryInteract;
        this.ExternalInvetoryUI.SetInventoryData(inventoryData);
    }

    public void ClearExternalInventoryData() => this.ExternalInvetoryUI.ClearInventoryData();

    /// <summary>
    /// Actualiza el <c>SlotUI</c> y se cogio con el raton si se coge nueva <c>SlotData</c>
    /// </summary>
    public void UpdateGrabbedSlot()
    {
        if (this.GrabbedSlotData != null)
        {
            this.GrabbedSlotUI.Show();
            this.GrabbedSlotUI.SetSlotData(this.GrabbedSlotData);
        }
        else
        {
            this.GrabbedSlotUI.Hide();
        }
    }

    /// <summary>
    /// Funcion que se ejecuta al interactuar con el <c>InventoryUI</c> para comprobar que tipo de <c>SlotUI</c> se ha pulsado y la inforamcion que contenia
    /// </summary>
    /// <param name="inventoryData">Informacion del inventario que se ha pulsado</param>
    /// <param name="index">Indice del <c>SlotUI</c> se ha interactuado</param>
    /// <param name="mouseButtonIndex">Boton del raton que se ha pulsado al interactuar</param>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// Funcion que se ejcuta al interactuar  con <c>InvetoryControl</c> comprueba que suelta el <c>SlotData</c> que tiene el raton a donde se este pulsando ya sea fuera o dentro del inventario
    /// </summary>
    /// <param name="event">Tipo de evento que se lanzo al interactuar</param>
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

    /// <summary>
    /// Funcion que se ejecuta al cambiar la visibilidad de <c>InventoryControl</c> haciendo que el objeto que se tenga en el raton se suelte de forma automatica
    /// </summary>
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
