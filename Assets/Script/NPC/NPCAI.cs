using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    [Header("徘徊ポイントをまとめた親オブジェクト")]
    public Transform waypointGroup;

    private List<Transform> points = new List<Transform>();
    private int destPoint = 0;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        if(waypointGroup != null)
        {
            foreach(Transform child in waypointGroup)
            {
                points.Add(child);
            }
        }

        GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        if (points.Count == 0) return;

        agent.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    public void SetCanMove(bool value)
    {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = !value;
        }
    }

}
