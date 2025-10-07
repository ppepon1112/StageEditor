using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.IO;
using UnityEditor.VersionControl;
using UnityEngine.Windows;

public class EnemyDataEditorWindow : EditorWindow
{
    private List<EnemyData> enemyList = new List<EnemyData>();
    private EnemyData selectedEnemy;
    private Vector2 scrollPos;

    [MenuItem("Tools/Enemy Data Editor")]
    public static void OpenWindow()
    {
        GetWindow<EnemyDataEditorWindow>("Enemy Data Editor");
    }
    private void OnEnable()
    {
        LoadAllEnemyData();
    }
    private void LoadAllEnemyData()
    {
        enemyList.Clear();
        string[] guids = AssetDatabase.FindAssets("t:EnemyData");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EnemyData data = AssetDatabase.LoadAssetAtPath<EnemyData>(path);
            if(data != null) enemyList.Add(data);
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        
        // 左：リスト
        DrawenemyList();
        // 右：詳細
        DrawSelectedEnemy();

        EditorGUILayout.EndHorizontal();
    }
    private void DrawenemyList()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach(var enemy in enemyList)
        {
            if (GUILayout.Button(enemy.enemyName))
            {
                selectedEnemy = enemy;
            }
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("新規作成"))
        {
            CreateNewEnemyData();
        }

        EditorGUILayout.EndVertical();
    }
    private void DrawSelectedEnemy()
    {
        if (selectedEnemy == null) return;

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Enemy Data 編集", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        selectedEnemy.enemyName = EditorGUILayout.TextField("名前", selectedEnemy.enemyName);
        selectedEnemy.hp = EditorGUILayout.IntField("HP", selectedEnemy.hp);
        selectedEnemy.attack = EditorGUILayout.IntField("攻撃力", selectedEnemy.attack);
        selectedEnemy.defense = EditorGUILayout.IntField("防御力", selectedEnemy.defense);
        selectedEnemy.experience = EditorGUILayout.IntField("経験値", selectedEnemy.experience);
        selectedEnemy.moveSpeed = EditorGUILayout.FloatField("移動速度", selectedEnemy.moveSpeed);
        selectedEnemy.icon = (Sprite)EditorGUILayout.ObjectField("アイコン", selectedEnemy.icon,typeof(Sprite),false);
        selectedEnemy.modelPrefab = (GameObject)EditorGUILayout.ObjectField("プレハブ", selectedEnemy.modelPrefab, typeof(GameObject), false);

        // スキル配列
        SerializedObject so = new SerializedObject(selectedEnemy);
        SerializedProperty abilitiesProp = so.FindProperty("abilities");
        EditorGUILayout.PropertyField(abilitiesProp, true);
        so.ApplyModifiedProperties();

        EditorGUILayout.Space();
        if (GUILayout.Button("保存"))
        {
            EditorUtility.SetDirty(selectedEnemy);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("削除"))
        {
            string path = AssetDatabase.GetAssetPath(selectedEnemy);
            if (EditorUtility.DisplayDialog("確認", "本当に削除しますか？", "削除", "キャンセル"))
            {
                AssetDatabase.DeleteAsset(path);
                selectedEnemy = null;
                LoadAllEnemyData();
            }
        }
        EditorGUILayout.EndVertical();
    }
    private void CreateNewEnemyData()
    {
        string path = "Assets/EnemyData";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/NewEnemy.asset");

        EnemyData newEnemy = ScriptableObject.CreateInstance<EnemyData>();
        AssetDatabase.CreateAsset(newEnemy, assetPath);
        LoadAllEnemyData();
        selectedEnemy = newEnemy;
    }
}
