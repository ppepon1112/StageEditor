using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceSkill : ISkill
{
    public void Activate(GameObject user,SkillData data)
    {
        user.GetComponent<MonoBehaviour>().StartCoroutine(CastSkill(user, data));
    }

    private IEnumerator CastSkill(GameObject user, SkillData data)
    {
        var move = user.GetComponent<RiraMove>();
        move.isAttacking = true;
        if (data.MasicCirclePrefab != null)
        {
            GameObject circle = GameObject.Instantiate(
                data.MasicCirclePrefab, 
                user.transform.position, 
                Quaternion.identity
            );
            GameObject.Destroy(circle, 5f);
        }
        yield return new WaitForSeconds(0.5f);

        Vector3 forward = user.transform.forward;
        Vector3 spawnPos = user.transform.position + Vector3.up * 1f;

        if (data.effectPrefab != null)
        {
            // === 小 ===
            Vector3 posSmall = GetGroundPosition(spawnPos + forward * 1.5f,0.3f);
            GameObject small = GameObject.Instantiate(
                data.effectPrefab,
                posSmall,
                Quaternion.LookRotation(forward)
            );
            small.transform.localScale = Vector3.one * 0.5f;
            IceSkillDMG smallDMG = small.GetComponent<IceSkillDMG>();
            if(smallDMG == null)
            {
                smallDMG = small.AddComponent<IceSkillDMG>();
            }
            smallDMG.Init((int)data.damage / 4, data.range, user.transform.forward);
            GameObject.Destroy(small, 5f);
            yield return new WaitForSeconds(0.2f);

            // === 中 ===
            Vector3 posMedium = GetGroundPosition(spawnPos + forward * 3f,0.5f);
            GameObject medium = GameObject.Instantiate(
                data.effectPrefab,
                posMedium,
                Quaternion.LookRotation(forward)
            );
            medium.transform.localScale = Vector3.one * 1f;
            IceSkillDMG mediumDMG = medium.GetComponent<IceSkillDMG>();
            if (mediumDMG == null)
            {
                mediumDMG = medium.AddComponent<IceSkillDMG>();
            }
            mediumDMG.Init((int)data.damage / 2, data.range, user.transform.forward);

            GameObject.Destroy(medium, 5f);
            yield return new WaitForSeconds(0.2f);

            // === 大 ===
            Vector3 posLarge = GetGroundPosition(spawnPos + forward * 4.5f,0.8f);
            GameObject large = GameObject.Instantiate(
                data.effectPrefab,
                posLarge,
                Quaternion.LookRotation(forward)
            );
            large.transform.localScale = Vector3.one * 1.5f;

            IceSkillDMG skillDMG = large.GetComponent<IceSkillDMG>();
            if(skillDMG == null)
            {
                skillDMG = large.AddComponent<IceSkillDMG>();
            }
            skillDMG.Init((int)data.damage, data.range, user.transform.forward);

            GameObject.Destroy(large, 5f);
        }

        yield return new WaitForSeconds(data.castTime);
        move.isAttacking = false;
    }

    /// <summary>
    /// 渡された位置の真下にレイキャストして地面の高さを取得
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    private Vector3 GetGroundPosition(Vector3 startPos,float offsetY = 0.5f)
    {
        Ray ray = new Ray(startPos, Vector3.down);
        if(Physics.Raycast(ray,out RaycastHit hit, 10f))
        {
            return hit.point + Vector3.up * offsetY;
        }
        return startPos;
    }
}
