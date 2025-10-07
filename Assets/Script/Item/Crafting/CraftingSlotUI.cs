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
        // CanvasGroup �m��
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // DragItem �ڈ���t�^
        if (GetComponent<DragItem>() == null)
            gameObject.AddComponent<DragItem>();

        // InventoryUI �������T���iInspector �œ���Ă� OK�j
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void OnDrop(PointerEventData ev)
    {
        // �h���b�O���f�[�^�擾
        //if (ev.pointerDrag == null) return;
        var from = ev.pointerDrag?.GetComponent<ItemSlotUI>();
        if (from == null) return;

        var data = from.GetDragData();
        // ����̓N���t�g�X���b�g�ɓ����Ȃ�
        if (data.item.type == ItemType.Weapon) return;

        // ���łɃX���b�g�ɕʂ̃A�C�e��������ꍇ�A�C���x���g���ɃA�C�e����ԋp
        if(item != null && item.ItemID != data.item.ItemID)
        {
            var P_inv = FindObjectOfType<PlayerInventory>();
            Debug.Log($"�N���t�g�X���b�g�ɂ�����{item.ItemName} x{count}��ԋp");
            if (P_inv != null) P_inv.AddItem(item, count); // �O�̃A�C�e����ԋp
        }

        // �X���b�g����Ȃ�󂯓����
        // �V�����A�C�e���ŏ㏑�� or ���Z
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

        // �C���x���g�����̌����P���炷
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        inv.ReduceItem(item, data.count);

        // �C���x���g�����ĕ`��
        FindObjectOfType<InventoryUI>()?.RefreshInventoryUI();
        from.ReturnToOrigin();
    }

    public void ReceiveItem(ItemDragData data)
    {
        // �ʂ̃A�C�e���������Ă�����ԋp
        if(item != null && item != data.item)
        {
            var P_inv = FindObjectOfType<PlayerInventory>();
            Debug.Log($"�N���t�g�X���b�g�ɂ�����{item.ItemName} x{count}��ԋp");
            if (P_inv != null) P_inv.AddItem(item, count);
        }
        // ���łɓ����A�C�e��������Ȃ���Z
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

        // �C���x���g�����̌����P���炷
        PlayerInventory inv = FindObjectOfType<PlayerInventory>();
        inv.ReduceItem(item, data.count);

        // �C���x���g�����ĕ`��
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
        inv.AddItem(item,count); // count���ǉ��i������count�g���Ă������j

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
