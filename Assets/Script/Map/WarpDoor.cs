using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpDoor : MonoBehaviour
{
    [Header("�J�ڐ�V�[����")]
    public string targetScene = "ShopScene";

    [Header("�J�ڐ�ł̃v���C���[�o���ʒu")]
    public Vector3 targetPosition = new Vector3(10, 1, 10);

    private bool isWarping = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isWarping) return;
        if (!other.CompareTag("Player")) return;

        isWarping = true;
        Debug.Log($"[WarpDoor] {targetScene}�փ��[�v�J�n");

        StartCoroutine(WarpProcess());
    }

    private IEnumerator WarpProcess()
    {
        // ���̈ʒu���Z�[�u�f�[�^�ɋL�^
        if (PlayerStatusData.Instance != null)
        {
            var pos = GameObject.FindWithTag("Player").transform.position;
            PlayerStatusData.Instance.saveData.posX = pos.x;
            PlayerStatusData.Instance.saveData.posY = pos.y;
            PlayerStatusData.Instance.saveData.posZ = pos.z;

            SaveLoadManager.Save(PlayerStatusData.Instance);
        }

        // ���̃V�[���p�ɃX�|�[�����W���Z�b�g
        PlayerSpawnManager.nextSpawnPosition = targetPosition;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        while (!asyncLoad.isDone)
            yield return null;

        Debug.Log($"[WarpDoor] �V�[���J�ڊ��� �� {targetScene}");
    }
}
