using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StageEditor/PlaceableObject")]

//==============================
// StageEditorの項目を追加
// StageEditorの項目にPlaceableObjectを追加
// Assets右クリックでPlaceableObject追加
// toolタグを追加
// EnemyDataに夜敵オブジェクト
// 
// 詳細：ステージ配置用オブジェクト(プレハブ)を登録
//==============================
public class PlaceableObject : ScriptableObject
{
    public string objectName;
    public GameObject prefab;
}
