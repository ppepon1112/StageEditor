using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("トリガーに入った: " + other.name);
        if(other.CompareTag("Player"))
        {
            Debug.Log("Playerにヒット！");
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if(inventory != null)
            {
                Debug.Log("Inventoryにアクセス成功");
                inventory.AddItem(itemData);
                Debug.Log($"{itemData.ItemName} を拾った！");
                Destroy(gameObject);
            }
        }
    }
}
