using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugSaveResetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerdata.json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("�Z�[�u�f�[�^���폜���܂���");
        }
        else
        {
            Debug.Log("�Z�[�u�f�[�^��������܂���ł���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
