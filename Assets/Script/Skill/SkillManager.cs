using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skills = new List<SkillData>();

    private Dictionary<SkillType, ISkill> skillHandler;
    private Dictionary<SkillType, float> skillCooldownTimer;

    public PlayerStatusData player;
    public Animator anim;

    // ���ʃC���^�[�o��
    private bool isCasting = false;
    private float globalCooldown = 0.5f;
    private float globalCooldownTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        skillHandler = new Dictionary<SkillType, ISkill>
        {
            { SkillType.Fire, new FireSkill() },
            { SkillType.Ice, new IceSkill() },
        };

        skillCooldownTimer = new Dictionary<SkillType, float>();
        foreach(var skill in skills)
        {
            skillCooldownTimer[skill.type] = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < globalCooldownTimer) return;
        foreach (var skill in skills)
        {
            // �v���C���[���x�����K�v�����𖞂����Ă��Ȃ��ꍇ�̓X�L�b�v
            if (player.saveData.level < skill.requiredLevel)
            {
                continue;
            }

            // ������ꂽ�X�L���͑������p�\�ɂȂ�
            if (Input.GetKeyDown(skill.SkillKey))
            {
                if (isCasting) return;

                if (Time.time < skillCooldownTimer[skill.type])
                {
                    float remain = skillCooldownTimer[skill.type] - Time.time;
                    Debug.Log($"{skill.SkillName} �͂��� {remain:F1} �b�ōĎg�p�\");
                    continue;
                }

                StartCoroutine(CastSkill(skill));
                return;
            }
        }
    }

    private IEnumerator CastSkill(SkillData skill)
    {
        isCasting = true;
        anim.SetTrigger("Skill");

        if (skillHandler.TryGetValue(skill.type, out ISkill handler))
        {
            handler.Activate(gameObject, skill);
            // �N�[���^�C����ݒ�
            skillCooldownTimer[skill.type] = Time.time + skill.cooldown;
        }

        globalCooldownTimer = Time.time + globalCooldown;

        isCasting = false;
        anim.SetTrigger("Cooltime");

        yield return new WaitForSeconds(skill.castTime);
    }
}
