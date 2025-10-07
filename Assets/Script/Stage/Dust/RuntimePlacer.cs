using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RuntimePlacer : MonoBehaviour
{
//    [SerializeField] ObjectDatabase db;
//    [SerializeField] RuntimeChangeBuffer buffer;

//    int prefabIndex = 0;
//    Camera cam;

//    [SerializeField] StageRoot stageRoot;
//    GameObject currentPrefab;

//    public void SelectPrefab(GameObject pf) => currentPrefab = pf;
//    // Start is called before the first frame update
//    void Start()
//    {
//        cam = Camera.main;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (currentPrefab == null) return;
//        if (!Input.GetKey(KeyCode.LeftControl)) return;
//        if (Input.GetMouseButtonDown(0)) Place();
//        if (Input.GetMouseButtonDown(1))
//        {
//            TryDelete(stageRoot.gameObject);

//            Remove();
//        }
//    }
//    public void Place()
//    {
//        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit)) return;

//        GameObject prefab = db.placeableObjects[prefabIndex].prefab;
//#if UNITY_EDITOR
//        string path = AssetDatabase.GetAssetPath(prefab);
//        string guid = AssetDatabase.AssetPathToGUID(path);
//#else
//        string guid = "";
//#endif

//        buffer.changes.Add(new RuntimeChange
//        {
//            operation = RuntimeChange.Op.Add,
//            prefabGUID = guid,
//            position = hit.point,
//            yaw = 0f
//        });

//        Instantiate(prefab,hit.point,Quaternion.identity);
//    }
//    public void Remove()
//    {
//        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit)) return;
//        var removePos = hit.collider.transform.position;
//        buffer.changes.Add(new RuntimeChange
//        {
//            operation = RuntimeChange.Op.Remove,
//            prefabGUID = "",
//            position = removePos,
//        });
//        Destroy(hit.collider.gameObject);
//    }

//    void TryDelete(GameObject hitGo)
//    {
//        if (hitGo.CompareTag("Player") || hitGo.GetComponent<Terrain>() != null) return;
//        if (hitGo.transform.parent != stageRoot.transform) return;
//        Destroy(hitGo);
//    }
}
