using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Fire,
    Ice,
    Lightning,
    Water,
    Wind,
    Ground,
    Buff,
}
[CreateAssetMenu(menuName = "Game/Skill Data")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public SkillType type;
    public Sprite icon;
    public string description;

    [Header("効果")]
    public float damage;
    public float range;
    public float deration;
    public float castTime = 1.0f;

    [Header("クールタイム")]
    public float cooldown = 2.0f;

    [Header("スキル解放条件")]
    public int requiredLevel = 1;

    [Header("エフェクト/SE")]
    public GameObject MasicCirclePrefab;
    public GameObject effectPrefab;

    public KeyCode SkillKey = KeyCode.Alpha1;
}

public interface ISkill
{
    void Activate(GameObject user, SkillData data);
}
