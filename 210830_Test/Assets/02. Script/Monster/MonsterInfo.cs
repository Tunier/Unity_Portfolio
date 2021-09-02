using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterInfo : MonsterBase
{
    public GameObject group;            //���ͺ� ��������Ʈ ���� ���� �־��ֱ�
    public List<Transform> movePoints;  //��������Ʈ
    public int nextIdx;                 //���� ���� ������ �ε���
    public float meleeDist = 3f;        //�ּ� ���ݰŸ�
    public float attackDist = 5f;       //�ִ� ���ݰŸ�
    public float traceDist = 10f;       //���� �Ÿ�

    public float attackRate = 0.2f;     //���ݵ�����
    public float nextFire = 0f;

    public float patrolSpeed = 6f;      //�ϻ�ӵ�
    public float traceSpeed = 10f;      //�����ӵ�
    public float backSpeed = 20f;       //�ڷ� �������� �ӵ�

    private int obstacleLayer;          //��ֹ� ���̾�
    private int playerLayer;            //�÷��̾� ���̾�

    public bool isDie = false;
    public bool isAttack = false;

    Vector3 monsterTr;
    NavMeshAgent agent;
    MonsterAnim monsterAnim;

    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        agent = GetComponent<NavMeshAgent>();
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        playerLayer = LayerMask.NameToLayer("Player");
        monsterTr = transform.position + (Vector3.up * 2);

        if (group)
        {
            movePoints.AddRange(group.GetComponentsInChildren<Transform>());
            movePoints.RemoveAt(0);
            nextIdx = Random.Range(0, movePoints.Count);
        }
    }

    public void StartMove()
    {
        StartCoroutine(MovePoint());
    }

    /// <summary>
    /// �������� �̵� & ���� �޽Ľð�
    /// </summary>
    public IEnumerator MovePoint()
    {
        isAttack = false;

        if (agent.isPathStale)
            yield return null;

        monsterAnim.OnMove(true);
        agent.speed = patrolSpeed;
        agent.destination = movePoints[nextIdx].position;
        agent.isStopped = false;

        if (agent.velocity.sqrMagnitude < 0.2f * 0.2f)
        {
            if (agent.remainingDistance <= 0.5f)
            {
                monsterAnim.OnMove(false);
                agent.isStopped = true;
                nextIdx = Random.Range(0, movePoints.Count);
            }
        }
        yield return new WaitForSeconds(Random.Range(0, 2.5f));

    }

    public void Chase(Vector3 _target)
    {
        isAttack = false;

        if (agent.isPathStale)
            return;

        monsterAnim.OnMove(true);
        agent.speed = traceSpeed;
        agent.destination = _target;
        agent.isStopped = false;
    }

    public void Attack(Vector3 _target)
    {
        Vector3 dir = (_target - transform.position).normalized;
        if (Physics.Raycast(transform.position + (Vector3.up * 2), transform.forward, attackDist * 1.5f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //�þ߰�
        {
            if (isAttack == false)
            {
                isAttack = true;
            }
            Stop();
            if (Time.time >= nextFire)
            {
                monsterAnim.OnAttack();
                nextFire = Time.time + attackRate + Random.Range(0.5f, 1f);
            }
        }
        else
        {
            if (isAttack == true)
            {
                isAttack = false;
            }
            else
            {
                monsterAnim.OnMove(true);
                agent.SetDestination(_target);
                agent.speed = backSpeed;
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
            }
        }
    }

    public void BackMove(Vector3 _target)
    {
        agent.isStopped = false;
        isAttack = false;
        monsterAnim.OnMove(true);
        Vector3 dir = (transform.position - _target).normalized;
        if (Physics.Raycast(monsterTr, -transform.forward, 5f, 1 << obstacleLayer))
        {
            Debug.Log("�ɽ�ŸŬ����");
            //�ڷ� �����Ҷ� �ɽ�ŸŬ ���ϴ� ��ġ�� �������� �����ϴ� �Լ� �ֱ�
        }
        else
        {
            Debug.Log("�ɽ�ŸŬ ����");
            agent.SetDestination(transform.position + dir * traceDist);
            agent.speed = backSpeed;
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f)
            {
                if (agent.remainingDistance <= 0.5f)
                {
                    state = STATE.Chase;
                }
            }
        }
    }

    /// <summary>
    /// Agent ���ߴ� �Լ�
    /// </summary>
    public void Stop()
    {
        monsterAnim.OnMove(false);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public override void Die()
    {
    }

    public override void DropItem()
    {
    }

    public override void Hit(float _damage)
    {
    }
}
