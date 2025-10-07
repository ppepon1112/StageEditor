using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject triangle = new GameObject("Triangle");

        MeshFilter meshFilter = triangle.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = triangle.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        // �O�p�`�̒��_
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
        };
        // �O�p�`�̖�(���v���)
        int[] triangles = new int[] { 0, 1, 2 };

        // ���b�V���ɐݒ�
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // �f�t�H���g�}�e���A�������蓖�āi���蓖�Ă��Ȃ���Ε\������Ȃ��j
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
}
