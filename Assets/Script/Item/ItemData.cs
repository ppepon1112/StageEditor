using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Consumable,
    Weapon,
    //KeyItem,
}

[CreateAssetMenu(menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string ItemID;
    public ItemType type;
    public Sprite icon;
    public GameObject worldPrefab; // ƒhƒƒbƒv‚ÌŒ©‚½–Ú
    public GameObject equipPrefab;

    [TextArea]
    public string description;

    public int healAmount;
    public int attackBoost;
    [Header("‘•”õ‚Ì•â³")]
    public Vector3 equipPosition = Vector3.zero;
    public Vector3 equipRotation = Vector3.zero;
}
