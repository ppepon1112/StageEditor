using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragData
{
    // どのアイテム
    // 個数
    // 元スロット
    public ItemData item;
    public int count;
    public ItemSlotUI origin;

    public ItemDragData(ItemData item, int count, ItemSlotUI origin)
    {
        this.item = item;
        this.count = count;
        this.origin = origin;
    }
}
