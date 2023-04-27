using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(SlotData), "", nameof(Resource))]
public partial class SlotData : Resource
{
    public const int MAX_STACK_SIZE = 99;

    [Export]
    public ItemData ItemData { get; set; }

    [Export(PropertyHint.Range, "1, 99")]
    public int Quantity { get; set; }

    public SlotData()
    {
        this.ItemData = null;
        this.Quantity = 1;
    }
}
