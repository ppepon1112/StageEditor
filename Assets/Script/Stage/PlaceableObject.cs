using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StageEditor/PlaceableObject")]

//==============================
// StageEditor�̍��ڂ�ǉ�
// StageEditor�̍��ڂ�PlaceableObject��ǉ�
// Assets�E�N���b�N��PlaceableObject�ǉ�
// tool�^�O��ǉ�
// EnemyData�ɖ�G�I�u�W�F�N�g
// 
// �ڍׁF�X�e�[�W�z�u�p�I�u�W�F�N�g(�v���n�u)��o�^
//==============================
public class PlaceableObject : ScriptableObject
{
    public string objectName;
    public GameObject prefab;
}
