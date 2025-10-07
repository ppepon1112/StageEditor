using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEncounterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerPos = other.transform.position;
            PlayerPrefs.SetFloat("LastPosX", playerPos.x);
            PlayerPrefs.SetFloat("LastPosY", playerPos.y);
            PlayerPrefs.SetFloat("LastPosZ", playerPos.z);
            PlayerPrefs.Save();

            // 戦闘相手の情報を保存（敵名：ID）
            PlayerPrefs.SetString("EncounterEnemy", gameObject.name);

            // 戦闘シーンへ移動
            //SceneManager.LoadScene("BattleScene");
            Debug.Log("BattleSceneへ移動");
        }
    }
}
