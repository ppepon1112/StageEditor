using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCulling : MonoBehaviour
{
    private Transform Player;
    public float visibleDistance = 100f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (Player == null) return;

        float distance = Vector3.Distance(Player.position, transform.position);
        bool shouldBeVisible = distance < visibleDistance;

        if(gameObject.activeSelf != shouldBeVisible)
        {
            gameObject.SetActive(shouldBeVisible);
        }
    }
}
