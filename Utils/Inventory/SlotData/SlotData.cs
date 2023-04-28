using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(SlotData), "", nameof(Resource))]
public partial class SlotData : Resource
{
    public const int MAX_STACK_SIZE = 99;

    [Export]
    public ItemData ItemData { get; set; }

    private int _quantity;

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

    public SlotData()
    {
        this.ItemData = null;
        this.Quantity = 1;
    }

    public bool CanFullyMergeWith(SlotData otherSlotData)
    {
        return this.ItemData == otherSlotData.ItemData
            && this.ItemData.IsStackable
            && this.Quantity + otherSlotData.Quantity <= MAX_STACK_SIZE;
    }
    public bool CanMergeWith(SlotData otherSlotData)
    {
        return this.ItemData == otherSlotData.ItemData
            && this.ItemData.IsStackable
            && this.Quantity < MAX_STACK_SIZE;
    }
    public void FullyMergeWith(SlotData otherSlotData)
    {
        this.Quantity += otherSlotData.Quantity;
    }

    public SlotData CreateSingleSlotData()
    {
        var newSlotData = this.Duplicate() as SlotData;

        newSlotData.Quantity = 1;
        this.Quantity--;

        return newSlotData;
    }
}
