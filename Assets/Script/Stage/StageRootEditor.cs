#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(StageRoot))]

//==============================
// Unityエディタ拡張ツール：ステージ編集用
// ? ObjectDataBaseからプレハブを選択して配置
// ? Ctrl+左クリック：Terrain上 or 他オブジェクトの面上に配置(※面配置は効果がCubeに限定されるためモデル優先にし廃止)
// ? Ctrl+右クリック：配置オブジェクトを削除
// ? EnemyDataをScriptableObjectで管理し、シーンに敵を配置
// ? Alt+左クリック：敵を配置(StageRootに登録)
// ? Alt+右クリック：敵を削除(リストからも削除)
//==============================
public class StageRootEditor : Editor
{
    private int selectedIndex = 0;
    private float rotationY = 0f;

    public override void OnInspectorGUI()
    {
        StageRoot stage = (StageRoot)target;
        // --- エディタ初期チェック ---
        if(stage.objectDatabase == null)
        {
            EditorGUILayout.HelpBox("ObjectDatabaseを割り当ててください", MessageType.Warning);
            return;
        }

        // --- オブジェクト配置 ---
        // プレハブオブジェクト一覧から選択肢表示(ObjectDataBase参照)
        string[] name = stage.objectDatabase.placeableObjects.Select(o => o.objectName).ToArray();
        selectedIndex = EditorGUILayout.Popup("配置オブジェクト", selectedIndex, name);

        if (GUILayout.Button("シーンに配置"))
        {
            GameObject prefab = stage.objectDatabase.placeableObjects[selectedIndex].prefab;
            if(prefab != null)
            {
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(obj, "ステージオブジェクト配置");
                obj.transform.position = Vector3.zero;
                obj.transform.parent = stage.transform;
                Selection.activeObject = obj;
            }
        }
        EditorGUILayout.Space();
        rotationY = EditorGUILayout.Slider("配置角度(Y)", rotationY, 0f, 360f);
        DrawDefaultInspector();
        // --- EnemyData選択と情報表示 ---
        // Sceneビューで配置する敵のScriptableObjectを手動選択
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Enemy 配置", EditorStyles.boldLabel);
        stage.selectedEnemyData = (EnemyData)EditorGUILayout.ObjectField("EnemyData", stage.selectedEnemyData, typeof(EnemyData), false);
        EditorGUILayout.HelpBox("Sceneビューを左クリックで敵を配置します", MessageType.Info);
    }

    public void OnSceneGUI()
    {
        StageRoot stage = (StageRoot)target;

        if(stage.objectDatabase == null || stage.objectDatabase.placeableObjects.Count == 0)
        {
            return;
        }

        Event e = Event.current;
        // Ctrl + 左クリックでPrefab配置
        // 地面クリック → Terrainにスナップして配置
        if(e.type == EventType.MouseDown && e.button == 0 && e.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                GameObject prefab = stage.objectDatabase.placeableObjects[selectedIndex].prefab;
                if(prefab != null)
                {
                    Vector3 basePosition;

                    bool isPlaceObject = hit.transform != null && hit.transform.parent == stage.transform;

                    if (isPlaceObject)
                    {
                        basePosition = hit.transform.position;

                        // 正面・側面・上下どこをクリックしたかに応じたオフセット(モデル優先に変更し廃止)
                        Vector3 normal = hit.normal;
                        Vector3 offset = Vector3.zero;

                        if (Mathf.Abs(normal.x) >= Mathf.Abs(normal.y) && Mathf.Abs(normal.x) >= Mathf.Abs(normal.z))
                            offset.x = Mathf.Round(normal.x);
                        else if (Mathf.Abs(normal.y) >= Mathf.Abs(normal.x) && Mathf.Abs(normal.y) >= Mathf.Abs(normal.z))
                            offset.y = Mathf.Round(normal.y);
                        else
                            offset.z = Mathf.Round(normal.z);

                        basePosition += offset;
                    }
                    else
                    {
                        // 通常地面クリックの場合、XZは整数スナップ
                        basePosition = hit.point;
                        basePosition.x = Mathf.Round(basePosition.x);
                        basePosition.z = Mathf.Round(basePosition.z);
                    }

                    // Y座標（地面の高さ）を取得
                    float y = basePosition.y;
                    if (Physics.Raycast(new Vector3(basePosition.x, 1000f, basePosition.z), Vector3.down, out RaycastHit groundHit, 2000f))
                    {
                        y = groundHit.point.y;
                    }

                    Vector3 spawnPos = new Vector3(basePosition.x, y, basePosition.z); GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    Undo.RegisterCreatedObjectUndo(obj, "ステージオブジェクト配置");
                    obj.transform.position = spawnPos;
                    obj.transform.rotation = Quaternion.Euler(0, rotationY, 360);
                    obj.transform.parent = stage.transform;

                    // Selection を変更しない(配置ごとにインスペクタが切り替わるのを防ぐ)
                    Selection.activeGameObject = stage.gameObject;
                }
                // イベント消費
                e.Use();
            }
        }

        // Ctrl + 右クリックでステージ内の配置オブジェクトを削除
        if(e.type == EventType.MouseDown && e.button == 1 && e.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit hit))
            {
                Transform hitTransform = hit.transform;

                // 親がStageRootなら削除対象とみなす
                if(hitTransform != null && hitTransform.parent == stage.transform)
                {
                    Undo.DestroyObjectImmediate(hitTransform.gameObject);
                }
                e.Use();
            }
        }

        // EnemyDataが選択されている状態での左クリックにより、敵を配置＆削除
        if(e.type == EventType.MouseDown && e.button == 0 && stage.selectedEnemyData != null)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                Vector3 spawnPos = hit.point;
                stage.AddEnemy(stage.selectedEnemyData, spawnPos);
                e.Use();
            }
        }

        if(e.type == EventType.MouseDown && e.button == 1 && stage.selectedEnemyData != null)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay (e.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                Transform hitTransform = hit.transform;

                if(hitTransform != null && hitTransform.parent == stage.transform)
                {
                    // Enemiesリストから削除
                    EnemyAI ai = hitTransform.GetComponent<EnemyAI>();
                    if(ai != null && ai.data != null)
                    {
                        stage.RemoveEnemy(ai.data, hitTransform.position);
                    }
                    Undo.DestroyObjectImmediate(hitTransform.gameObject);
                }
            }
            e.Use();
        }
    }
}
#endif