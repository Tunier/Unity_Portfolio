using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterHunter : MonsterBase
{
    [SerializeField]
    GameObject AttackEffect;

    GameObject playerGo;
    public GameObject minimapCube;
    public GameObject hpCanvas;

    PlayerInfo player;
    Transform playerTr;

    public GameObject group;            //몬스터별 무브포인트 기준 파일 넣어주기
    public int nextIdx;                 //다음 순찰 지점의 인덱스
    public float minDist = 4f;          //최소 공격거리
    public float maxDist = 10f;
    public float attackDist = 13f;       //최대 공격거리
    public float traceDist = 16f;       //추적 거리

    public float attackRate = 0.2f;     //공격딜레이
    public float shotAfterTime = 0f;

    public float patrolSpeed = 4f;      //일상속도
    public float traceSpeed = 10f;      //추적속도
    public float backSpeed = 20f;       //뒤로 도망가는 속도

    private int obstacleLayer;          //장애물 레이어
    private int playerLayer;            //플레이어 레이어
    private int monsterLayer;           //몬스터 레이어

    private Collider monsterCollider;

    public bool isDie = false;
    public bool isAttack = false;
    public bool isHit = false;

    float dist; //플레이어와 적의 거리
    Vector3 monsterTr;
    NavMeshAgent agent;
    MonsterAnim monsterAnim;

    //public Collider[] monsters; //테스트
    public List<Collider> monsters = new List<Collider>();
    public Coroutine checkState;
    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        monsterCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        playerGo = GameObject.FindGameObjectWithTag("Player");
        player = playerGo.GetComponent<PlayerInfo>();

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

        if (playerGo != null)
        {
            playerTr = playerGo.GetComponent<Transform>();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Action());
        checkState = StartCoroutine(CheckState());
        GetComponent<CapsuleCollider>().enabled = true;

        isAnger = true;

        state = STATE.Patrol;

        exp = 30f;
        dropGold = 30;
        finalNormalAtk = 40f;
        finalMaxHp = 70f;
        finalNormalDef = 0;
        curHp = finalMaxHp;
    }

    private void Update()
    {
        foreach (var obj in monsters)
        {
            if (obj == null)
            {
                monsters.Remove(obj);
            }
        }
        dist = Vector3.Distance(playerTr.position, transform.position);

        shotAfterTime += Time.deltaTime;
    }
    public void MovePoint()
    {
        if (agent.isPathStale)
            return;

        if (isAttack == false)
        {
            agent.isStopped = false;
            agent.destination = movePoints[nextIdx].position;
            agent.speed = patrolSpeed;
            monsterAnim.OnMove(true, agent.speed);

            if (agent.velocity.magnitude < 1.5f && agent.remainingDistance <= 1.5f)
            {
                nextIdx = Random.Range(0, movePoints.Count);
            }
        }
    }

    public void Chase(Vector3 _target)
    {
        if (agent.isPathStale)
            return;

        agent.speed = traceSpeed;
        if (isAttack == false)
        {
            monsterAnim.OnMove(true, agent.speed);
            agent.destination = _target;
            agent.isStopped = false;
        }
    }

    public void Attack(Vector3 _target)
    {
        
        if (Physics.Raycast(transform.position + (Vector3.up * 2.5f), transform.forward, attackDist * 1.1f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //시야각
        {
            agent.enabled = true;
            Stop();

            if (isAttack == false)
            {
                isAttack = true;
            }

            if (shotAfterTime >= attackRate)
            {
                shotAfterTime = 0;
                monsterAnim.OnAttack();
            }
        }
        else
        {
            monsterAnim.OnMove(true, agent.speed);
            agent.enabled = false;

            //agent.SetDestination(_target);
            //agent.speed = backSpeed;
            Vector3 dir = (_target - transform.position).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 120f);
        }
    }

    public void BackMove(Vector3 _target)
    {
        agent.isStopped = false;
        monsterAnim.OnMove(true, agent.speed);
        Vector3 dir = (transform.position - _target).normalized;
        if (Physics.Raycast(monsterTr, -transform.forward, 5f, 1 << obstacleLayer))
        {
            Debug.Log("옵스타클갑지");
            //뒤로 무빙할때 옵스타클 피하는 위치를 목적지로 설정하는 함수 넣기
        }
        else if(!isAttack && !isHit )
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
        monsterAnim.OnDie();
        monsterAnim.OnDieIdx();

        if (agent.enabled)
        {
            Stop();
        }
        isDie = true;
        isAttack = false;
        GetComponent<CapsuleCollider>().enabled = false;

        minimapCube.SetActive(false);
    }

    public override void DropItem()
    {
    }

    public override void Hit(float _damage)
    {
        isHit = true;
        Stop();
        curHp -= _damage - finalNormalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;

            player.stats.CurExp += exp;
            player.stats.Gold += dropGold;

            if (player.stats.CurExp > player.stats.MaxExp)
                player.LevelUp();

            StopCoroutine(checkState);

            return;
        }

        var ary_monster = Physics.OverlapSphere(transform.position, traceDist * 3, 1 << monsterLayer);

        foreach (var monster in ary_monster)
        {
            if (!monsters.Contains(monster))
            {
                monsters.Add(monster);
            }
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == null)
                continue;
            else
            {
                var mob = monsters[i].GetComponent<MonsterBase>();

                if (mob.isAnger == false)
                {
                    mob.isAnger = true;
                }
            }
        }

        monsterAnim.OnHit();
    }

    public void ExitAttackMotion()
    {
        isAttack = false;
    }

    public void ExitHitMotion()
    {
        isHit = false;
    }

    public void OnOffAttackEffect()
    {
        AttackEffect.SetActive(!AttackEffect.activeSelf);
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(0.1f);

        while (state != STATE.Die || curHp >= 0) //죽지않고 Anger가 true일때 상태변화
        {
            if (isAnger)
            {
                if (dist <= maxDist)
                {
                    if (dist < minDist)
                    {
                        agent.enabled = true;
                        state = STATE.Backing;
                    }
                }
                else if (dist <= attackDist && dist > maxDist)
                {
                    state = STATE.Attacking;
                }
                else if (dist <= traceDist && dist > attackDist)
                {
                    agent.enabled = true;
                    state = STATE.Chase;
                }
                else
                {
                    agent.enabled = true;
                    state = STATE.Patrol;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 스테이터스에 따른 애니메이션,행동을 제어하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            switch (state)
            {
                case STATE.Patrol:
                    MovePoint();
                    break;
                case STATE.Chase:
                    Chase(playerTr.position);
                    break;
                case STATE.Attacking:
                    agent.velocity = Vector3.zero;
                    Attack(playerTr.position);
                    break;
                case STATE.Backing:
                    BackMove(playerTr.position);
                    break;
                case STATE.Die:
                    Die();
                    DropItem();
                    hpCanvas.SetActive(false);
                    minimapCube.SetActive(false);
                    StartCoroutine(co_MonsterDie());
                    yield break;
            }

            if (state == STATE.Die)
                break;

            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator co_MonsterDie()
    {
        yield return new WaitForSeconds(3f);

        spawner.spawnCount--;
        gameObject.SetActive(false);
    }
}
