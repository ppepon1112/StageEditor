using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBerFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3 (0, 2f, 0);

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }
}
