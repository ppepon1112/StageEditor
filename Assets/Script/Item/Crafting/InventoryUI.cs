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
        // 既存のスロットをすべて削除
        foreach(GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

        if(statusData == null || statusData.saveData.ownedItems == null)
        {
            Debug.Log("InventoryUI: 所持アイテムが未設定です");
            return;
        }

        // デバック：所持アイテム数を表示
        Debug.Log("所持アイテム： " + statusData.saveData.ownedItems.Count);

        // 所持アイテムの数だけスロットを作成
        foreach(ItemEntry entry in statusData.saveData.ownedItems)
        {
            // ここで変換
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
                Debug.LogWarning($"Item '{entry.itemID}' が ItemDatabase に見つかりませんでした");
            }
        }
    }
}
