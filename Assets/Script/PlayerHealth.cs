using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̗͊Ǘ�
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    // �_���[�W�\���EHP�o�[�E�o���l�o�[
    public GameObject damageTextPrefab;
    public Slider HPBar;
    public Slider ExperienceBar;
    public TMP_Text HPText;
    public TMP_Text ExperienceText;
    // HP,�o���l,�h��͏����l
    public int maxHP = 100;
    public int experience = 0;
    public int maxExperience = 100;
    public int level = 1;
    public int currentHP;
    public int defence;
    public int attack;
    // �X�e�[�^�X�f�[�^�A�C���x���g��
    public PlayerStatusData statusData;
    private PlayerInventory inventory;
    private bool invincible = false;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("�ۑ�����");

        // �X�e�[�^�X�f�[�^�̓ǂݍ��݁A�ŏI�ʒu�̎擾
        SaveLoadManager.Load(statusData);

        if (!PlayerWarpData.isWarping)
        {
            Vector3 savePos = new Vector3(
                statusData.saveData.posX,
                statusData.saveData.posY,
                statusData.saveData.posZ
            );
            transform.position = savePos;
        }
        else
        {
            Debug.Log("���[�v���̂��߃Z�[�u�f�[�^�ʒu�𖳎����܂���");
        }
        // �Z�[�u�f�[�^���畜��
        level = statusData.saveData.level;
        currentHP = statusData.saveData.currentHP;
        maxHP = statusData.saveData.maxHP;
        experience = statusData.saveData.experience;
        maxExperience = statusData.saveData.maxExperience;
        defence = statusData.saveData.defence;
        attack = statusData.saveData.attack;
        if (HPBar != null)
        {
            HPBar.maxValue = statusData.saveData.maxHP;
            HPBar.value = statusData.saveData.currentHP;
        }
        if(ExperienceBar != null)
        {
            ExperienceBar.maxValue = statusData.saveData.maxExperience;
            ExperienceBar.value = statusData.saveData.experience;
        }
        StartCoroutine(AssignUIReferences());
        // �����\��
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveLoadManager.Save(statusData);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadManager.Load(statusData);
            Debug.Log("���݂�HP: " + statusData.saveData.currentHP);
        }
        // ---HP��---
        // Potion_001�������Ă���ꍇ����ĉ�
        // �񕜗ʂ�HealBoost��
        if (Input.GetKeyDown(KeyCode.H))
        {
            bool used = inventory.UseItem("Potion_001");
        }
    }

    // ===�yUI�X�V�p�z===
    private void UpdateUI()
    {
        if (HPBar != null)
        {
            HPBar.maxValue = statusData.saveData.maxHP;
            HPBar.value = statusData.saveData.currentHP;
        }
        if (ExperienceBar != null)
        {
            ExperienceBar.maxValue = statusData.saveData.maxExperience;
            ExperienceBar.value = statusData.saveData.experience;
        }
        if (HPText != null)
        {
            HPText.text = $"{statusData.saveData.currentHP} / {statusData.saveData.maxHP}";
        }
        if(ExperienceText != null)
        {
            ExperienceText.text = $"{statusData.saveData.experience} / {statusData.saveData.maxExperience}";
        }
    }

    // ===�y�񕜁z===
    // ���݂�HP�ɃA�C�e���̉񕜗ʂ����Z
    // �Z�[�u�f�[�^�ɂ��ۑ�
    public void Heal(int amount)
    {
        statusData.saveData.currentHP = Mathf.Min(statusData.saveData.currentHP + amount, statusData.saveData.maxHP);
        currentHP = statusData.saveData.currentHP;
        if(HPBar != null)
        {
            HPBar.value = currentHP;
        }
        UpdateUI();
        Debug.Log($"HP��:{statusData.saveData.currentHP}/{statusData.saveData.maxHP}");
    }
    // ===�y�o���l�z===
    // ���݂̌o���l�ɐV���Ɏ擾�����o���l�����Z
    // �Z�[�u�f�[�^�ɂ��ۑ�
    public void Experience(int amount)
    {
        // �Z�[�u�f�[�^�̍ő�o���l�̒l�������ĂȂ���Β��~
        if(statusData.saveData.maxExperience <= 0)
        {
            Debug.LogError("maxExperience��0�ȉ��̂��ߌo���l�v�Z�𒆎~");
            return;
        }
        // �o���l�̎擾
        statusData.saveData.experience += amount;
        Debug.Log($"�o���l +{amount}(����:{statusData.saveData.experience}/{statusData.saveData.maxExperience}");
        // �o���l��maxExperience�ȏ�ɂȂ�΃��x���A�b�v
        while(statusData.saveData.experience >= statusData.saveData.maxExperience)
        {
            statusData.saveData.experience -= statusData.saveData.maxExperience;
            LevelUp();
        }
        UpdateUI();
    }

    // ===�y���x���A�b�v�z===
    // ���x���A�b�v���ɃX�e�[�^�X������
    // HP,�o���l����̉��(HP�̍ő�l+20,�o���l���1.5�{)
    // HP�̑S��
    private void LevelUp()
    {
        statusData.saveData.level++;
        statusData.saveData.maxHP += 20;
        statusData.saveData.currentHP = statusData.saveData.maxHP;
        statusData.saveData.maxExperience = Mathf.RoundToInt(statusData.saveData.maxExperience * 1.5f);
        statusData.saveData.attack = Mathf.RoundToInt(statusData.saveData.attack * 1.2f);
        statusData.saveData.defence = Mathf.RoundToInt(statusData.saveData.attack * 1.1f);

        UpdateUI();
        Debug.Log($"���x�����オ�����I���݂̃��x��:{statusData.saveData.level},���̕K�v�o���l:{statusData.saveData.maxExperience}");
    }
    // ===�y�o���A��Ԃ̔��f�z===
    public void SetInvincible(bool value)
    {
        invincible = value;
    }
    // ===�y�_���[�W�����z===
    // ����HP����(�G����̃_���[�W�� - �h���)���̃_���[�W���󂯂�
    // �o���A���̓_���[�W����
    public void TakeDamage(int damage)
    {
        if (invincible)
        {
            Debug.Log("���G���Ȃ̂Ń_���[�W0");
            return;
        }

        int actualDamage = damage - statusData.saveData.defence;

        if(actualDamage <= 0)
        {
            actualDamage = UnityEngine.Random.Range(1, 11);
        }
        else
        {
            float randFactor = UnityEngine.Random.Range(0.9f, 1.1f);
            actualDamage = Mathf.RoundToInt(actualDamage * randFactor);
            actualDamage = Mathf.Max(1, actualDamage);
        }

        statusData.saveData.currentHP -= actualDamage;
        currentHP = Mathf.Clamp(statusData.saveData.currentHP, 0, statusData.saveData.maxHP);
        
        UpdateUI();
        Debug.Log($"�v���C���[��{actualDamage}�_���[�W���󂯂��B�c��HP�F{currentHP}");

        ShowDamageText(actualDamage);

        if(currentHP <= 0)
        {
            Die();
        }
    }

    // ===�y���S�����z===
    void Die()
    {
        Debug.Log("�v���C���[���S");
        Destroy(gameObject);
    }

    void ShowDamageText(int damage)
    {
        Vector3 spawnPos = transform.position + Vector3.up * 2f;
        GameObject obj = Instantiate(damageTextPrefab,spawnPos, Quaternion.identity);
        obj.GetComponent<DamageText>().SetUp(damage);
    }
    private IEnumerator AssignUIReferences()
    {
        yield return null; // 1�t���[���҂�

        if (HPBar == null)
            HPBar = GameObject.Find("PlayerHPBar")?.GetComponent<Slider>();

        if (ExperienceBar == null)
            ExperienceBar = GameObject.Find("ExperienceBar")?.GetComponent<Slider>();

        if (HPText == null)
            HPText = GameObject.Find("HPText")?.GetComponent<TMP_Text>();

        if (ExperienceText == null)
            ExperienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();

        if (HPBar == null || ExperienceBar == null)
        {
            Debug.LogWarning("[PlayerHealth] UI�v�f��������܂���BCanvas�̃I�u�W�F�N�g�����m�F���Ă��������B");
        }
        else
        {
            Debug.Log("[PlayerHealth] UI�Q�Ƃ��������蓖�Ă��܂����B");
            UpdateUI();
        }
    }
    void OnApplicationQuit()
    {
        SaveLoadManager.Save(statusData);
    }
}
