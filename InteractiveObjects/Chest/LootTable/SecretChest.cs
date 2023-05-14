/// <summary>
/// Clase que genera un inventario para un cofre con una espada magica
/// </summary>
public partial class SecretChest : InventoryData
{
    public SecretChest()
      : base(27)
    {
        var magicSword = new SlotData();
        magicSword.ItemData = new MagicSword();

        this.SlotDatas[0] = magicSword;
    }
}
