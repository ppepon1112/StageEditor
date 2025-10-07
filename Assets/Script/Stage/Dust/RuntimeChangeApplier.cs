#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

[InitializeOnLoad]
public static class RuntimeChangeApplier
{
    //static RuntimeChangeApplier()
    //{
    //    EditorApplication.playModeStateChanged += OnStateChanged;
    //}

    //private static void OnStateChanged(PlayModeStateChange state)
    //{
    //    if (state == PlayModeStateChange.ExitingEditMode)
    //    {
    //        // プレイ直前（Editモード → Playモード前）に反映
    //        ApplyBufferToScene();
    //    }

    //    if (state == PlayModeStateChange.EnteredEditMode)
    //    {
    //        // Playモード終了後：バッファを消して保存
    //        ClearBufferAndSave();
    //    }
    //}

    //private static void ApplyBufferToScene()
    //{
    //    var buffer = AssetDatabase.LoadAssetAtPath<RuntimeChangeBuffer>(
    //        "Assets/EditorRuntime/RuntimeChangeBuffer.asset");
    //    if (buffer == null || buffer.changes.Count == 0) return;

    //    var stage = Object.FindObjectOfType<StageRoot>();
    //    if (stage == null) return;

    //    Undo.RegisterFullObjectHierarchyUndo(stage.gameObject, "Apply Runtime Changes");

    //    foreach (var ch in buffer.changes)
    //    {
    //        if (ch.operation == RuntimeChange.Op.Add)
    //        {
    //            string prefabPath = AssetDatabase.GUIDToAssetPath(ch.prefabGUID);
    //            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    //            if (!prefab) continue;

    //            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, stage.transform);
    //            Undo.RegisterCreatedObjectUndo(obj,"Apply Runtime Added Object");
    //            obj.transform.position = ch.position;
    //            obj.transform.rotation = Quaternion.Euler(0, ch.yaw, 0);
    //        }
    //        else if (ch.operation == RuntimeChange.Op.Remove)
    //        {
    //            var tgt = stage.transform.GetComponentsInChildren<Transform>()
    //                .FirstOrDefault(t => (t.position - ch.position).sqrMagnitude < 0.01f && t != stage.transform);
    //            if (tgt) Object.DestroyImmediate(tgt.gameObject);
    //        }
    //    }

    //    Debug.Log("<color=green>[ApplyBuffer]</color> バッファの変更をSceneに反映しました");
    //}

    //private static void ClearBufferAndSave()
    //{
    //    var buffer = AssetDatabase.LoadAssetAtPath<RuntimeChangeBuffer>(
    //        "Assets/EditorRuntime/RuntimeChangeBuffer.asset");

    //    if (buffer != null)
    //    {
    //        buffer.Clear();
    //        EditorUtility.SetDirty(buffer);
    //        AssetDatabase.SaveAssets();
    //        Debug.Log("<color=cyan>[ClearBuffer]</color> バッファをクリアしました");
    //    }

    //    EditorApplication.delayCall += () => {
    //        var scene = SceneManager.GetActiveScene();
    //        EditorSceneManager.MarkSceneDirty(scene);
    //        EditorSceneManager.SaveScene(scene);
    //        Debug.Log("<color=yellow>[Save]</color> シーンを保存しました");
    //    };
    //}
}
#endif

//static RuntimeChangeApplier()
//{
//    EditorApplication.playModeStateChanged -= OnStateChanged;
//    EditorApplication.playModeStateChanged += OnStateChanged;
//}
//private static void OnStateChanged(PlayModeStateChange state)
//{
//    // ? Play終了時の変更反映は DelayCall で遅延実行にする
//    if (state == PlayModeStateChange.ExitingPlayMode)
//    {
//        EditorApplication.delayCall += ApplyChangesAfterPlayMode;
//    }

//    // ? Play開始時：RuntimeChangeBuffer を反映してバッファをクリア
//    if (state == PlayModeStateChange.EnteredPlayMode)
//    {
//        ApplyRuntimeChanges();
//    }
//}

//private static void ApplyRuntimeChanges()
//{
//    var buffer = AssetDatabase.LoadAssetAtPath<RuntimeChangeBuffer>(
//        "Assets/EditorRuntime/RuntimeChangeBuffer.asset");
//    if (buffer == null || buffer.changes.Count == 0) return;

//    var stage = Object.FindObjectOfType<StageRoot>();
//    if (stage == null) return;

//    Undo.RegisterFullObjectHierarchyUndo(stage.gameObject, "Runtime Stage Edit");

//    foreach (var ch in buffer.changes)
//    {
//        if (ch.operation == RuntimeChange.Op.Add)
//        {
//            string prefabPath = AssetDatabase.GUIDToAssetPath(ch.prefabGUID);
//            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
//            if (!prefab) continue;

//            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, stage.transform);
//            obj.transform.position = ch.position;
//            obj.transform.rotation = Quaternion.Euler(0, ch.yaw, 0);
//        }
//        else if (ch.operation == RuntimeChange.Op.Remove)
//        {
//            var tgt = stage.transform.GetComponentsInChildren<Transform>()
//                .FirstOrDefault(t => (t.position - ch.position).sqrMagnitude < 0.01f && t != stage.transform);
//            if (tgt) Object.DestroyImmediate(tgt.gameObject);
//        }
//    }

//    buffer.Clear();
//    EditorUtility.SetDirty(buffer);  // ScriptableObject に変更あり
//    Debug.Log("Runtime changes applied and buffer cleared");
//}

//private static void ApplyChangesAfterPlayMode()
//{
//    Scene scene = SceneManager.GetActiveScene();
//    EditorSceneManager.MarkSceneDirty(scene);
//    Debug.Log($"Scene '{scene.name}' marked dirty. Please save with Ctrl+S.");
//}
