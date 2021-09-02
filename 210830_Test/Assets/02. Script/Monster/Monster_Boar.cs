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
    public bool isMove = false;
    public float meleeDist = 3f;
    public float attackDist = 5f;
    public float traceDist = 10f;
    public float viewAngle = 60f;

    bool goalDestination = false;
    float dist; //플레이어와 적의 거리;
    float nextFire = 0f; //공격 관련 변수
    Vector3 backDestinaion;
    Vector3 dir;

    int obstacleLayer;
    int playerLayer;
    Transform playerTr;
    Transform enemyTr;
    Quaternion rot;

    PlayerInfo playerinfo;

    [SerializeField]
    GameObject player;

    NavMeshAgent agent;
    Animator animator;

    readonly float attackRate = 0.2f;
    readonly float patrolSpeed = 6f;
    readonly float traceSpeed = 10f;
    readonly float backSpeed = 20f;
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
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        playerLayer = LayerMask.NameToLayer("Player");




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
        //rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
        dir = (playerTr.position - enemyTr.position).normalized;
        MovedPoint();
    }

    public void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 2), transform.forward, attackDist, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //시야각
        {
            if (isAttack == false)
            {
                isAttack = true;
            }
            Stop();
            if (Time.time >= nextFire)
            {
                animator.SetTrigger(hashAttack);
                nextFire = Time.time + attackRate + Random.Range(0.5f, 1f);
            }

        }
        else
        {
            if(isAttack == true)
            {
                isAttack = false;
            }

            if (agent.isPathStale)
                return;

            isMove = true;
            isAttack = false;
            agent.speed = backSpeed;
            agent.destination = playerTr.position;
            agent.isStopped = false;
            //animator.SetBool(hashMove, true);
            //agent.angularSpeed = 360f;
            //agent.SetDestination(transform.position + backDestinaion * 5);
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
        agent.ResetPath();
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

    void BackMove()
    {

        backDestinaion = new Vector3(transform.position.x - playerTr.position.x, 0f, transform.position.z - playerTr.position.z).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 2), -transform.forward, out hit, 5f, 1 << obstacleLayer))
        {
            Debug.Log("옵스타클감지");
            Debug.DrawRay(transform.position + (Vector3.up * 2), -transform.forward * 50, Color.red);
        }
        else
        {
            Debug.Log("옵스타클불감지");
            Debug.DrawRay(transform.position + (Vector3.up * 2), -transform.forward * 50, Color.green);

            agent.SetDestination(transform.position + backDestinaion * attackDist);
            //agent.destination = transform.position - transform.forward * 5f;
            agent.speed = backSpeed;
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f)
            {
                if (agent.remainingDistance <= 0.5f)
                {
                    state = STATE.Attacking;
                }
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
            else if (dist <= meleeDist)
            {
                state = STATE.Backing;
            }
            else if (dist <= attackDist && dist > meleeDist)
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

    void LookAt()
    {
        if (isAttack != true)
        {
            animator.SetBool(hashMove, true);
            transform.rotation = Quaternion.Lerp(enemyTr.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
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
                    isMove = true;
                    isAttack = false;
                    patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Chase:
                    isMove = true;
                    isAttack = false;
                    traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Attacking:
                    agent.velocity = Vector3.zero;
                    LookAt();
                    Attack();
                    //animator.SetBool(hashMove, false);
                    break;
                case STATE.Backing:
                    isMove = true;
                    isAttack = false;
                    BackMove();
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.Die:
                    Die();
                    break;
            }
        }
    }
}
