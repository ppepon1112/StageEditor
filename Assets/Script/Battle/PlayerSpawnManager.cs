using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }
    public static Vector3? nextSpawnPosition; // ���[�v����W��ێ�
    public GameObject playerPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // �V�[�����܂����ł����̃}�l�[�W���[�͕ێ�
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

        // �D�揇�ʁFnextSpawnPosition �� �Z�[�u�f�[�^���W �� ���_
        if (nextSpawnPosition.HasValue)
        {
            spawnPos = nextSpawnPosition.Value;
            Debug.Log($"[SpawnManager] nextSpawnPosition ���g�p: {spawnPos}");
        }
        else if (PlayerStatusData.Instance != null)
        {
            spawnPos = new Vector3(
                PlayerStatusData.Instance.saveData.posX,
                PlayerStatusData.Instance.saveData.posY,
                PlayerStatusData.Instance.saveData.posZ
            );
            Debug.Log($"[SpawnManager] �Z�[�u�f�[�^���W���g�p: {spawnPos}");
        }
        else
        {
            spawnPos = Vector3.zero;
            Debug.LogWarning("[SpawnManager] �Z�[�u�f�[�^���������A���_�ɃX�|�[�����܂�");
        }

        if (player == null)
        {
            // �v���C���[�����݂��Ȃ� �� ����
            player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            DontDestroyOnLoad(player);
            Debug.Log($"[SpawnManager] �v���C���[���� @ {spawnPos}");
        }
        else
        {
            // �����v���C���[���ړ�
            player.transform.position = spawnPos;
            Debug.Log($"[SpawnManager] �����v���C���[���ړ� @ {spawnPos}");
        }

        // �J�����Ǐ]�ݒ�
        var cam = FindObjectOfType<PlayerCamera>();
        if (cam != null)
        {
            cam.target = player.transform;
            Debug.Log("[SpawnManager] �J�����^�[�Q�b�g�ݒ芮��");
        }
    }
}
