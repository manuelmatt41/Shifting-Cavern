using System.Collections.Generic;
using Godot;
using MonoCustomResourceRegistry;

/// <summary>
/// Clase que repsenta la informacion de un inventario
/// </summary>
[RegisteredType(nameof(InventoryData), "", nameof(Resource))]
public partial class InventoryData : Resource
{
    /// <summary>
    /// Evento que se lanza al interactuar con la interfaz de inventario
    /// </summary>
    /// <param name="inventoryData">Informacion del inventario interactuado</param>
    /// <param name="index">Indice del <c>SlotUI</c> interactuado</param>
    /// <param name="mouseButtonIndex">Boton del raton pulsado al interactuar con el inventario</param>
    [Signal]
    public delegate void InventoryInteractEventHandler(
        InventoryData inventoryData,
        int index,
        int mouseButtonIndex
    );

    /// <summary>
    /// Evento que se lanza al actualizar el inventario
    /// </summary>
    /// <param name="inventoryData">Informacion del inventario actualizado</param>
    [Signal]
    public delegate void InventoryUpdateEventHandler(InventoryData inventoryData);

    /// <summary>
    /// Cantidad de <c>SlotData</c> que tiene el <c>InventoryData</c>
    /// </summary>
    [Export]
    public SlotData[] SlotDatas;

    /// <summary>
    /// Clase para generar numeros aleatorios
    /// </summary>
    private RandomNumberGenerator _randonNumberGenerator = new();

    public InventoryData()
    {
        this.SlotDatas = null;
    }

    /// <summary>
    /// Coge un <c>SlotData</c>, comprueba  que exista en el inventario, y si es asi lo elimina y lanza el evento <c>InventoryUpdate</c>
    /// </summary>
    /// <param name="index">Ibdice que indica que <c>SlotData</c> coger</param>
    /// <returns>Devuelve el <c>SlotData</c> ya sea null o no</returns>
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

    /// <summary>
    /// Suelta un <c>SlotData</c> comprueba si existe un item en slot y si es asi intenta juntarlo sino se puede lo posiciona en esa posicion y coge el item que lo ocupaba, lanza el evento <c>InventoryUpdate</c> y devuelve el <c>SlotData</c> que se ha cogido
    /// </summary>
    /// <param name="grabbedSlotData"><c>SlotData</c> que se va a soltar</param>
    /// <param name="index">Posicion del <c>SlotData</c> donde se quiere soltar</param>
    /// <returns><c>SlotData</c> que se ha cogido al soltar el otro o <c>null</c> si no se ha cambiado por ninguno</returns>
    public virtual SlotData DropSlotData(SlotData grabbedSlotData, int index)
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

    /// <summary>
    /// Suelta un solo item del <c>SlotData</c> comprueba si existe un item del mismo tipo para juntarlo sino posiciona el item en esa posicion
    /// </summary>
    /// <param name="grabbedSlotData"><c>SlotData</c> que se va a soltar el item</param>
    /// <param name="index">Posicion del <c>SlotData</c> donde se quiere soltar</param>
    /// <returns><c>SlotData</c> que se ha soltado el item o <c>null</c> si se ha soltado el ultimo item del <c>SlotData</c></returns>
    public virtual SlotData DropSingleSlotData(SlotData grabbedSlotData, int index)
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

    /// <summary>
    /// Coge un <c>SlotData</c> y lo anyade en la posicion donde tenga el mismo item y pueda juntarlo o sino en la primera posicion vacia sino no lo anyade al inventario
    /// </summary>
    /// <param name="slotData"><c>SlotData</c> que se va anyadir al inventario</param>
    /// <returns><c>true</c> si lo anyadio al inventario sino <c>false</c></returns>
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

    /// <summary>
    /// Suelte <c>SLotData</c> de forma aleatoria dependiendo del <c>DropRate</c> que tenga el <c>SlotData</c>
    /// </summary>
    /// <returns>La cantidad de <c>SlotData</c> que se hayan escogido de forma aleatoria</returns>
    public SlotData[] DropRandomSlotDatas()
    {
        List<SlotData> randomSlotDatas = new();

        foreach (var slotData in this.SlotDatas)
        {
            if (
                slotData != null
                && slotData.DropRate >= this._randonNumberGenerator.RandiRange(1, 100)
            )
            {
                randomSlotDatas.Add(slotData);
            }
        }

        return randomSlotDatas.ToArray();
    }

    /// <summary>
    /// Funcion que se ejecuta al clicar en un <c>SlotUi</c> y que lanza el evento <c>InventoryInteract</c>
    /// </summary>
    /// <param name="index">Indice del <c>SLotUI</c> que se ha hecho click</param>
    /// <param name="mouseButtonIndex">Boton del raton que se ha pulsado</param>
    public void OnSlotClicked(int index, int mouseButtonIndex) // NO se puede pasar el objeto, hay que hacerlo en la propia funcion
    {
        this.EmitSignal(SignalName.InventoryInteract, this, index, mouseButtonIndex);
    }
}
