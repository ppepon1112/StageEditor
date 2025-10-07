using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    public PlayerStatusData playerStatus;
    public TextMeshProUGUI statusText;
    public KeyCode toggleKey = KeyCode.K;

    private bool isVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        if(statusText != null)
        {
            statusText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isVisible = !isVisible;
            statusText.gameObject.SetActive(isVisible);
            if (isVisible)
            {
                UpdateStatusText();
            }
        }

        if (isVisible)
        {
            UpdateStatusText();
        }
    }

    void UpdateStatusText()
    {
        if (playerStatus != null && statusText != null)
        {
            statusText.text =
                $"<b>STATUS<b>\n" +
                $"Level:{playerStatus.saveData.level}\n" +
                $"HP:{playerStatus.saveData.currentHP}/{playerStatus.saveData.maxHP}\n" +
                $"Experience:{playerStatus.saveData.experience}/{playerStatus.saveData.maxExperience}\n" +
                $"ATK:{playerStatus.saveData.attack}\n" +
                $"DFE:{playerStatus.saveData.defence}";
        }
    }
}
