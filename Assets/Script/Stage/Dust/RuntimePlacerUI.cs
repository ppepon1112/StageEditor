using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimePlacerUI : MonoBehaviour
{
    //[Header("ÉfÅ[É^")]
    //public ObjectDatabase database;
    //[Header("UI")]
    //public Canvas canvas;
    //public RectTransform panel;
    //public GameObject buttonPrefab;

    //RuntimePlacer placer;
    //bool visible;

    //void Awake()
    //{
    //    placer = FindObjectOfType<RuntimePlacer>();
    //    BuildButtons();
    //    Toggle(false);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T)) Toggle(!visible);
    //}

    //void Toggle(bool on)
    //{
    //    visible = on;
    //    canvas.enabled = on;
    //}

    //void BuildButtons()
    //{
    //    foreach(var obj in database.placeableObjects)
    //    {
    //        Button b = Instantiate(buttonPrefab,panel).GetComponent<Button>();
    //        b.GetComponentInChildren<Text>().text = obj.objectName;
    //        b.onClick.AddListener(() => OnClick(obj));
    //    }
    //}
    //void OnClick(PlaceableObject po)
    //{
    //    placer.SelectPrefab(po.prefab);
    //    Toggle(false);
    //}
}
