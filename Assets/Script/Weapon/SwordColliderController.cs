using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderController : MonoBehaviour
{
    public Collider weaponCollider;
    public TrailRenderer trailRenderer;
    int damage = 10;
    private bool hasHit = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!weaponCollider) weaponCollider = GetComponent<Collider>();

        if(weaponCollider != null)
            weaponCollider.enabled = false;
        if(!trailRenderer) trailRenderer = GetComponent<TrailRenderer>();
        
        if(trailRenderer != null)
            trailRenderer.enabled = false;
    }

    public void EnableDamage(int dmg)
    {
        damage = dmg;
        hasHit = false;
        if(weaponCollider != null)
            weaponCollider.enabled = true;
        if(trailRenderer != null)
            trailRenderer.enabled = true;
    }
    public void DisableDamage()
    {
        if(weaponCollider != null)
            weaponCollider.enabled = false;
        if (trailRenderer != null)
            trailRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                hasHit = true;
            }
        }
    }
}
