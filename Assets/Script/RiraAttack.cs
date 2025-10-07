using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class RiraAttack : MonoBehaviour
{
    public GameObject bulletPrefab; // �e�̃v���n�u
    public Transform firePoint; // �e�̔��ˈʒu
    public float fireRate = 0.5f; // �A�ˑ��x
    public Animator anim;
    private SwordColliderController sword;
    public bool isAttacking = false;
    public float attackCooldown = 1.0f;
    private float nextAttackTime = 0f;
    private RiraMove moveScript;
    int weaponDamage = 2;
    public PlayerStatusData statusData;

    void Start()
    {
        anim = GetComponent<Animator>();
        moveScript = GetComponent<RiraMove>();
        if (sword == null)
        {
            sword = GetComponentInChildren<SwordColliderController>();
        }
        if (anim == null) Debug.LogWarning("Animator not found on Player.");
        if (sword == null) Debug.LogWarning("SwordColliderController not found on child.");
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextAttackTime && !isAttacking) // ���N���b�N�ōU��
        {
            StartCoroutine(AttackRoutine());
        }
    }

    public void SetDamage(int damage)
    {
        weaponDamage = damage;

        // �Ď擾�����݂�(����؂�ւ���)
        sword = GetComponentInChildren<SwordColliderController>();
        if(sword == null)
        {
            Debug.LogWarning("�����SwoedColliderController��������܂���");
        }
    }

    public void SetSword(SwordColliderController newSword)
    {
        sword = newSword;

    }
    // ===�y�U���͏����z===
    // ��b�U���� + ����U���� = �ŏI�U����
    // �ŏI�U���� �} 10%�����_��
    private int GetFinalDamage()
    {
        int baseAttack = statusData.saveData.attack;
        int total = baseAttack + weaponDamage;

        float randFactor = UnityEngine.Random.Range(0.9f, 1.1f);
        total = Mathf.RoundToInt(total * randFactor);

        return Mathf.Max(1, total);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        moveScript.isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.2f);
        if(sword != null)
        {
            // ���킪����
            int finalDamage = GetFinalDamage();
            sword.EnableDamage(finalDamage);
            Debug.Log($"�U����:��b{statusData.saveData.attack} + ����{weaponDamage} => �ŏI{finalDamage}"); ;
        }
        else
        {
            // �f��U���p�̓����蔻��
            TryUnarmedAttack();
        }

        yield return new WaitForSeconds(0.85f);
        if(sword != null)
        {
            sword.DisableDamage();
        }

        anim.SetTrigger("Cooltime");

        moveScript.isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    void TryUnarmedAttack()
    {
        float radius = 1.0f;
        float range = 2f;
        //float damage = 2;

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;
        if(Physics.SphereCast(origin,radius,direction,out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    int finalDamage = GetFinalDamage();
                    enemy.TakeDamage(finalDamage);
                    Debug.Log($"�f���{finalDamage}�_���[�W�^�����I");
                }
            }
        }
        Debug.DrawRay(origin, direction * range, Color.red, 0.5f);
    }
}
