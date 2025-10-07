using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpDoor : MonoBehaviour
{
    [Header("遷移先シーン名")]
    public string targetScene = "ShopScene";

    [Header("遷移先でのプレイヤー出現位置")]
    public Vector3 targetPosition = new Vector3(10, 1, 10);

    private bool isWarping = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isWarping) return;
        if (!other.CompareTag("Player")) return;

        isWarping = true;
        Debug.Log($"[WarpDoor] {targetScene}へワープ開始");

        StartCoroutine(WarpProcess());
    }

    private IEnumerator WarpProcess()
    {
        // 今の位置をセーブデータに記録
        if (PlayerStatusData.Instance != null)
        {
            var pos = GameObject.FindWithTag("Player").transform.position;
            PlayerStatusData.Instance.saveData.posX = pos.x;
            PlayerStatusData.Instance.saveData.posY = pos.y;
            PlayerStatusData.Instance.saveData.posZ = pos.z;

            SaveLoadManager.Save(PlayerStatusData.Instance);
        }

        // 次のシーン用にスポーン座標をセット
        PlayerSpawnManager.nextSpawnPosition = targetPosition;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        while (!asyncLoad.isDone)
            yield return null;

        Debug.Log($"[WarpDoor] シーン遷移完了 → {targetScene}");
    }
}
