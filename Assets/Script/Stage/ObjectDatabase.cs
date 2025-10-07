using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StageEditor/ObjectDatabase")]

//==============================
// StageEditorの項目にObjectDatabaseを追加
// Assets右クリックでObjectDatabase追加
// 
// 詳細：登録したステージ配置用オブジェクトをリストにまとめて一覧にする
//==============================
public class ObjectDatabase : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;
}
