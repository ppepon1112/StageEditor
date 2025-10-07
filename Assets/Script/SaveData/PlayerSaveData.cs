using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int level;
    public int maxExperience;
    public int experience;
    public int maxHP;
    public int currentHP;
    public int attack;
    public int defence;
    public List<string> clearedStageID = new();
    public List<ItemEntry> ownedItems = new();

    public float posX;
    public float posY;
    public float posZ;

    public string lastScene = "GameScene";
}
