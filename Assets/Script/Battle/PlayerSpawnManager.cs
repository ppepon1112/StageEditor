using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }
    public static Vector3? nextSpawnPosition; // ワープ先座標を保持
    public GameObject playerPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // シーンをまたいでもこのマネージャーは保持
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnOrMovePlayer();
    }
    private void Start()
    {
        SpawnOrMovePlayer();
    }

    private void SpawnOrMovePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 spawnPos;

        // 優先順位：nextSpawnPosition → セーブデータ座標 → 原点
        if (nextSpawnPosition.HasValue)
        {
            spawnPos = nextSpawnPosition.Value;
            Debug.Log($"[SpawnManager] nextSpawnPosition を使用: {spawnPos}");
        }
        else if (PlayerStatusData.Instance != null)
        {
            spawnPos = new Vector3(
                PlayerStatusData.Instance.saveData.posX,
                PlayerStatusData.Instance.saveData.posY,
                PlayerStatusData.Instance.saveData.posZ
            );
            Debug.Log($"[SpawnManager] セーブデータ座標を使用: {spawnPos}");
        }
        else
        {
            spawnPos = Vector3.zero;
            Debug.LogWarning("[SpawnManager] セーブデータ未初期化、原点にスポーンします");
        }

        if (player == null)
        {
            // プレイヤーが存在しない → 生成
            player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            DontDestroyOnLoad(player);
            Debug.Log($"[SpawnManager] プレイヤー生成 @ {spawnPos}");
        }
        else
        {
            // 既存プレイヤーを移動
            player.transform.position = spawnPos;
            Debug.Log($"[SpawnManager] 既存プレイヤーを移動 @ {spawnPos}");
        }

        // カメラ追従設定
        var cam = FindObjectOfType<PlayerCamera>();
        if (cam != null)
        {
            cam.target = player.transform;
            Debug.Log("[SpawnManager] カメラターゲット設定完了");
        }
    }
}
