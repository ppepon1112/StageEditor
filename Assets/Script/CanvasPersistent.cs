using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPersistent : MonoBehaviour
{
    private static CanvasPersistent instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
