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

            // �퓬����̏���ۑ��i�G���FID�j
            PlayerPrefs.SetString("EncounterEnemy", gameObject.name);

            // �퓬�V�[���ֈړ�
            //SceneManager.LoadScene("BattleScene");
            Debug.Log("BattleScene�ֈړ�");
        }
    }
}
