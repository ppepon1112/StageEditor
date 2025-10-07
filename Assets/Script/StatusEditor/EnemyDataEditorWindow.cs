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
        
        // ���F���X�g
        DrawenemyList();
        // �E�F�ڍ�
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

        if (GUILayout.Button("�V�K�쐬"))
        {
            CreateNewEnemyData();
        }

        EditorGUILayout.EndVertical();
    }
    private void DrawSelectedEnemy()
    {
        if (selectedEnemy == null) return;

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Enemy Data �ҏW", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        selectedEnemy.enemyName = EditorGUILayout.TextField("���O", selectedEnemy.enemyName);
        selectedEnemy.hp = EditorGUILayout.IntField("HP", selectedEnemy.hp);
        selectedEnemy.attack = EditorGUILayout.IntField("�U����", selectedEnemy.attack);
        selectedEnemy.defense = EditorGUILayout.IntField("�h���", selectedEnemy.defense);
        selectedEnemy.experience = EditorGUILayout.IntField("�o���l", selectedEnemy.experience);
        selectedEnemy.moveSpeed = EditorGUILayout.FloatField("�ړ����x", selectedEnemy.moveSpeed);
        selectedEnemy.icon = (Sprite)EditorGUILayout.ObjectField("�A�C�R��", selectedEnemy.icon,typeof(Sprite),false);
        selectedEnemy.modelPrefab = (GameObject)EditorGUILayout.ObjectField("�v���n�u", selectedEnemy.modelPrefab, typeof(GameObject), false);

        // �X�L���z��
        SerializedObject so = new SerializedObject(selectedEnemy);
        SerializedProperty abilitiesProp = so.FindProperty("abilities");
        EditorGUILayout.PropertyField(abilitiesProp, true);
        so.ApplyModifiedProperties();

        EditorGUILayout.Space();
        if (GUILayout.Button("�ۑ�"))
        {
            EditorUtility.SetDirty(selectedEnemy);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("�폜"))
        {
            string path = AssetDatabase.GetAssetPath(selectedEnemy);
            if (EditorUtility.DisplayDialog("�m�F", "�{���ɍ폜���܂����H", "�폜", "�L�����Z��"))
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
