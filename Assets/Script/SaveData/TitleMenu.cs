using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private PlayerStatusData playerStatusData;
    public void OnNewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        playerStatusData.Reset();

        SaveLoadManager.Save(playerStatusData);
        SceneManager.LoadScene("GameScene");
    }

    public void OnContinue()
    {
        if (PlayerPrefs.HasKey("PLAYER_SAVE"))
        {
            SaveLoadManager.Load(playerStatusData);
        }
        else
        {
            SaveLoadManager.Save(playerStatusData);
        }
        SceneManager.LoadScene("GameScene");
    }
}
