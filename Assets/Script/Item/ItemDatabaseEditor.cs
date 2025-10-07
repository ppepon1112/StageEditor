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
        EditorGUILayout.LabelField("�o�^�ς݃A�C�e�����F " + Idb.allItems.Count);

        if (GUILayout.Button("�V�����A�C�e����ǉ�"))
        {
            ItemData newItem = ScriptableObject.CreateInstance<ItemData>();
            string path = EditorUtility.SaveFilePanelInProject("�V�K�A�C�e��","NewItem","asset","�A�C�e����ۑ�����ꏊ��I�����Ă�������");
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