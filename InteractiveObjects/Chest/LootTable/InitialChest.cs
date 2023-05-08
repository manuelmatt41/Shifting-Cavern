public partial class InitialChest : InventoryData
{
    public InitialChest()
        : base(27)
    {
        var wooBlade = new SlotData();
        wooBlade.ItemData = new WoodBlade();

        this.SlotDatas[0] = wooBlade;
    }
}
