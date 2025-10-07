using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StatusEditor/EnemyData")]

public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int hp;
    public int attack;
    public int defense;
    public int experience;
    public float moveSpeed;
    public Sprite icon;
    public GameObject modelPrefab;
    public string[] abilities;
}
