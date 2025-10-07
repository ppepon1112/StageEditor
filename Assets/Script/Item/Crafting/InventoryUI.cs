using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform contentParent;
    public PlayerStatusData statusData;
    public ItemDatabase itemDatabase;

    private List<GameObject> slots  = new List<GameObject>();

    void OnEnable() => RefreshInventoryUI();
    void Start()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if(inventory != null)
        {
            inventory.ItemChanged += RefreshInventoryUI;
        }
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        // �����̃X���b�g�����ׂč폜
        foreach(GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

        if(statusData == null || statusData.saveData.ownedItems == null)
        {
            Debug.Log("InventoryUI: �����A�C�e�������ݒ�ł�");
            return;
        }

        // �f�o�b�N�F�����A�C�e������\��
        Debug.Log("�����A�C�e���F " + statusData.saveData.ownedItems.Count);

        // �����A�C�e���̐������X���b�g���쐬
        foreach(ItemEntry entry in statusData.saveData.ownedItems)
        {
            // �����ŕϊ�
            ItemData item = itemDatabase.GetItemByName(entry.itemID);

            if (item == null) continue;
            if (item != null)
            {
                GameObject slotObj = Instantiate(slotPrefab, contentParent);
                ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();

                if (slotUI != null)
                {
                    slotUI.SetItem(item,entry.Count);
                }
                slots.Add(slotObj);
            }
            else
            {
                Debug.LogWarning($"Item '{entry.itemID}' �� ItemDatabase �Ɍ�����܂���ł���");
            }
        }
    }
}
