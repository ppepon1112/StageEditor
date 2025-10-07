using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMover : MonoBehaviour
{
    public float speed = 10f;
    private int damage;
    private float maxDistance;
    private Vector3 direction;
    private Vector3 startPos;

    private bool isReady = false;
    private bool hasHit = false;
    private Vector3 targetScale;

    [Header("���G�t�F�N�g")]
    public GameObject smokePrefab;
    public float smokeInterval = 0.1f;
    private float smokeTimer = 0f;

    public void Init(int dmg, float range, Vector3 dir)
    {
        damage = dmg;
        maxDistance = range;
        direction = dir.normalized;
        startPos = transform.position;
        
        // �I�u�W�F�N�g�̃X�P�[����ۑ�&�X�P�[��0,0,0�Ƀ��Z�b�g
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;

        StartCoroutine(ScaleUpAndLaunch());
    }

    private IEnumerator ScaleUpAndLaunch()
    {
        float duration = 1f;
        float t = 0f;

        // 1�b������0,0,0����targetScale�܂Ŋg��
        while(t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, lerp);
            yield return null;
        }
        // targetScale�̃X�P�[���ɒB�����甭��
        transform.localScale = targetScale;
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady || hasHit) return;

        transform.position += direction * speed * Time.deltaTime;

        smokeTimer += Time.deltaTime;
        if(smokePrefab != null && smokeTimer >= smokeInterval)
        {
            smokeTimer = 0f;
            GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
            Destroy(smoke, 2f);
        }

        if(Vector3.Distance(startPos,transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"�΂̋ʂ��G�ɖ����I {damage} �_���[�W");
            }
            Destroy(gameObject); // ������͏�����
        }
    }
}
