using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject hpBarPrefab;
    public EnemyData data;
    private int attackPower;
    private int defencePower;
    private int experience;
    private float moveSpeed;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float detectionRange = 5f;
    private int currentHP;


    private Transform player;
    private NavMeshAgent agent;
    private Slider hpSlider;
    private float lastAttackTime;
    private bool playerDetected = false;
    private Animator animator;

    // ---2025/06/18---
    public EnemySpawner groupSpawner;
    public EnemySpawner.SpawnInfo spawnInfo;
    // ---2025/07/11---
    public ItemData dropItem; // 1��
    public GameObject itemDropPrefab;
    private GameObject hpBarObj;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(hpBarPrefab != null)
        {
            hpBarObj = Instantiate(hpBarPrefab);
            hpBarObj.transform.SetParent(null);
            hpBarObj.transform.position = transform.position + new Vector3(0, 2f, 0);
            hpBarObj.transform.localScale = Vector3.one;

            hpSlider = hpBarObj.GetComponentInChildren<Slider>();
            HPBerFollow follow = hpBarObj.GetComponent<HPBerFollow>();
            follow.target = this.transform;
        }

        if (data != null)
        {
            attackPower = data.attack;
            defencePower = data.defense;
            experience = data.experience;
            moveSpeed = data.moveSpeed;
            currentHP = data.hp;
            hpSlider.maxValue = currentHP;
            hpSlider.value = data.hp;

            if(hpSlider != null)
            {
                hpSlider.maxValue = currentHP;
                hpSlider.value = currentHP;
            }
            agent.speed = moveSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || currentHP <= 0) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!playerDetected)
        {
            if(distance <= detectionRange)
            {
                animator.SetBool("isWalking", true);
                playerDetected = true;
                Debug.Log($"{data.enemyName}�F�v���C���[�𔭌��I");
            }
            else
            {
                animator.SetBool("isWalking", false);
                return;
            }
        }
        // �v���C���[�܂ňړ�
        if(distance > attackRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
        else
        {
            // �͈͍U���Ȃ�~�܂��čU��
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
            Attack();
        }
    }

    void Attack()
    {
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
            Debug.Log("�v���C���[�ɍU���I");
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackPower);
            animator.SetTrigger("Cooltime");
        }
    }
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Damage");

        // ---���ۂɗ^����_���[�W���v�Z---
        int actualDamage = damage - defencePower;

        if(actualDamage <= 0)
        {
            // �h��͂������Ă���ꍇ��1�`10�̃����_���_���[�W
            actualDamage = UnityEngine.Random.Range(1, 11);
        }
        else
        {
            // �}10%�̕␳������
            float randFactor = UnityEngine.Random.Range(0.9f, 1.1f);
            actualDamage = Mathf.RoundToInt(actualDamage * randFactor);
            actualDamage = Mathf.Max(1, actualDamage);
        }

        currentHP -= actualDamage;
        currentHP = Mathf.Clamp(currentHP, 0, data.hp);
        Debug.Log($"{data.enemyName}��{actualDamage}�_���[�W�󂯂��I�c��HP{currentHP}");
        ShowDamageText(actualDamage);

        if(hpSlider != null) hpSlider.value = currentHP;
        if (currentHP <= 0)
        {
            animator.SetTrigger("Die");
            Die();
        }
    }

    public void Die()
    {
        if(groupSpawner != null && spawnInfo != null)
        {
            groupSpawner.OnEnemyKilled(spawnInfo);
        }

        // �v���C���[�Ɍo���l��n��
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Experience(data.experience);
        }

        // �A�C�e���h���b�v
        if (dropItem != null && itemDropPrefab != null)
        {
            Vector3 dropPosition = transform.position + new Vector3(0, 1.5f, 0);
            GameObject drop = Instantiate(itemDropPrefab, dropPosition, Quaternion.identity);
            ItemDrop itemDrop = drop.GetComponent<ItemDrop>();
            if (itemDrop != null)
            {
                itemDrop.itemData = dropItem;
            }
        }
        if (hpBarObj != null) Destroy(hpBarObj);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void ShowDamageText(int damage)
    {
        Vector3 spawnPos = transform.position + Vector3.up * 2f;
        GameObject obj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
        obj.GetComponent<DamageText>().SetUp(damage);
    }

    private void OnDestroy()
    {
        if(hpBarObj != null) Destroy (hpBarObj);
    }
}
