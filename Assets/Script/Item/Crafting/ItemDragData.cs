using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragData
{
    // �ǂ̃A�C�e��
    // ��
    // ���X���b�g
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
