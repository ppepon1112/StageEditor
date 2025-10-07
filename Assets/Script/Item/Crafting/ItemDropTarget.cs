using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ItemDropTarget : IEventSystemHandler
{
    void ReceiveItem(ItemDragData data);
}
