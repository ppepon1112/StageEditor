using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

//==============================
// 詳細：編集するステージを作成
//==============================
public class StageRoot : MonoBehaviour
{
    public ObjectDatabase objectDatabase;
    public EnemyData selectedEnemyData;

    [System.Serializable]
    public class EnemyInstanceData
    {
        public EnemyData enemyData;
        public Vector3 position;
    }

    public List<EnemyInstanceData> enemies = new List<EnemyInstanceData>();

    public void AddEnemy(EnemyData data, Vector3 position)
    {
        enemies.Add(new EnemyInstanceData
        {
            enemyData = data,
            position = position
        });

        GameObject enemyObj = Instantiate(data.modelPrefab, position, Quaternion.identity, this.transform);
        enemyObj.name = data.enemyName;

        EnemyAI ai = enemyObj.GetComponent<EnemyAI>();
        if(ai != null)
        {
            ai.data = data;
        }
    }
    public void RemoveEnemy(EnemyData data, Vector3 position)
    {
        // EnemyDataが一致する最初の要素を削除
        var target = enemies.FirstOrDefault(e => e.enemyData == data && Vector3.Distance(e.position,position) < 0.1f);
        if (target != null)
        {
            enemies.Remove(target);
        }
    }
}
