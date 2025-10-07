using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [Header("ショップシーンの初期スポーン位置（デフォルト）")]
    public Vector3 defaultSpawnPosition = new Vector3(0, 0, 0);

    void Start()
    {
        // シーン遷移時に残っているプレイヤーを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPos = PlayerWarpData.GetPositionOrDefault(defaultSpawnPosition);
            player.transform.position = spawnPos;
            PlayerWarpData.Clear(); // 一度使ったらリセット
            Debug.Log($"プレイヤーをショップの位置 {spawnPos} に移動しました");
        }
        else
        {
            Debug.LogWarning("ショップシーンにプレイヤーが見つかりません。");
        }
    }
}
