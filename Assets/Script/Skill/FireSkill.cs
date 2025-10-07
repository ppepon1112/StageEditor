using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : ISkill
{
    public void Activate(GameObject user, SkillData data)
    {
        user.GetComponent<MonoBehaviour>().StartCoroutine(CastSkill(user, data));
    }

    private IEnumerator CastSkill(GameObject user, SkillData data)
    {
        var move = user.GetComponent<RiraMove>();
        move.isAttacking = true;

        // 魔法陣
        if (data.MasicCirclePrefab != null)
        {
            Debug.Log("魔法陣発動");
            GameObject circle = GameObject.Instantiate(data.MasicCirclePrefab,
                user.transform.position, Quaternion.identity);
            GameObject.Destroy(circle, 5f);
        }

        yield return new WaitForSeconds(0.3f);

        // エフェクト
        if (data.effectPrefab != null)
        {
            Debug.Log("魔法発動");
            Vector3 spawnPos = user.transform.position + Vector3.up * 0.75f + user.transform.forward * 0.5f;
            GameObject effect = GameObject.Instantiate(
                data.effectPrefab,
                spawnPos,
                user.transform.rotation
            );

            FireBallMover mover = effect.GetComponent<FireBallMover>();
            if(mover == null)
            {
                mover = effect.AddComponent<FireBallMover>();
            }
            mover.Init((int)data.damage, data.range, user.transform.forward);


            GameObject.Destroy(effect, 5f);
        }

        yield return new WaitForSeconds(data.castTime);
        move.isAttacking = false;
    }
}
