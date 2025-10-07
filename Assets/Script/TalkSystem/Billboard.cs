using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public void LateUpdate()
    {
        if(Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
