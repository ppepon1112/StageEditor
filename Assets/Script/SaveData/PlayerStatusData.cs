using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObjectでUnity上でインスペクタに表示・編集
/// </summary>
[CreateAssetMenu(menuName = "Game/Player Status Data")]
public class PlayerStatusData : ScriptableObject
{
    public static PlayerStatusData Instance;
    public PlayerSaveData saveData = new PlayerSaveData();
    //public List<string> ownedItem = new List<string>();

    void OnEnable()
    {
        if(saveData == null)
        {
            saveData = new PlayerSaveData();
        }
    }

    public void Reset()
    {
        saveData = new PlayerSaveData();

        saveData.level = 1;
        saveData.maxExperience = 100;
        saveData.experience = 0;
        saveData.maxHP = 100;
        saveData.currentHP = 100;
        saveData.attack = 10;
        saveData.defence = 10;
        saveData.posX = 3f;
        saveData.posY = 0.5f;
        saveData.posZ = 2f;

        saveData.ownedItems = new List<ItemEntry>();
        saveData.clearedStageID = new List<string>();
    }
}
