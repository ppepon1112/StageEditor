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
        Debug.Log("�ۑ�����:" + json);
    }

    public static void Load(PlayerStatusData statusData)
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            JsonUtility.FromJsonOverwrite(json, statusData.saveData);
            Debug.Log(Application.persistentDataPath);
            Debug.Log("�ǂݍ��݊���:" + json);
        }
        else
        {
            // ���������Z�[�u(����N����)
            Save(statusData);
            Debug.Log("����N���̂��ߏ����l��ۑ����܂���");
        }
    }

    [ContextMenu("�Z�[�u�f�[�^��S�폜")]
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("�Z�[�u�f�[�^�폜���܂���");
    }
}
