using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NpcMovement : MonoBehaviour
{
    Animator animator;
    public float walkSpeed = 4f;

    public int nextIdx;

    NavMeshAgent agent;
    public List<Transform> movePoints;
    readonly int hashMove = Animator.StringToHash("IsMove");
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    void Update()
    {
        MovePoint();
    }

    void MovePoint()
    {
        if (agent.isPathStale)
            return;

        animator.SetBool(hashMove,true);

        agent.destination = movePoints[nextIdx].position;
        agent.speed = walkSpeed;

        if(agent.velocity.magnitude < 1.5f && agent.remainingDistance <= 1.5f)
        {
            nextIdx = Random.Range(0, movePoints.Count+1);
        }

    }
}
