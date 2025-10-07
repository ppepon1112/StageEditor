using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public PlayerStatusData playerStatusData;
    public ItemDatabase itemDatabase;

    public void AddItem(string itemID)
    {
        // ���łɏ������Ă��邩�`�F�b�N
        ItemEntry existing = playerStatusData.saveData.ownedItems.Find(e => e.itemID == itemID);
        if (existing != null)
        {
            existing.Count++; // �J�E���g�𑝂₷
            Debug.Log($"���łɏ����F{itemID} �� �J�E���g+1 = {existing.Count}");
        }
        else
        {
            // �V�K�A�C�e���Ƃ��Ēǉ�
            ItemEntry newEntry = new ItemEntry { itemID = itemID, Count = 1 };
            playerStatusData.saveData.ownedItems.Add(newEntry);
            Debug.Log($"�V�K�A�C�e���ǉ��F{itemID}");
        }
    }

    public bool HasItem(string itemID)
    {
        return playerStatusData.saveData.ownedItems.Exists(e => e.itemID == itemID);
    }
}
