using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [Header("Slot & UI")]
    public CraftingSlotUI slotA;
    public CraftingSlotUI slotB;
    public ResultSlotUI resultSlot;
    public Button craftButton;
    public InventoryUI inventoryUI;
    [Header("Recipes")]
    public List<CraftingRecipe> recipeList;

    private ItemData currentResult = null;
    // Start is called before the first frame update
    void Start()
    {
        craftButton.onClick.AddListener(OnCraft);
    }
    void Update()
    {
        UpdateResultSlot();
    }

    void UpdateResultSlot()
    {
        if(slotA.item == null || slotB.item == null)
        {
            resultSlot.SetItem(null);
            currentResult = null;
            return;
        }

        foreach(var recipe in recipeList)
        {
            bool matchA = (slotA.item == recipe.inputA && slotB.item == recipe.inputB);
            bool matchB = (slotA.item == recipe.inputB && slotB.item == recipe.inputA);

            if(matchA || matchB)
            {
                resultSlot.SetItem(recipe.result);
                currentResult = recipe.result;
                return;
            }
        }
        // �Y�����V�s��������΋�
        resultSlot.SetItem(null);
        currentResult = null;
    }

    void OnCraft()
    {
        if (currentResult == null || slotA.item == null || slotB.item == null) return;
        // �f�ނ����炷
        slotA.count--;
        slotB.count--;

        if (slotA.count <= 0) slotA.Clear();
        else slotA.nameText.text = $"{slotA.item.ItemName} x{slotA.count}";

        if (slotB.count <= 0) slotB.Clear();
        else slotB.nameText.text = $"{slotB.item.ItemName} x{slotB.count}";

        // �C���x���g���ɒǉ�
        FindObjectOfType<PlayerInventory>()?.AddItem(currentResult);

        // UI�X�V
        inventoryUI?.RefreshInventoryUI();
        Debug.Log($"�N���t�g����:{currentResult.ItemName}");

        resultSlot.SetItem(null);
        currentResult = null;
    }
}
