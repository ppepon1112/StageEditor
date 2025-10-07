using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    const string SaveKey = "PLAYER_SAVE";

    public static void Save(PlayerStatusData statusData)
    {
        var player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            Vector3 pos = player.transform.position;
            statusData.saveData.posX = pos.x;
            statusData.saveData.posY = pos.y;
            statusData.saveData.posZ = pos.z;
        }
        string json = JsonUtility.ToJson(statusData.saveData);
        PlayerPrefs.SetString(SaveKey, json);
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log(Application.persistentDataPath);
        Debug.Log("保存完了:" + json);
    }

    public static void Load(PlayerStatusData statusData)
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            JsonUtility.FromJsonOverwrite(json, statusData.saveData);
            Debug.Log(Application.persistentDataPath);
            Debug.Log("読み込み完了:" + json);
        }
        else
        {
            // 初期化＆セーブ(初回起動時)
            Save(statusData);
            Debug.Log("初回起動のため初期値を保存しました");
        }
    }

    [ContextMenu("セーブデータを全削除")]
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("セーブデータ削除しました");
    }
}
