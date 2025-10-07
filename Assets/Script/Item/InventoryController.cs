using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryRoot;
    public InventoryUI inventoryUI;

    private bool isOpen = false;
    float prevTimeScale = 1f;
    void Start()
    {
        Debug.Log("�C���x���g���F��\��");
        inventoryRoot.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("�C���x���g���F�\��");
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryRoot.SetActive(isOpen);
        if(isOpen)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;    // �|�[�Y
            inventoryUI.RefreshInventoryUI(); // �J���x�ɍŐV��
        }
        else
        {
            Time.timeScale = prevTimeScale;
        }
    }
}
