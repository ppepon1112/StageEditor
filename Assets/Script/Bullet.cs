using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public float speed = 10f;
    //public float lifetime = 3f;
    //public int damage = 10;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    Destroy(gameObject, lifetime);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        EnemyAI enemy = other.GetComponent<EnemyAI>();
    //        if(enemy != null)
    //        {
    //            enemy.TakeDamage(damage);
    //        }
    //        Destroy(gameObject);
    //    }
    //    else if (!other.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //    }

    //    //IDamageable damageable = other.GetComponent<IDamageable>();
    //    //if(damageable != null)
    //    //{
    //    //    damageable.TakeDamage(damage);
    //    //}
    //    //Destroy(gameObject);
    //}

    //public interface IDamageable
    //{
    //    void TakeDamage(int amount);
    //}
}
