using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAi : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    public bool isDie = false;
    public bool isAttack = false;

    MonsterAnim monsterAnim;
    MonsterInfo monsterInfo;
    Creature creature;

    NavMeshAgent agent;
    Transform playerTr;

    float dist; //플레이어와 적의 거리


    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        monsterInfo = GetComponent<MonsterInfo>();
        creature = GetComponent<Creature>();
        agent = GetComponent<NavMeshAgent>();

        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
    }
    private void Update()
    {
        dist = Vector3.Distance(playerTr.position, transform.position);
    }
    private void OnEnable()
    {
        StartCoroutine(Action());
        StartCoroutine(CheckState());
    }
    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1f);

        while (!isDie)
        {
            if (creature.state == STATE.Die)
                yield break;
            else if (dist <= monsterInfo.meleeDist)
            {
                creature.state = STATE.Backing;
            }
            else if (dist <= monsterInfo.attackDist && dist > monsterInfo.meleeDist+3)
            {
                creature.state = STATE.Attacking;
            }
            else if (dist <= monsterInfo.traceDist && dist > monsterInfo.attackDist)
            {
                creature.state = STATE.Chase;
            }
            else
            {
                creature.state = STATE.Patrol;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// 스테이터스에 따른 애니메이션,행동을 제어하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            switch (creature.state)
            {
                case STATE.Patrol:
                    agent.enabled = true;
                    monsterInfo.MovePoint();
                    break;
                case STATE.Chase:
                    agent.enabled = true;
                    monsterInfo.Chase(playerTr.position);
                    break;
                case STATE.Attacking:
                    //agent.enabled = false;
                    //agent.isStopped = true;
                    //agent.speed = 0;
                    agent.velocity = Vector3.zero;
                    monsterInfo.Attack(playerTr.position);
                    break;
                case STATE.Backing:
                    agent.enabled = true;
                    monsterInfo.BackMove(playerTr.position);
                    break;
                case STATE.Die:
                    monsterInfo.Die();
                    break;
            }
        }
    }

}
