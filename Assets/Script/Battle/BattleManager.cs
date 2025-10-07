using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public void EndBattle()
    {
        // ���̈ʒu��ǂݍ���
        float x = PlayerPrefs.GetFloat("LastPosX", 0f);
        float y = PlayerPrefs.GetFloat("LastPosY", 0f);
        float z = PlayerPrefs.GetFloat("LastPosZ", 0f);

        SceneManager.LoadScene("GameScene");

        PlayerPrefs.SetFloat("SpawnX", x);
        PlayerPrefs.SetFloat("SpawnY", y);
        PlayerPrefs.SetFloat("SpawnZ", z);
    }
}
