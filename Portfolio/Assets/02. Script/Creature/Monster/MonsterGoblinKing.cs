using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterGoblinKing : MonsterBase
{
    [SerializeField] GameObject attackEffect;
    [SerializeField] GameObject tauntEffect;
    [SerializeField] GameObject tauntAttackGo;

    [SerializeField] GameObject pattunSpawner;

    PlayerInfo player;
    Transform playerTr;
    Inventory inven;

    public int nextIdx;                 //다음 순찰 지점의 인덱스
    public float minDist = -1f;          //최소 공격거리
    public float maxDist = 0f;
    public float attackDist = 6f;       //최대 공격거리
    public float traceDist = 15f;       //추적 거리

    public float attackRate = 0.2f;     //공격딜레이
    public float nextFire = 0f;

    public float patrolSpeed = 4f;      //일상속도
    public float traceSpeed = 10f;      //추적속도
    public float backSpeed = 20f;       //뒤로 도망가는 속도

    private int obstacleLayer;          //장애물 레이어
    private int playerLayer;            //플레이어 레이어
    private int monsterLayer;           //몬스터 레이어

    [SerializeField] Renderer meshRenderer;
    public bool isDie = false;
    public bool isAttack = false;
    public bool isHit = false;
    public bool isTaunt = false;
    public bool isRaze = false;

    float dist; //플레이어와 적의 거리
    Vector3 monsterTr => transform.position + (Vector3.up * 2);

    NavMeshAgent agent;
    MonsterAnim monsterAnim;
    KingPointCtrl kingPoint;

    [SerializeField] List<Collider> monsters = new List<Collider>();
    Coroutine checkState;

    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        agent = GetComponent<NavMeshAgent>();
        playerGo = GameObject.FindGameObjectWithTag("Player");
        player = playerGo.GetComponent<PlayerInfo>();
        inven = FindObjectOfType<Inventory>();

        kingPoint = FindObjectOfType<KingPointCtrl>();

        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        playerLayer = LayerMask.NameToLayer("Player");
        monsterLayer = LayerMask.NameToLayer("Monster");

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
        stats.s_Name = "고블린 킹";
    }

    private void OnEnable()
    {
        kingPoint.goblinKing = transform;

        StartCoroutine(Action());
        checkState = StartCoroutine(CheckState());

        minimapCube.SetActive(true);
        hpCanvas.SetActive(true);

        GetComponent<CapsuleCollider>().enabled = true;

        state = STATE.Patrol;

        isDie = false;
        isAnger = true;
        isRaze = false;

        meshRenderer.material.color = Color.white;

        exp = 75f;
        dropGold = 100 + Random.Range(0, 26);
        finalNormalAtk = 45f;
        finalMaxHp = 300f;
        finalNormalDef = 0f;
        curHp = finalMaxHp;
    }

    private void OnDisable()
    {
        pattunSpawner.SetActive(false);
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

        if (curHp <= (finalMaxHp / 2) && isRaze == false)
        {
            TauntMode();
        }
        else if (curHp <= (finalMaxHp * 3 / 4) && isTaunt == false)
        {
            isTaunt = true;
            monsterAnim.OnTaunt();
            for (int i = 0; i < movePoints.Count; i++)
                pattunSpawner.GetComponent<MonsterSpawner>().movepoints[i] = movePoints[i].gameObject;
            pattunSpawner.SetActive(true);
        }
    }

    public void MovePoint()
    {
        agent.enabled = true;

        if (agent.isPathStale)
            return;

        if (!isAttack)
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
        agent.enabled = true;

        if (agent.isPathStale)
            return;

        monsterAnim.OnMove(true, agent.speed);
        agent.speed = traceSpeed;
        if (isAttack == false)
        {
            agent.destination = _target;
            agent.isStopped = false;
        }
        else
        {
            return;
        }
    }

    public void Attack(Vector3 _target)
    {
        if (Physics.Raycast(transform.position + (Vector3.up * 2.5f), transform.forward, attackDist * 1.5f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //시야각
        {
            agent.enabled = true;
            agent.isStopped = false;
            Stop();
            int randomAttack = Random.Range(0, 10000);
            if (Time.time >= nextFire && !isAttack)
            {
                isAttack = true;

                if (isRaze == false)
                {
                    monsterAnim.OnAttack();
                }
                else
                {
                    int x = randomAttack <= 3500 ? 1 : 0;
                    switch (x)
                    {
                        case 0:
                            monsterAnim.OnAttack();
                            break;
                        case 1:
                            monsterAnim.OnComboAttack();
                            break;
                    }
                }
                nextFire = Time.time + attackRate + Random.Range(0.5f, 1f);
            }
        }
        else
        {
            monsterAnim.OnMove(true, agent.speed);
            agent.enabled = false;

            //agent.SetDestination(_target);
            //agent.speed = backSpeed;
            Vector3 dir = new Vector3(_target.x, 0, _target.z) - new Vector3(transform.position.x, 0, transform.position.z);
            dir = dir.normalized;
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

    public void TauntMode()
    {
        isRaze = true;
        monsterAnim.OnTaunt();
        tauntAttackGo.SetActive(true);
        meshRenderer.material.color = Color.red;
    }

    public override void Die()
    {
        monsterAnim.OnDie();

        movePoints.Clear();

        if (agent.enabled)
        {
            Stop();
        }
        isDie = true;
        isAttack = false;
        isHit = false;
        GetComponent<CapsuleCollider>().enabled = false;

        if (QuestManager.Instance.QuestDic["003"].State == 1 && QuestManager.Instance.quest3_Count < 1)
            QuestManager.Instance.quest3_Count++;
    }

    public override void DropItem()
    {
        int random = Random.Range(0, 10000);

        if (random >= 9500)
        {
            var item = ItemDatabase.instance.newItem("0000003");
            inven.GetItem(item);

            SystemText_ScrollView_Ctrl.Instance.PrintText(item.Name + " 을 획득했습니다.");
        }
        else if (random >= 9000)
        {
            var item = ItemDatabase.instance.newItem("0000007");
            inven.GetItem(item);

            SystemText_ScrollView_Ctrl.Instance.PrintText(item.Name + " 을 획득했습니다.");
        }
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage - finalNormalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;

            player.GetExp(exp);
            player.GetGold(dropGold);

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
    }

    public void ExitHitMotion()
    {
        isHit = false;
    }

    public void ExitAttackMotion()
    {
        isAttack = false;
    }

    public IEnumerator OnOffAttackEffect()
    {
        yield return new WaitForSeconds(0.4f);
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackEffect.SetActive(false);
    }

    public IEnumerator OnOffTauntEffect()
    {
        tauntEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        tauntEffect.SetActive(false);
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
