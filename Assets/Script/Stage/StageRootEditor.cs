#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(StageRoot))]

//==============================
// Unity�G�f�B�^�g���c�[���F�X�e�[�W�ҏW�p
// ? ObjectDataBase����v���n�u��I�����Ĕz�u
// ? Ctrl+���N���b�N�FTerrain�� or ���I�u�W�F�N�g�̖ʏ�ɔz�u(���ʔz�u�͌��ʂ�Cube�Ɍ��肳��邽�߃��f���D��ɂ��p�~)
// ? Ctrl+�E�N���b�N�F�z�u�I�u�W�F�N�g���폜
// ? EnemyData��ScriptableObject�ŊǗ����A�V�[���ɓG��z�u
// ? Alt+���N���b�N�F�G��z�u(StageRoot�ɓo�^)
// ? Alt+�E�N���b�N�F�G���폜(���X�g������폜)
//==============================
public class StageRootEditor : Editor
{
    private int selectedIndex = 0;
    private float rotationY = 0f;

    public override void OnInspectorGUI()
    {
        StageRoot stage = (StageRoot)target;
        // --- �G�f�B�^�����`�F�b�N ---
        if(stage.objectDatabase == null)
        {
            EditorGUILayout.HelpBox("ObjectDatabase�����蓖�ĂĂ�������", MessageType.Warning);
            return;
        }

        // --- �I�u�W�F�N�g�z�u ---
        // �v���n�u�I�u�W�F�N�g�ꗗ����I�����\��(ObjectDataBase�Q��)
        string[] name = stage.objectDatabase.placeableObjects.Select(o => o.objectName).ToArray();
        selectedIndex = EditorGUILayout.Popup("�z�u�I�u�W�F�N�g", selectedIndex, name);

        if (GUILayout.Button("�V�[���ɔz�u"))
        {
            GameObject prefab = stage.objectDatabase.placeableObjects[selectedIndex].prefab;
            if(prefab != null)
            {
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(obj, "�X�e�[�W�I�u�W�F�N�g�z�u");
                obj.transform.position = Vector3.zero;
                obj.transform.parent = stage.transform;
                Selection.activeObject = obj;
            }
        }
        EditorGUILayout.Space();
        rotationY = EditorGUILayout.Slider("�z�u�p�x(Y)", rotationY, 0f, 360f);
        DrawDefaultInspector();
        // --- EnemyData�I���Ə��\�� ---
        // Scene�r���[�Ŕz�u����G��ScriptableObject���蓮�I��
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Enemy �z�u", EditorStyles.boldLabel);
        stage.selectedEnemyData = (EnemyData)EditorGUILayout.ObjectField("EnemyData", stage.selectedEnemyData, typeof(EnemyData), false);
        EditorGUILayout.HelpBox("Scene�r���[�����N���b�N�œG��z�u���܂�", MessageType.Info);
    }

    public void OnSceneGUI()
    {
        StageRoot stage = (StageRoot)target;

        if(stage.objectDatabase == null || stage.objectDatabase.placeableObjects.Count == 0)
        {
            return;
        }

        Event e = Event.current;
        // Ctrl + ���N���b�N��Prefab�z�u
        // �n�ʃN���b�N �� Terrain�ɃX�i�b�v���Ĕz�u
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

                        // ���ʁE���ʁE�㉺�ǂ����N���b�N�������ɉ������I�t�Z�b�g(���f���D��ɕύX���p�~)
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
                        // �ʏ�n�ʃN���b�N�̏ꍇ�AXZ�͐����X�i�b�v
                        basePosition = hit.point;
                        basePosition.x = Mathf.Round(basePosition.x);
                        basePosition.z = Mathf.Round(basePosition.z);
                    }

                    // Y���W�i�n�ʂ̍����j���擾
                    float y = basePosition.y;
                    if (Physics.Raycast(new Vector3(basePosition.x, 1000f, basePosition.z), Vector3.down, out RaycastHit groundHit, 2000f))
                    {
                        y = groundHit.point.y;
                    }

                    Vector3 spawnPos = new Vector3(basePosition.x, y, basePosition.z); GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    Undo.RegisterCreatedObjectUndo(obj, "�X�e�[�W�I�u�W�F�N�g�z�u");
                    obj.transform.position = spawnPos;
                    obj.transform.rotation = Quaternion.Euler(0, rotationY, 360);
                    obj.transform.parent = stage.transform;

                    // Selection ��ύX���Ȃ�(�z�u���ƂɃC���X�y�N�^���؂�ւ��̂�h��)
                    Selection.activeGameObject = stage.gameObject;
                }
                // �C�x���g����
                e.Use();
            }
        }

        // Ctrl + �E�N���b�N�ŃX�e�[�W���̔z�u�I�u�W�F�N�g���폜
        if(e.type == EventType.MouseDown && e.button == 1 && e.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit hit))
            {
                Transform hitTransform = hit.transform;

                // �e��StageRoot�Ȃ�폜�ΏۂƂ݂Ȃ�
                if(hitTransform != null && hitTransform.parent == stage.transform)
                {
                    Undo.DestroyObjectImmediate(hitTransform.gameObject);
                }
                e.Use();
            }
        }

        // EnemyData���I������Ă����Ԃł̍��N���b�N�ɂ��A�G��z�u���폜
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
                    // Enemies���X�g����폜
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