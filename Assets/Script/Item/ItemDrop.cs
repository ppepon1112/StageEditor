using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�g���K�[�ɓ�����: " + other.name);
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player�Ƀq�b�g�I");
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if(inventory != null)
            {
                Debug.Log("Inventory�ɃA�N�Z�X����");
                inventory.AddItem(itemData);
                Debug.Log($"{itemData.ItemName} ���E�����I");
                Destroy(gameObject);
            }
        }
    }
}
