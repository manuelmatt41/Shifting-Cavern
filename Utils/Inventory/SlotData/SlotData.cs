using Godot;
using MonoCustomResourceRegistry;

/// <summary>
/// Clase que repsenta un la informacion que contiene un espacio de inventario
/// </summary>
[RegisteredType(nameof(SlotData), "", nameof(Resource))]
public partial class SlotData : Resource
{
    /// <summary>
    /// Valor que representa la cantidad maxima de items que puede haber en un slot
    /// </summary>
    public const int MAX_STACK_SIZE = 99;

    /// <summary>
    /// Tipo de item que contiene el slot
    /// </summary>
    [Export]
    public ItemData ItemData { get; set; }

    private int _quantity;

    /// <summary>
    /// Cantidad que hay del item en el slot
    /// </summary>
    [Export(PropertyHint.Range, "1, 99")]
    public int Quantity
    {
        get => this._quantity;
        set
        {
            this._quantity = value;

            if (this._quantity > 1 && !this.ItemData.IsStackable)
            {
                this._quantity = 1;
                GD.PushError($"{this.ItemData.Name} is not stackable");
            }
        }
    }

    /// <summary>
    /// Probabilidad de que el item se suelte al juego al morir determinado enemigo
    /// </summary>
    [Export(PropertyHint.Range, "1, 100")]
    public int DropRate { get; set; }

    public SlotData()
    {
        this.ItemData = null;
        this.Quantity = 1;
        this.DropRate = 0;
    }

    /// <summary>
    /// Comprueba que se pueda juntar los items de otro <c>SlotData</c> al completo
    /// </summary>
    /// <param name="otherSlotData"><c>SlotData</c> que se comprueba si se puede juntar</param>
    /// <returns><c>true</c> si se puede juntar al completo sino <c>false</c></returns>
    public bool CanFullyMergeWith(SlotData otherSlotData)
    {
        return this.ItemData == otherSlotData.ItemData
            && this.ItemData.IsStackable
            && this.Quantity + otherSlotData.Quantity <= MAX_STACK_SIZE;
    }

    /// <summary>
    /// Comprueba que se pueda juntar los items de otro slot data
    /// </summary>
    /// <param name="otherSlotData"><c>SlotData</c> que se comprueba si se puede juntar</param>
    /// <returns><c>true</c> si se puede juntar sino <c>false</c></returns>
    public bool CanMergeWith(SlotData otherSlotData)
    {
        return this.ItemData == otherSlotData.ItemData
            && this.ItemData.IsStackable
            && this.Quantity < MAX_STACK_SIZE;
    }

    /// <summary>
    /// Junta al completo la cantidad de items de otro <c>SlotData</c>
    /// </summary>
    /// <param name="otherSlotData"><c>SlotData</c> que va a juntar el total de sus items</param>
    public void FullyMergeWith(SlotData otherSlotData)
    {
        this.Quantity += otherSlotData.Quantity;
    }

    /// <summary>
    /// Crea una copia de <c>SlotData</c> con un solo item, y se resta uno asi mismo
    /// </summary>
    /// <returns>La copia de <c>SlotData</c> con un item</returns>
    public SlotData CreateSingleSlotData()
    {
        var newSlotData = this.Duplicate() as SlotData;

        newSlotData.Quantity = 1;
        this.Quantity--;

        return newSlotData;
    }
}
