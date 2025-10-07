using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public PlayerStatusData statusData;
    public List<ItemData> ownedItems = new List<ItemData>();

    public delegate void OnItemChanged();
    public event OnItemChanged ItemChanged;

    public ItemData EquippedWeapon { get; private set; }

    public ItemDatabase itemDatabase;

    void Awake()
    {
        LoadFromSave();
    }

    void LoadFromSave()
    {
        ownedItems.Clear();
        foreach(var entry in statusData.saveData.ownedItems)
        {
            ItemData data = itemDatabase.GetItemByName(entry.itemID);
            if(data != null)
                ownedItems.Add(data);
        }
    }

    public void Equip(ItemData weapon)
    {
        if(weapon != null && weapon.type == ItemType.Weapon)
            EquippedWeapon = weapon;
    }

    public void AddItem(ItemData itemData,int addCount = 1)
    {
        if (itemData == null) return;

        string id = itemData.ItemID;

        if(itemData.type == ItemType.Weapon && statusData.saveData.ownedItems.Exists(e => e.itemID == id))
        {
            Debug.Log($"����{id} �͂��łɏ������Ă��܂�");
            return;
        }

        var entry = statusData.saveData.ownedItems.Find(i => i.itemID == id);
        if(entry == null)
        {
            statusData.saveData.ownedItems.Add(new ItemEntry(id, addCount));
            //Debug.Log($"{itemData.ItemName} ��ǉ� (�������F{entry.Count})");
        }
        else
        {
            entry.Count += addCount;
            //Debug.Log($"{itemData.ItemName} �����擾");
        }

        if (!ownedItems.Exists(e => e.ItemID == id))
            ownedItems.Add(itemData);

        ItemChanged?.Invoke();
        SaveLoadManager.Save(statusData);
    }

    internal void ReduceItem(ItemData itemData, int reduce = 1)
    {
        string id = itemData.ItemID;
        var entry = statusData.saveData.ownedItems.Find(i => i.itemID == id);

        if(entry != null)
        {
            entry.Count -= reduce;
            if (entry.Count <= 0)
            {
                statusData.saveData.ownedItems.Remove(entry);
                ownedItems.RemoveAll(i => i.ItemID == id);
            }
        }
        SaveLoadManager.Save(statusData);
    }

    public bool UseItem(string itemID)
    {
        var entry = statusData.saveData.ownedItems.Find(i => i.itemID == itemID);
        if (entry == null || entry.Count <= 0) return false;

        ItemData data = itemDatabase.GetItemByName(itemID);
        if (data == null || data.type != ItemType.Consumable) return false;

        // ���ʂ̓K�p
        var playerStatus = FindObjectOfType<PlayerHealth>();
        if (playerStatus != null)
        {
            playerStatus.Heal(data.healAmount);
            Debug.Log($"{data.ItemID} ���g�p �� HP{data.healAmount}��");
        }

        entry.Count--;
        if (entry.Count <= 0)
        {
            statusData.saveData.ownedItems.Remove(entry);
            ownedItems.RemoveAll(i => i.ItemID == itemID);
        }

        ItemChanged?.Invoke();
        SaveLoadManager.Save(statusData);
        return true;
    }
}
