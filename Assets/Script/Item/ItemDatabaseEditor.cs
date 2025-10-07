#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDatabase Idb = (ItemDatabase)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("登録済みアイテム数： " + Idb.allItems.Count);

        if (GUILayout.Button("新しいアイテムを追加"))
        {
            ItemData newItem = ScriptableObject.CreateInstance<ItemData>();
            string path = EditorUtility.SaveFilePanelInProject("新規アイテム","NewItem","asset","アイテムを保存する場所を選択してください");
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(newItem,path);
                AssetDatabase.SaveAssets();
                Idb.allItems.Add(newItem);
                EditorUtility.SetDirty(Idb);
            }
        }
    }
}
#endif