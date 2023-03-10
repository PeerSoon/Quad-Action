using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{

    public Transform target;
    NavMeshAgent nav;
    public float lifeTime = 5f;
    private float startTime;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        startTime = Time.time; // 0초부터 초를 저장함.
    }
    void Update()
    {
        nav.SetDestination(target.position);

        if (Time.time - startTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
