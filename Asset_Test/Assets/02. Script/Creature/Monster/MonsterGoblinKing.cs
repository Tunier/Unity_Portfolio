using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterGoblinKing : MonsterBase
{
    [SerializeField]
    GameObject AttackEffect1;
    [SerializeField]
    GameObject AttackEffect2;

    GameObject playerGo;
    public GameObject minimapCube;
    public GameObject hpCanvas;

    PlayerInfo player;
    Transform playerTr;

    public GameObject group;            //���ͺ� ��������Ʈ ���� ���� �־��ֱ�
    public int nextIdx;                 //���� ���� ������ �ε���
    public float minDist = -1f;          //�ּ� ���ݰŸ�
    public float maxDist = 0f;
    public float attackDist = 4f;       //�ִ� ���ݰŸ�
    public float traceDist = 15f;       //���� �Ÿ�

    public float attackRate = 0.2f;     //���ݵ�����
    public float nextFire = 0f;

    public float patrolSpeed = 4f;      //�ϻ�ӵ�
    public float traceSpeed = 10f;      //�����ӵ�
    public float backSpeed = 20f;       //�ڷ� �������� �ӵ�

    private int obstacleLayer;          //��ֹ� ���̾�
    private int playerLayer;            //�÷��̾� ���̾�
    private int monsterLayer;           //���� ���̾�

    private Collider monsterCollider;

    public bool isDie = false;
    public bool isAttack = true;

    float dist; //�÷��̾�� ���� �Ÿ�
    Vector3 monsterTr;
    NavMeshAgent agent;
    MonsterAnim monsterAnim;

    //public Collider[] monsters; //�׽�Ʈ
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

        state = STATE.Patrol;

        isAnger = true;

        exp = 75f;
        dropGold = 60 + Random.Range(0, 6);
        finalMaxHp = 125f;
        finalNormalDef = 0f;
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
    }

    public void MovePoint()
    {
        agent.enabled = true;
        isAttack = false;

        if (agent.isPathStale)
            return;

        agent.isStopped = false;
        agent.destination = movePoints[nextIdx].position;
        agent.speed = patrolSpeed;
        monsterAnim.OnMove(true, agent.speed);

        if (agent.velocity.magnitude < 1.5f && agent.remainingDistance <= 1.5f)
        {
            nextIdx = Random.Range(0, movePoints.Count);
        }
    }

    public void Chase(Vector3 _target)
    {
        agent.enabled = true;
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
        if (Physics.Raycast(transform.position + (Vector3.up * 2.5f), transform.forward, attackDist * 1.5f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //�þ߰�
        {
            agent.enabled = true;
            Stop();

            if (isAttack == false)
            {
                isAttack = true;
            }
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
            agent.enabled = false;

            //agent.SetDestination(_target);
            //agent.speed = backSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 120f);

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
        monsterAnim.OnMove(false, agent.speed);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public override void Die()
    {
        monsterAnim.OnDie();

        Stop();
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

    public void OnOffAttackEffect()
    {
        AttackEffect1.SetActive(!AttackEffect1.activeSelf);
    }

    public void OnOffAttackEffect2()
    {
        AttackEffect2.SetActive(!AttackEffect2.activeSelf);
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(0.1f);

        while (state != STATE.Die || curHp >= 0) //�����ʰ� Anger�� true�϶� ���º�ȭ
        {
            if (isAnger)
            {
                if (dist <= maxDist)
                {
                    if (dist < minDist)
                    {
                        state = STATE.Backing;
                    }
                }
                else if (dist <= attackDist && dist > maxDist)
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
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// �������ͽ��� ���� �ִϸ��̼�,�ൿ�� �����ϴ� �ڷ�ƾ �Լ�
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
