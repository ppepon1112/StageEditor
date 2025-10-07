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

    [Header("����")]
    public float damage;
    public float range;
    public float deration;
    public float castTime = 1.0f;

    [Header("�N�[���^�C��")]
    public float cooldown = 2.0f;

    [Header("�X�L���������")]
    public int requiredLevel = 1;

    [Header("�G�t�F�N�g/SE")]
    public GameObject MasicCirclePrefab;
    public GameObject effectPrefab;

    public KeyCode SkillKey = KeyCode.Alpha1;
}

public interface ISkill
{
    void Activate(GameObject user, SkillData data);
}
