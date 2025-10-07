using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSlotUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;

    public void SetItem(ItemData item)
    {
        if(item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
            nameText.text = item.name;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            nameText.text = "";
        }
    }
}
