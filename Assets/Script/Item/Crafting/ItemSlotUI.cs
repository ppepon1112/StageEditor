using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// スロット内アイテムのIconとNameテキストの表示
/// </summary>
public class ItemSlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("UI")]
    public Image icon;
    public Text nameText;

    [Header("内部保持")]
    [HideInInspector] public ItemData item;
    [HideInInspector] public int count;

    // ドラッグ用
    public Canvas canvas;
    public CanvasGroup cg;
    public Vector2 dragOffset;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        cg = gameObject.AddComponent<CanvasGroup>();
    }
    // ------ 表示セット -------
    public void SetItem(ItemData data,int cnt)
    {
        item = data;
        count = cnt;
        if(icon != null) icon.sprite = data.icon;
        if (nameText != null) nameText.text = $"{data.ItemName} x{cnt}";
        gameObject.SetActive(true);
    }
    public void OnBeginDrag(PointerEventData ev)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            ev.position, ev.pressEventCamera, out var localMousePos);

        dragOffset = (Vector2)transform.localPosition;

        transform.SetParent(canvas.transform, false);
        cg.blocksRaycasts = false;
        Debug.Log("ドラッグ");
    }

    public void OnDrag(PointerEventData ev)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            ev.position, ev.pressEventCamera, out var localMousePos);
        transform.localPosition = localMousePos;
    }

    public void OnEndDrag(PointerEventData ev)
    {
        Debug.Log("ドロップ");
        cg.blocksRaycasts = true;
        // ドロップ先が無ければ元スロットへ
        if(transform.parent == canvas.transform)
        {
            ReturnToOrigin();
        }
    }
    public ItemDragData GetDragData()
    {
        return new ItemDragData(item, 1, this);
    }

    public void OnDrop(PointerEventData ev)
    {
        var drag = ev.pointerDrag?.GetComponent<ItemSlotUI>();
        if (drag == null) return;
        
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        inv.AddItem(drag.item);
        drag.count--;
        if (drag.count <= 0) drag.Clear();

        FindObjectOfType<InventoryUI>()?.RefreshInventoryUI();
    }

    public void Clear()
    {
        item = null;
        count = 0;
        icon.sprite = null;
        nameText.text = "";
    }

    public void ReturnToOrigin()
    {
        Destroy(gameObject);
        FindObjectOfType<InventoryUI>()?.RefreshInventoryUI();
    }
}
