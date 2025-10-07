using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkillDMG : MonoBehaviour
{
    private int damage;
    private float maxDistance;
    private Vector3 direction;

    public void Init(int dmg,float range,Vector3 dir)
    {
        damage = dmg;
        maxDistance = range;
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"•XƒXƒLƒ‹:{damage}");
            }
        }
    }
}
