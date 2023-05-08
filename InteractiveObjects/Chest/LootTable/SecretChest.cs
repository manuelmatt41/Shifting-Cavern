using Godot;
using System;

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
