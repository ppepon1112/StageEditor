using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarrir : MonoBehaviour
{
    public PlayerHealth health;

    public GameObject barrirEffectPrefab;
    private GameObject activeBarrirEffect;

    public float barrirDuration = 10f;
    public KeyCode barrirKey = KeyCode.B;
    private bool isInvincible = false;
    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        {
            health = GetComponent<PlayerHealth>();
            if(health == null)
            {
                Debug.LogWarning("PlayerHealth script not found on" + gameObject.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(barrirKey) && !isInvincible)
        {
            StartCoroutine(UseBarrirSkill());
        }
    }

    IEnumerator UseBarrirSkill()
    {
        isInvincible = true;

        health.SetInvincible(true);

        if(barrirEffectPrefab != null && activeBarrirEffect == null)
        {
            activeBarrirEffect = Instantiate(barrirEffectPrefab, new Vector3(transform.position.x,transform.position.y + 0.5f,transform.position.z), Quaternion.identity,transform);
        }

        yield return new WaitForSeconds(barrirDuration);

        isInvincible = false;
        health.SetInvincible(false);

        if(activeBarrirEffect != null)
        {
            Destroy(activeBarrirEffect);
            activeBarrirEffect = null;
        }
    }
}
