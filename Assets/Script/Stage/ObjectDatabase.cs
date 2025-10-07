using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StageEditor/ObjectDatabase")]

//==============================
// StageEditor�̍��ڂ�ObjectDatabase��ǉ�
// Assets�E�N���b�N��ObjectDatabase�ǉ�
// 
// �ڍׁF�o�^�����X�e�[�W�z�u�p�I�u�W�F�N�g�����X�g�ɂ܂Ƃ߂Ĉꗗ�ɂ���
//==============================
public class ObjectDatabase : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;
}
