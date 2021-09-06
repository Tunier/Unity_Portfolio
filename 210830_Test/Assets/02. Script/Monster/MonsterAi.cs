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
    MonsterAction monsterAction;
    Creature creature;

    NavMeshAgent agent;
    Transform playerTr;

    float dist; //�÷��̾�� ���� �Ÿ�


    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        monsterAction = GetComponent<MonsterAction>();
        creature = GetComponent<Creature>();
        agent = GetComponent<NavMeshAgent>();
        creature.state = STATE.Patrol;

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

        while (!isDie && monsterAction.isAnger == true) //�����ʰ� Anger�� true�϶� ���º�ȭ
        {
            //ü���� �������ϸ� backing���·� ���� �̵� ���� �ɾ��ֱ�
            if (creature.state == STATE.Die)
                yield break;
            else if (dist <= monsterAction.maxDist)
            {
                if (dist < monsterAction.minDist)
                {
                    creature.state = STATE.Backing;
                }
            }
            else if (dist <= monsterAction.attackDist && dist > monsterAction.maxDist)
            {
                creature.state = STATE.Attacking;
            }
            else if (dist <= monsterAction.traceDist && dist > monsterAction.attackDist)
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
    /// �������ͽ��� ���� �ִϸ��̼�,�ൿ�� �����ϴ� �ڷ�ƾ �Լ�
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
                    monsterAction.MovePoint();
                    break;
                case STATE.Chase:
                    monsterAction.Chase(playerTr.position);
                    break;
                case STATE.Attacking:
                    //agent.enabled = false;
                    //agent.isStopped = true;
                    //agent.speed = 0;
                    agent.velocity = Vector3.zero;
                    monsterAction.Attack(playerTr.position);
                    break;
                case STATE.Backing:
                    monsterAction.BackMove(playerTr.position);
                    break;
                case STATE.Die:
                    monsterAction.Die();
                    break;
            }
        }
    }

}
