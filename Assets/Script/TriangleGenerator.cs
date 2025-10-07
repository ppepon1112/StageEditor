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

        // 三角形の頂点
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
        };
        // 三角形の面(時計回り)
        int[] triangles = new int[] { 0, 1, 2 };

        // メッシュに設定
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // デフォルトマテリアルを割り当て（割り当てられなければ表示されない）
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
}
