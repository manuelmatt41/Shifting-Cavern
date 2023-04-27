using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(InventoryData), "", nameof(Resource))]

public partial class InventoryData : Resource
{
    [Export]
    public SlotData[] SlotDatas;


    public InventoryData()
    {
        this.SlotDatas = null;
    }
}
