using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CraftingSlotUI : MonoBehaviour,IDropHandler, IPointerClickHandler
{
    public Image icon;
    public Text nameText;

    [HideInInspector] public ItemData item;
    [HideInInspector] public int count;

    public InventoryUI inventoryUI;

    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // CanvasGroup 確保
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // DragItem 目印も付与
        if (GetComponent<DragItem>() == null)
            gameObject.AddComponent<DragItem>();

        // InventoryUI を自動探索（Inspector で入れても OK）
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void OnDrop(PointerEventData ev)
    {
        // ドラッグ元データ取得
        //if (ev.pointerDrag == null) return;
        var from = ev.pointerDrag?.GetComponent<ItemSlotUI>();
        if (from == null) return;

        var data = from.GetDragData();
        // 武器はクラフトスロットに入れれない
        if (data.item.type == ItemType.Weapon) return;

        // すでにスロットに別のアイテムがある場合、インベントリにアイテムを返却
        if(item != null && item.ItemID != data.item.ItemID)
        {
            var P_inv = FindObjectOfType<PlayerInventory>();
            Debug.Log($"クラフトスロットにあった{item.ItemName} x{count}を返却");
            if (P_inv != null) P_inv.AddItem(item, count); // 前のアイテムを返却
        }

        // スロットが空なら受け入れる
        // 新しいアイテムで上書き or 加算
        if(item != null && item.ItemID == data.item.ItemID)
        {
            count += data.count;
        }
        else
        {
            item = data.item;
            count = data.count;
        }
        icon.enabled = true;
        icon.sprite = item.icon;
        nameText.text = $"{item.ItemName} x{count}";

        // インベントリ側の個数を１減らす
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        inv.ReduceItem(item, data.count);

        // インベントリを再描画
        FindObjectOfType<InventoryUI>()?.RefreshInventoryUI();
        from.ReturnToOrigin();
    }

    public void ReceiveItem(ItemDragData data)
    {
        // 別のアイテムが入っていたら返却
        if(item != null && item != data.item)
        {
            var P_inv = FindObjectOfType<PlayerInventory>();
            Debug.Log($"クラフトスロットにあった{item.ItemName} x{count}を返却");
            if (P_inv != null) P_inv.AddItem(item, count);
        }
        // すでに同じアイテムがあるなら加算
        if(item == data.item)
        {
            count += data.count;
        }
        else
        {
            item = data.item;
            count = data.count;
        }
        icon.enabled = true;
        icon.sprite = item.icon;
        nameText.text = $"{item.ItemName} x{count}";

        // インベントリ側の個数を１減らす
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        inv.ReduceItem(item, data.count);

        // インベントリを再描画
        FindObjectOfType<InventoryUI>()?.RefreshInventoryUI();
    }

    public void SetItem(ItemData data, int cnt)
    {
        item = data;
        count = cnt;
        icon.sprite = data.icon;
        icon.enabled = true;
        nameText.text = $"{data.ItemName} x{cnt}";
        gameObject.SetActive(true);
    }

    public void ReturnToInventory()
    {
        if (item == null || count <= 0) return;

        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        inv.AddItem(item,count); // count分追加（ここでcount使ってもいい）

        Clear();
    }
    public void Clear()
    {
        item = null;
        count = 0;
        icon.sprite = null;
        icon.enabled = false;
        nameText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ReturnToInventory();
            inventoryUI?.RefreshInventoryUI();
        }
    }

}
