using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public PlayerStatusData playerStatusData;
    public ItemDatabase itemDatabase;

    public void AddItem(string itemID)
    {
        // すでに所持しているかチェック
        ItemEntry existing = playerStatusData.saveData.ownedItems.Find(e => e.itemID == itemID);
        if (existing != null)
        {
            existing.Count++; // カウントを増やす
            Debug.Log($"すでに所持：{itemID} → カウント+1 = {existing.Count}");
        }
        else
        {
            // 新規アイテムとして追加
            ItemEntry newEntry = new ItemEntry { itemID = itemID, Count = 1 };
            playerStatusData.saveData.ownedItems.Add(newEntry);
            Debug.Log($"新規アイテム追加：{itemID}");
        }
    }

    public bool HasItem(string itemID)
    {
        return playerStatusData.saveData.ownedItems.Exists(e => e.itemID == itemID);
    }
}
