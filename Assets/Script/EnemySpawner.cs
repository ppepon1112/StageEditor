using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnInfo
    {
        public GameObject dayEnemyPrefab;
        public GameObject nightEnemyPrefab;
        public Vector3 spawnPosition;

        [HideInInspector] public GameObject currentInstance;
    }

    // ï°êîìoò^â¬î\
    public List<SpawnInfo> spawnList = new List<SpawnInfo>();
    public float respawnDelay = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // ç≈èâÇÃÉXÉ|Å[Éì
        foreach(var info in spawnList)
        {
            SpawnEnemy(info);
        }

        if(DayCycle.Instance != null)
        {
            DayCycle.Instance.OnTimePeriodChanged += OnTimePeriodChanged;
        }
    }

    public void OnDestroy()
    {
        if(DayCycle.Instance != null)
        {
            DayCycle.Instance.OnTimePeriodChanged -= OnTimePeriodChanged;
        }
    }

    public void OnTimePeriodChanged(bool isNight)
    {
        foreach(var info in spawnList)
        {
            if(info.currentInstance != null)
            {
                Destroy(info.currentInstance);
                info.currentInstance = null;
            }
            SpawnEnemy(info);
        }
    }

    public void SpawnEnemy(SpawnInfo info)
    {
        GameObject prefabToUse = DayCycle.Instance.IsNight ? info.nightEnemyPrefab : info.dayEnemyPrefab;

        if (prefabToUse == null) return;
        GameObject enemy = Instantiate(prefabToUse, info.spawnPosition, Quaternion.identity);
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if(ai != null)
        {
            ai.groupSpawner = this;
            ai.spawnInfo = info;  // é©ï™ÇÃSpawnInfoÇìnÇ∑
        }
        info.currentInstance = enemy;
    }

    public void OnEnemyKilled(SpawnInfo info)
    {
        info.currentInstance = null;
        StartCoroutine(RespawnEnemy(info));
    }
    IEnumerator RespawnEnemy(SpawnInfo info)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy(info);
    }
}
