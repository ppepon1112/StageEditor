using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [Header("�V���b�v�V�[���̏����X�|�[���ʒu�i�f�t�H���g�j")]
    public Vector3 defaultSpawnPosition = new Vector3(0, 0, 0);

    void Start()
    {
        // �V�[���J�ڎ��Ɏc���Ă���v���C���[��T��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPos = PlayerWarpData.GetPositionOrDefault(defaultSpawnPosition);
            player.transform.position = spawnPos;
            PlayerWarpData.Clear(); // ��x�g�����烊�Z�b�g
            Debug.Log($"�v���C���[���V���b�v�̈ʒu {spawnPos} �Ɉړ����܂���");
        }
        else
        {
            Debug.LogWarning("�V���b�v�V�[���Ƀv���C���[��������܂���B");
        }
    }
}
