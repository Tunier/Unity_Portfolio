using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Boar : MonsterBase
{
    public List<Transform> movePoints;  //무브포인트
    public int nextIdx; //다음 순찰 지점의 인덱스
    public bool isDie = false;
    public bool isAttack = false;
    public float attackDist = 5f;
    public float traceDist = 10f;
    public float viewAngle = 60f;

    float dist; //플레이어와 적의 거리;
    float nextFire = 0f; //공격 관련 변수
    
    float damping = 1f;
    Transform playerTr;
    Transform enemyTr;
    Quaternion rot;

    PlayerInfo playerinfo;

    [SerializeField]
    GameObject player;

    NavMeshAgent agent;
    Animator animator;

    readonly float attackRate = 0.2f;
    readonly float patrolSpeed = 4f;
    readonly float traceSpeed = 7f;
    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashDie = Animator.StringToHash("IsDie");
    readonly int hashAttack = Animator.StringToHash("IsAttack");

    bool _patrolling;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1f;
                MovePoint();
            }
        }
    }

    Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7f;
            TraceTarget(_traceTarget);
        }
    }
    

    private void Start()
    {
        state = STATE.Patrol;
        finalAtk = 10f;
        finalDef = 10f;
        finalMaxHp = 50f;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerinfo = player.GetComponent<PlayerInfo>();
        
        


        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        enemyTr = GetComponent<Transform>();

        //무브포인트를 찾고 있으면 위치를 받아서 랜덤으로 설정
        var group = GameObject.Find("MovePoints");
        if (group)
        {
            group.GetComponentsInChildren<Transform>(movePoints);
            movePoints.RemoveAt(0);
            nextIdx = Random.Range(0, movePoints.Count);
            MovePoint();
        }
    }

    private void Update()
    {
        dist = Vector3.Distance(playerTr.position, enemyTr.position);
        MovedPoint();
        rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
    }

    public void LookRotation()
    {
        damping = 30f;
        animator.SetBool(hashMove, true);
        enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
    }
    public void Attack()
    {
        if (isAttack)
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            if (Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f)
            {
                if (Time.time >= nextFire)
                {
                    animator.SetTrigger(hashAttack);
                    nextFire = Time.time + attackRate + Random.Range(0.5f, 1f);
                }
            }
            else
            {
                return;
            }
        }
    }

    public override void Die()
    {
        gameObject.tag = "Untagged";
        isDie = true;
        Stop();
        animator.SetTrigger(hashDie);
        GetComponent<BoxCollider>().enabled = false;
    }

    public override void DropItem()
    {
    }

    public override void Hit(float _damage)
    {
    }

    /// <summary>
    /// 이동 중일때 다른 경로로 변경 하지 않도록 하는 함수
    /// </summary>
    void MovePoint()
    {
        if (agent.isPathStale)
            return;

        agent.destination = movePoints[nextIdx].position;
        agent.isStopped = false;
    }

    /// <summary>
    /// 상태가 Chase일때 목적지를 매개변수로 설정
    /// </summary>
    /// <param name="pos"></param>
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;
        agent.isStopped = false;
    }
       
    /// <summary>
    /// Agent 멈추는 함수
    /// </summary>
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    /// <summary>
    /// 도착지점에 가까워지면 새로운 포인트로 계산하는 함수
    /// </summary>
    void MovedPoint()
    {
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f)
        {
            if (agent.remainingDistance <= 0.5f)
            {
                nextIdx = Random.Range(0, movePoints.Count);
                MovePoint();
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1f);

        while (!isDie)
        {
            if (state == STATE.Die)
                yield break;

            if(dist <= 1f)
            {
                state = STATE.Backoff;
            }
            else if (dist <= attackDist && dist > 2f)
            {
                state = STATE.Attacking;
            }
            else if (dist <= traceDist && dist > attackDist)
            {
                state = STATE.Chase;
            }
            else
            {
                state = STATE.Patrol;
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

            switch (state)
            {
                case STATE.Patrol:
                    isAttack = false;
                    patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Chase:
                    isAttack = false;
                    traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Attacking:
                    Stop();
                    animator.SetBool(hashMove, false);
                    if (isAttack == false)
                        isAttack = true;
                    Attack();
                    traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Backoff:
                    isAttack = false;
                    agent.destination = playerTr.position*2f;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Die:
                    Die();
                    break;
            }
        }
    }
}
