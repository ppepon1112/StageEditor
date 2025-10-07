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
        Debug.Log("インベントリ：非表示");
        inventoryRoot.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("インベントリ：表示");
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
            Time.timeScale = 0f;    // ポーズ
            inventoryUI.RefreshInventoryUI(); // 開く度に最新化
        }
        else
        {
            Time.timeScale = prevTimeScale;
        }
    }
}
