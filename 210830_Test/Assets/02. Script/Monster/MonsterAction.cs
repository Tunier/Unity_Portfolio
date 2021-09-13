using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAction : MonsterBase
{
    public int attackMetod;             //0:근접공격 1:화살공격 2:매직공격

    public GameObject group;            //몬스터별 무브포인트 기준 파일 넣어주기
    public List<Transform> movePoints;  //무브포인트
    public int nextIdx;                 //다음 순찰 지점의 인덱스
    public float minDist = 3f;          //최소 공격거리
    public float maxDist = 5f;
    public float attackDist = 5f;       //최대 공격거리
    public float traceDist = 10f;       //추적 거리

    public float attackRate = 0.2f;     //공격딜레이
    public float nextFire = 0f;

    public float patrolSpeed = 4f;      //일상속도
    public float traceSpeed = 10f;      //추적속도
    public float backSpeed = 20f;       //뒤로 도망가는 속도

    private int obstacleLayer;          //장애물 레이어
    private int playerLayer;            //플레이어 레이어
    private int monsterLayer;           //몬스터 레이어

    private Collider monsterCollider;

    public bool isDie = false;
    public bool isAttack = false;
    public bool isAnger = false;        //선공 : Anger true 후공 : Anger false

    Vector3 monsterTr;
    NavMeshAgent agent;
    MonsterAnim monsterAnim;
    MonsterFire monsterFire;

    //public Collider[] monsters; //테스트
    public List<Collider> monsters = new List<Collider>();

    private void Awake()
    {
        monsterFire = GetComponent<MonsterFire>();
        monsterAnim = GetComponent<MonsterAnim>();
        monsterCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        playerLayer = LayerMask.NameToLayer("Player");
        monsterLayer = LayerMask.NameToLayer("Monster");
        monsterTr = transform.position + (Vector3.up * 2);

        if (group)
        {
            movePoints.AddRange(group.GetComponentsInChildren<Transform>());
            movePoints.RemoveAt(0);
            nextIdx = Random.Range(0, movePoints.Count);
        }
    }


    /// <summary>
    /// 랜덤으로 이동 & 랜덤 휴식시간
    /// </summary>
    public void MovePoint()
    {
        isAttack = false;

        if (agent.isPathStale)
            return;

        agent.isStopped = false;
        agent.destination = movePoints[nextIdx].position;
        agent.speed = patrolSpeed;
        monsterAnim.OnMove(true, agent.speed);
        print(nextIdx);

        if (agent.velocity.magnitude < 1.5f && agent.remainingDistance <= 1.5f)
        {
            nextIdx = Random.Range(0, movePoints.Count);
        }
    }

    public void Chase(Vector3 _target)
    {
        isAttack = false;

        if (agent.isPathStale)
            return;

        monsterAnim.OnMove(true, agent.speed);
        agent.speed = traceSpeed;
        agent.destination = _target;
        agent.isStopped = false;
    }

    public void Attack(Vector3 _target)
    {
        Vector3 dir = (_target - transform.position).normalized;
        if (Physics.Raycast(transform.position + (Vector3.up * 2.5f), transform.forward, attackDist * 1.5f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //시야각
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

            monsterAnim.OnMove(true, agent.speed);
            agent.SetDestination(_target);
            agent.speed = backSpeed;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);

        }
    }

    public void BackMove(Vector3 _target)
    {
        agent.isStopped = false;
        isAttack = false;
        monsterAnim.OnMove(true, agent.speed);
        Vector3 dir = (transform.position - _target).normalized;
        if (Physics.Raycast(monsterTr, -transform.forward, 5f, 1 << obstacleLayer))
        {
            Debug.Log("옵스타클갑지");
            //뒤로 무빙할때 옵스타클 피하는 위치를 목적지로 설정하는 함수 넣기
        }
        else
        {
            Debug.Log("옵스타클 비감지");
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
    /// Agent 멈추는 함수
    /// </summary>
    public void Stop()
    {
        monsterAnim.OnMove(false, agent.speed);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public override void Die()
    {
        Stop();
        isDie = true;
        isAttack = false;
        gameObject.tag = "Untagged";
        GetComponent<CapsuleCollider>().enabled = false;

        monsterAnim.OnDie();
        monsterAnim.OnDieIdx();
    }

    public override void DropItem()
    {
    }

    public override void Hit(float _damage)
    {
        if (!isDie)
        {
            if (!monsters.Contains(monsterCollider))
            {
                monsters.AddRange(Physics.OverlapSphere(monsterTr, traceDist * 3f, 1 << monsterLayer));
            }
            for (int i = 0; i < monsters.Count; i++)
            {
                var mob = monsters[i].GetComponent<MonsterAction>();

                if (mob.isAnger == false)
                {
                    mob.isAnger = true;
                }
            }
            curHp -= _damage - finalDef;
            monsterAnim.OnHit();
            if (curHp <= 0)
            {
                state = STATE.Die;
            }
        }
    }
}

