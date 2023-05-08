using Godot;
using System;

public partial class GoblinLootTable : InventoryData
{
    public GoblinLootTable()
        : base(2)
    {
        var coins = new SlotData();
        coins.ItemData = new Coin();
        coins.Quantity = 2;
        coins.DropRate = 75;
        this.SlotDatas[0] = coins;

        var rottenFishes = new SlotData();
        rottenFishes.ItemData = new RottenFish();
        rottenFishes.Quantity = 1;
        rottenFishes.DropRate = 25;
        this.SlotDatas[1] = rottenFishes;
    }
}
